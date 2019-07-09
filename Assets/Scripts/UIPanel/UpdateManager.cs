﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using LeoHui;
using System.Net;
using SevenZip.Compression.LZMA;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;
    private string versionFile = "version.txt";
    private string resourceFile = "resource.csv";
    private ResourceData localResourceData;
    private ResourceData remoteResourceData;
    private List<string> downloadList = new List<string>();
    private int downloadRetryCount;
    private int downloadFileIndex;
    private int decompressFileIndex;
    private int totalFileCount;
	private long finishFileSize;//已下载文件的文件大小
	private long totalFileSize;//所有需要更新的文件大小

    public Action<bool> finishCallback = null;
	public Action<int, int> downloadUpdate = null;
	public Action<int, int> decompressUpdate = null;
	public Action<long, long> sizeUpdate = null;

    public string serverUrl= "https://xiaochu.gyatechnology.com:3993/cardrift/";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        CheckExtractResource(); //释放资源
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.AppFrame;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void CheckExtractResource()
    {
        bool isExists = Directory.Exists(PathTools.DataPath)
           && File.Exists(PathTools.DataPath + versionFile) && File.Exists(PathTools.DataPath + resourceFile);
        if (isExists)
        {
            CheckUpdateResource();
            return;   //文件已经解压过了，自己可添加检查文件列表逻辑
        }
        StartCoroutine(OnExtractResource());    //启动释放协成
    }

    IEnumerator OnExtractResource()
    {
        if (Directory.Exists(PathTools.DataPath))
            Directory.Delete(PathTools.DataPath, true);
        Directory.CreateDirectory(PathTools.DataPath);

        yield return StartCoroutine(OnExtractFile(versionFile));
        yield return StartCoroutine(OnExtractFile(resourceFile));

        localResourceData = new ResourceData();
        localResourceData.InitDataFromFile(PathTools.DataPath + resourceFile);
        int dataRow = localResourceData.GetDataRow();
        string fullName = string.Empty;
        for (int i=1; i<=dataRow; ++i)
        {
            fullName = localResourceData.GetBundleFullName(i);
            yield return StartCoroutine(OnExtractFile(fullName));
            if (decompressUpdate != null)
                decompressUpdate(i, dataRow);
        }

        //释放完成，开始启动更新资源
        CheckUpdateResource();
    }

    /// <summary>
    /// 从StreamingAssets解压缩文件到数据目录
    /// </summary>
    /// <returns></returns>
    IEnumerator OnExtractFile(string fileName)
    {
        //DataPath  游戏数据目录
        //ResPath   游戏资源目录
        string infile = PathTools.ResPath + fileName;
        string outfile = PathTools.DataPath + fileName;

        string dir = Path.GetDirectoryName(outfile);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        if (File.Exists(outfile))
            File.Delete(outfile);

        //StreamingAssets路径下，Android平台不支持C#的系统函数获取文件数据，只能使用WWW方式
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;
            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return null;
        }
        else
        {
            File.Copy(infile, outfile, true);
        }
    }

    private void ReadVersionFile(ref int localVersionCode, ref string localVersionName)
    {
        string filePath = PathTools.DataPath + versionFile;
        if (File.Exists(filePath))
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            localVersionCode = int.Parse(sr.ReadLine());
            localVersionName = sr.ReadLine();
            sr.Close();
            fs.Close();
        }
    }

    /// <summary>
    /// 启动更新下载
    /// </summary>
    void CheckUpdateResource()
    {
        if (!AppConst.UpdateMode)
        {
            EndUpdateResource();
            return;
        }
        StartCoroutine(CheckVersionFile());
    }

    IEnumerator CheckVersionFile()
    {
        WWW www = new WWW(serverUrl + versionFile);
        yield return www;
        if(!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            EndUpdateResource();
            yield break;
        }
        string result = www.text;
        string[] dataArr = result.Split('\n');
        int remoteVersionCode = int.Parse(dataArr[0]);
        string remoteVersionName = dataArr[1];

        int localVersionCode = 1;
        string localVersionName = "1.0.0";
        ReadVersionFile(ref localVersionCode, ref localVersionName);

        if(remoteVersionCode > localVersionCode)
        {
            StartCoroutine(CheckResourceFile());
        }
        else
        {
            Debug.Log("已经是最新版本了");
            EndUpdateResource();
        }
    }

    IEnumerator CheckResourceFile()
    {
        WWW www = new WWW(serverUrl + resourceFile);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            EndUpdateResource();
            yield break;
        }
        string result = www.text;
        remoteResourceData = new ResourceData();
        remoteResourceData.InitData(result);
        localResourceData = new ResourceData();
        localResourceData.InitDataFromFile(PathTools.DataPath + resourceFile);
        Debug.Log(localResourceData.GetDataRow());

        downloadList.Clear();
        int dataRow = remoteResourceData.GetDataRow();
        for(int i=1; i<=dataRow; ++i)
        {
            string bundleName = remoteResourceData.GetBundleName(i);
            string remoteMd5 = remoteResourceData.GetMd5(i);
            string localMd5 = localResourceData.GetMd5ByBundleName(bundleName);
            Debug.Log(remoteMd5 + " : " + localMd5);
            if(remoteMd5.CompareTo(localMd5) != 0)
            {
                downloadList.Add(bundleName);
            }
        }
        if(downloadList.Count > 0)
        {
            downloadFileIndex = 0;
            downloadRetryCount = 0;
            totalFileCount = downloadList.Count;
            Debug.Log(totalFileCount);
            finishFileSize = 0;
            GetTotalFileSize();
			DownloadNextFile();
        }
    }

    void GetTotalFileSize()
    {
        totalFileSize = 0;
        for(int i=0; i<downloadList.Count; ++i)
        {
            totalFileSize += remoteResourceData.GetSizeByBundleName(downloadList[i]);
        }
    }

    void DownloadNextFile()
    {
        //下载完成
		if (downloadFileIndex >= totalFileCount)
        {
            FinishDownloadFile();
            return;
        }
        //更新进度
        if(downloadUpdate != null)
		{
			downloadUpdate(downloadFileIndex+1, totalFileCount);
		}

        string bundleName = downloadList[downloadFileIndex];
        string bundleFullName = remoteResourceData.GetBundleFullNameByBundleName(bundleName);
        string localFilePath = PathTools.DataPath + bundleFullName;
        string remoteFilePath = serverUrl + bundleFullName;
        Debug.Log(remoteFilePath);
        float progress = 0f;

        //开启子线程下载,使用匿名方法
        Loom.RunAsync(() =>
		{
            //使用流操作文件
            FileStream fs = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            //获取文件现在的长度
            long fileLength = fs.Length;
            //获取下载文件的总长度
            long totalLength = remoteResourceData.GetSizeByBundleName(bundleName);

            //如果没下载完
            if (fileLength < totalLength)
            {
                //断点续传核心，设置本地文件流的起始位置
                fs.Seek(fileLength, SeekOrigin.Begin);

                HttpWebRequest request = HttpWebRequest.Create(remoteFilePath) as HttpWebRequest;

                //断点续传核心，设置远程访问文件流的起始位置
                request.AddRange((int)fileLength);
                Stream stream = request.GetResponse().GetResponseStream();

                byte[] buffer = new byte[1024];
                //使用流读取内容到buffer中
                //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
                int length = stream.Read(buffer, 0, buffer.Length);
                while (length > 0)
                {
                    //将内容再写入本地文件中
                    fs.Write(buffer, 0, length);
                    //计算进度
                    fileLength += length;
                    progress = (float)fileLength / (float)totalLength;
                    //类似尾递归
                    length = stream.Read(buffer, 0, buffer.Length);

                    //更新下载大小
                    Loom.QueueOnMainThread(() =>
                    {
                        if(sizeUpdate != null)
                        {
                            sizeUpdate(finishFileSize + fileLength, totalFileSize);
                        }
                    });
                }
                stream.Close();
                stream.Dispose();
            }
            else
            {
                progress = 1;
            }

            //如果下载完毕，执行回调
            if (progress == 1)
            {
                Debug.Log(bundleName + " Download finished!");
                finishFileSize += remoteResourceData.GetSizeByBundleName(bundleName);
                ++downloadFileIndex;
                DownloadNextFile();
            }
            else
            {
                //多次重试 失败后 直接开始游戏
                if (downloadRetryCount > 4)
                {
                    Loom.QueueOnMainThread(() =>
                    {
                        EndUpdateResource();
                    });
                    return;
                }

                //下载出错了  再次下载
                Loom.QueueOnMainThread(() =>
                {
                    ++downloadRetryCount;
                    DownloadNextFile();
                });
                return;
            }
        });
	}

    void FinishDownloadFile()
    {
        Debug.Log("FinishDownloadFile");
    }

    void EndUpdateResource()
    {
        if (finishCallback != null)
            finishCallback(false);
    }
}
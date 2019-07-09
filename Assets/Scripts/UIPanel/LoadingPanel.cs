using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SevenZip.Compression.LZMA;

public class LoadingPanel : MonoBehaviour
{
    private string versionFile = "version.txt";
    private string resourceFile = "resource.csv";
    private ResourceData localResourceData;
    private ResourceData remoteResourceData;

    void Awake()
    {

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
            StartCoroutine(CheckUpdateResource());
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
        }

        //释放完成，开始启动更新资源
        StartCoroutine(CheckUpdateResource());
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

    /// <summary>
    /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
    /// </summary>
    IEnumerator CheckUpdateResource()
    {
        if (!AppConst.UpdateMode)
        {
            OnResourceInited();
            yield break;
        }
        yield return null;
    }

    IEnumerator OnUpdateResource()
    {
        yield return null;
    }

    /// <summary>
    /// 资源初始化结束
    /// </summary>
    public void OnResourceInited()
    {
       
    }
}
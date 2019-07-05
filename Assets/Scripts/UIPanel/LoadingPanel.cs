using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using U3DEventFrame;
using UnityEngine.UI;

public class LoadingPanel : UIBase
{
    private List<DownloadRequest> downloadList = new List<DownloadRequest>();
    private List<string> finishList = new List<string>();
    private long totalSize = 0;//需要更新的文件大小
    private long curSize = 0;//当前已更新的文件大小
    private int totalFileCount = 0;//需要更新的文件数量
    private int curFileCount = 0;//当前已更新的文件数量

    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)UIEvent.Update_Message,
            (ushort)UIEvent.Download,
            (ushort)UIEvent.LoadFinish
        };
        RegistSelf(this, msgIds);
    }

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegistSelf(this, msgIds);
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)UIEvent.Update_Message:
                StringMsg stringMsg = (StringMsg)tmpMsg;
                ShowMessage(stringMsg.data);
                print(stringMsg.data);
                break;
            case (ushort)UIEvent.Download:
                DownloadMsg msg = (DownloadMsg)tmpMsg;
                switch(msg.msgType)
                {
                    case "FinishEvent":
                        OnDownloadCompleted(msg.fileName, msg.fileSize);
                        break;
                    case "ProgressEvent":
                        OnDownloadProgress(msg.recvSize, msg.fileSize);
                        break;
                }
                break;
            case (ushort)UIEvent.LoadFinish:
                gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        GetGameObject("UpdatePanel").SetActive(false);
        GetUIComponent<UIBehaviour>("BtnUpdate").AddButtonListener(UpdateOnClick);
        GetUIComponent<UIBehaviour>("BtnCancel").AddButtonListener(CancelOnClick);

        CheckExtractResource(); //释放资源
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.AppFrame;
    }

    private void UpdateOnClick()
    {
        GetGameObject("UpdatePanel").SetActive(false);
        StartCoroutine(OnUpdateResource());    //启动更新协成
    }

    private void CancelOnClick()
    {
        OnResourceInited();
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void CheckExtractResource()
    {
        bool isExists = Directory.Exists(PathTools.DataPath) &&
          Directory.Exists(PathTools.DataPath + "lua/") && File.Exists(PathTools.DataPath + "files.txt");
        if (isExists)
        {
            StartCoroutine(CheckUpdateResource());
            return;   //文件已经解压过了，自己可添加检查文件列表逻辑
        }
        StartCoroutine(OnExtractResource());    //启动释放协成
    }

    IEnumerator OnExtractResource()
    {
        string dataPath = PathTools.DataPath;  //游戏数据目录
        string resPath = PathTools.AssetBundlePath;    //游戏资源目录

        if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
        Directory.CreateDirectory(dataPath);

        string infile = resPath + "files.txt";
        string outfile = dataPath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        ShowMessage("正在检查资源中...");

        //StreamingAssets路径下，Android平台不支持C#的系统函数获取文件数据，只能使用WWW方式
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else
        {
            File.Copy(infile, outfile, true);
        }
        yield return new WaitForEndOfFrame();

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        for (int i = 0; i < files.Length; ++i)
        {
            string file = files[i];
            string[] fs = file.Split('|');
            infile = resPath + fs[0];
            outfile = dataPath + fs[0];

            ShowMessage("正在解包文件:>" + fs[0]);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();

            GetUIComponent<Text>("MessageText").text = (i+1) + " : " + files.Length;
            GetUIComponent<Image>("ProgressImage").fillAmount = (i + 1) * 1.0f / files.Length;
        }

        ShowMessage("解包完成!!!");

        yield return new WaitForSeconds(0.1f);

        //释放完成，开始启动更新资源
        StartCoroutine(CheckUpdateResource());
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
        string dataPath = PathTools.DataPath;  //数据目录
        string url = AppConst.AppUrl;
        string message = string.Empty;
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt?v=" + random;
        Debug.LogWarning("LoadUpdate---->>>" + listUrl);

        WWW www = new WWW(listUrl); yield return www;
        if (www.error != null)
        {
            OnUpdateFailed(string.Empty);
            yield break;
        }
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        File.WriteAllBytes(dataPath + "files.txt", www.bytes);
        string filesText = www.text;
        string[] files = filesText.Split('\n');
        totalSize = 0;
        curSize = 0;
        for (int i = 0; i < files.Length; i++)
        {
            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string f = keyValue[0];
            string localfile = (dataPath + f).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + f + "?v=" + random;
            bool canUpdate = !File.Exists(localfile);
            Debug.Log(localfile);
            Debug.Log(!File.Exists(localfile));
            if (!canUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = UtilTools.md5file(localfile);
                canUpdate = !remoteMd5.Equals(localMd5);
                if (canUpdate) File.Delete(localfile);
            }
            if (canUpdate)
            {
                totalSize += long.Parse(keyValue[2]);
                DownloadRequest downloadRequest = new DownloadRequest(localfile, fileUrl);
                downloadList.Add(downloadRequest);
            }
        }

        if(totalSize >0)
        {
            GetUIComponent<Text>("DescText").text = "更新需要下载：" + UtilTools.getFileSizeFormat(totalSize);
            GetGameObject("UpdatePanel").SetActive(true);
        } else
        {
            OnResourceInited();
        }
    }

    IEnumerator OnUpdateResource()
    {
        totalFileCount = downloadList.Count;
        curFileCount = 0;
        for (int i = 0; i < totalFileCount; ++i)
        {
            DownloadRequest downloadRequest = downloadList[i];

            //这里都是资源文件，用线程下载
            BeginDownload(downloadRequest);
            while (!(IsDownOK(downloadRequest.fileName)))
            {
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForEndOfFrame();

        OnResourceInited();
    }

    void OnUpdateFailed(string file)
    {
        ShowMessage("更新失败!>" + file);
    }

    /// <summary>
    /// 是否下载完成
    /// </summary>
    bool IsDownOK(string file)
    {
        return finishList.Contains(file);
    }

    /// <summary>
    /// 线程下载
    /// </summary>
    void BeginDownload(DownloadRequest downloadRequest)
    {
        //线程下载
        ObjectMsg<DownloadRequest> arrayMsg = new ObjectMsg<DownloadRequest>((ushort)UIEvent.Download_Request, downloadRequest);
        SendMsg(arrayMsg);
    }

    /// <summary>
    /// 线程下载文件完成
    /// </summary>
    /// <param name="fileName"></param>
    void OnDownloadCompleted(string fileName, long fileSize)
    {
        if (finishList.Contains(fileName))
            return;
        ++curFileCount;
        curSize += fileSize;
        Debug.Log(curSize + " : " + totalSize);
        //下载一个完成
        finishList.Add(fileName);

        GetUIComponent<Text>("MessageText").text = UtilTools.getFileSizeFormat(curSize) + " : " + UtilTools.getFileSizeFormat(totalSize);
        GetUIComponent<Image>("ProgressImage").fillAmount = curSize * 1.0f / totalSize;
    }

    void OnDownloadProgress(long recvSize, long fileSize)
    {
        long curRecvSize = curSize + recvSize;
        Debug.Log(curRecvSize + " : " + totalSize);

        GetUIComponent<Text>("MessageText").text = UtilTools.getFileSizeFormat(curRecvSize) + " : " + UtilTools.getFileSizeFormat(totalSize);
        GetUIComponent<Image>("ProgressImage").fillAmount = curRecvSize * 1.0f / totalSize;
    }

    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="message">Message.</param>
    private void ShowMessage(string message)
    {
        GetUIComponent<Text>("MessageText").text = message;
    }

    /// <summary>
    /// 资源初始化结束
    /// </summary>
    public void OnResourceInited()
    {
        gameObject.AddComponent<LuaClient>();
        GetGameObject("ProgressBg").SetActive(false);
        GetGameObject("ProgressImage").SetActive(false);
        ShowMessage("初始化...");
    }
}
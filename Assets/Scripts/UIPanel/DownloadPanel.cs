using U3DEventFrame;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine.UI;

public class DownloadRequest
{
    public string fileName;
    public string fileUrl;

    public DownloadRequest(string fileName, string fileUrl)
    {
        this.fileName = fileName;
        this.fileUrl = fileUrl;
    }
}

public class DownloadPanel : UIBase
{
    private Thread thread;
    private Stopwatch sw = new Stopwatch();
    private string curDownFile = string.Empty;
    static readonly object request_lockObject = new object();
    static Queue<DownloadRequest> requestQueue = new Queue<DownloadRequest>();

    static readonly object message_lockObject = new object();
    static Queue<DownloadMsg> messageQueue = new Queue<DownloadMsg>();

    delegate void FinishSyncEvent(string fileName, long fileSize);
    private FinishSyncEvent finishSyncEvent;

    delegate void ProgressSyncEvent(long recvSize, long fileSize);
    private ProgressSyncEvent progressSyncEvent;

    void Awake()
    {
        finishSyncEvent = OnFinishSyncEvent;
        progressSyncEvent = OnProgressSyncEvent;
        thread = new Thread(ThreadUpdate);
        msgIds = new ushort[]
        {
            (ushort)UIEvent.Download_Request
        };
        RegistSelf(this, msgIds);
    }

    void Start()
    {
        thread.Start();
    }

    void Update()
    {
        lock(message_lockObject)
        {
            if(messageQueue.Count > 0)
            {
                SendMsg(messageQueue.Dequeue());
            }
        }
    }

    /// <summary>
    /// 应用程序退出
    /// </summary>
    void OnDestroy()
    {
        thread.Abort();
        if (msgIds != null)
        {
            UnRegistSelf(this, msgIds);
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch(tmpMsg.msgId)
        {
            case (ushort)UIEvent.Download_Request:
                ObjectMsg<DownloadRequest> msg = (ObjectMsg<DownloadRequest>)tmpMsg;
                lock(request_lockObject)
                {
                    requestQueue.Enqueue(msg.data);
                }
                break;
        }
    }

    /// <summary>
    /// 通知事件
    /// </summary>
    private void OnFinishSyncEvent(string fileName, long fileSize)
    {
        lock (message_lockObject)
        {
            DownloadMsg message = new DownloadMsg((ushort)UIEvent.Download, "FinishEvent", fileName, 0, fileSize);
            messageQueue.Enqueue(message);
        }
    }

    /// <summary>
    /// 通知事件
    /// </summary>
    private void OnProgressSyncEvent(long recvSize, long fileSize)
    {
        lock (message_lockObject)
        {
            DownloadMsg message = new DownloadMsg((ushort)UIEvent.Download, "ProgressEvent", "", recvSize, fileSize);
            messageQueue.Enqueue(message);
        }
    }

    void ThreadUpdate()
    {
        while (true)
        {
            lock (request_lockObject)
            {
                if (requestQueue.Count > 0)
                {
                    DownloadRequest request = requestQueue.Dequeue();
                    OnDownloadFile(request.fileName, request.fileUrl);
                }
            }
            Thread.Sleep(1);
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    void OnDownloadFile(string fileName, string fileUrl)
    {
        curDownFile = fileName;
        using (WebClient client = new WebClient())
        {
            sw.Start();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            client.DownloadFileAsync(new System.Uri(fileUrl), fileName);
        }
    }

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string message = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
        if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        {
            sw.Reset();

            if (finishSyncEvent != null)
                finishSyncEvent(curDownFile, e.TotalBytesToReceive);
        } else
        {
            if (progressSyncEvent != null)
                progressSyncEvent(e.BytesReceived, e.TotalBytesToReceive);
        }
    }
}

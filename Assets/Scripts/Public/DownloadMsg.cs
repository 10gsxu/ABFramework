using U3DEventFrame;

public class DownloadMsg : MsgBase
{
    public string msgType;
    public string fileName;
    public long recvSize;
    public long fileSize;

    public DownloadMsg(ushort msgId, string msgType, string fileName, long recvSize, long fileSize)
    {
        this.msgId = msgId;
        this.msgType = msgType;
        this.fileName = fileName;
        this.recvSize = recvSize;
        this.fileSize = fileSize;
    }
}
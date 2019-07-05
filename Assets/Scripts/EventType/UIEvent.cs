public enum UIEvent
{
    Update_Message = ManagerID.UIManager + 1,
    Download_Request,//下载请求
    Download,//下载进度&下载完成
    LoadFinish,//加载完成

    GameWin = ManagerID.LUIManager + 5,
    GameLose = ManagerID.LUIManager + 6,
    GameReborn = ManagerID.LUIManager + 7,
    GameProgress = ManagerID.LUIManager + 8,
}
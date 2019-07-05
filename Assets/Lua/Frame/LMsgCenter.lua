LMsgCenter = {
    managerDict = {}
}
LMsgCenter.__index = LMsgCenter;
local this = LMsgCenter;

function LMsgCenter.Awake()
    --绑定C#发送Msg到Lua中的回调函数
    LuaAndCMsgCenter.Instance:SettingLuaCallBack(this.RecvMsg);
    this.managerDict[LManagerID.LUIManager] = LUIManager;
    this.managerDict[LManagerID.LAssetManager] = LAssetManager;
    this.managerDict[LManagerID.LDataManager] = LDataManager;
end

function LMsgCenter.SendMsg(msg)
    this.ProcessEvent(msg);
end

function LMsgCenter.RecvMsg(msg)
    --C# Msg, 在各自的Manger中自行解析
    this.ProcessEvent(msg);
end

--msg,有可能是C#Msg,也有可能是LuaMsg
function LMsgCenter.ProcessEvent(msg)
    if msg.msgId < MsgStart then
        curManager = this.managerDict[msg:GetManager()];
        curManager:ProcessEvent(msg);
    else
        MsgCenter.Instance:ProcessEvent(msg);
    end
end
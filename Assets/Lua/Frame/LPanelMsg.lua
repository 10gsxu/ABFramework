LPanelMsg = LMsgBase:New();

LPanelMsg.__index = LPanelMsg;

function LPanelMsg:New(msgId, panelName)
    local self = {};
    setmetatable(self, LPanelMsg);

    self.msgId = msgId;
    self.panelName = panelName;

    return self;
end
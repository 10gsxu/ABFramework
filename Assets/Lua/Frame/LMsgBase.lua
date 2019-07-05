LMsgBase = {
    msgId = 0
}
LMsgBase.__index = LMsgBase;

function LMsgBase:New (msgid)
    local self = {};
    setmetatable(self, LMsgBase);
    self.msgId = msgid;
    return self;
end

function LMsgBase:GetManager()
    if self.msgId > MsgStart then
        local tmpId = math.floor((self.msgId - MsgStart) / MsgSpan * MsgSpan);
        return math.ceil(tmpId);
    else
        local tmpId = math.floor(self.msgId/MsgSpan)*MsgSpan;
        return math.ceil(tmpId);
    end
end
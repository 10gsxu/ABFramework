LManagerBase = {
    eventTree = {}
};
LManagerBase.__index = LManagerBase;
local this = LManagerBase;

function LManagerBase:New()
    local self = {};
    setmetatable(self, LManagerBase);

    self.eventTree = {};

    return self;
end

function LManagerBase:RegistMsgs(script, msgs)
    for i, v in pairs(msgs) do
        eventNode = LEventNode:New(script);
        self:RegistMsg(v, eventNode);
    end
end

function LManagerBase:RegistMsg(id, eventNode)
    if this:FindKey(self.eventTree, id) then
        tmpNode = self.eventTree[id];
        while tmpNode.next ~= nil do
            tmpNode = tmpNode.next;
        end
        tmpNode.next = eventNode;
    else
        self.eventTree[id] = eventNode;
    end
end

function LManagerBase:UnRegistMsgs(script, ...)
    if arg == nil then
        return
    end
    for i in arg do
        self:UnRegistMsg(script, i);
    end
end

function LManagerBase:UnRegistMsg(script, id)
    if this:FindKey(self.eventTree, id) then
        tmpNode = self.eventTree[id];
        if tmpNode.data == script then
            if tmpNode.next ~= nil then
                self.eventTree[id] = tmpNode.next;
                tmpNode.next = nil;
            end
        else
            while tmpNode.next ~= nil and tmpNode.next.data ~= script do
                tmpNode = tmpNode.next;
            end
            if tmpNode.next.next ~= nil then
                curNode = tmpNode.next;
                tmpNode.next = curNode.next;
                curNode.next = nil;
            else
                tmpNode.next = nil;
            end
        end
    end
end

function LManagerBase:ProcessEvent (msg)
    if this:FindKey(self.eventTree, msg.msgId) then
        local tmpNode = self.eventTree[msg.msgId];
        while tmpNode ~= nil do
            tmpNode.data:ProcessEvent(msg);
            tmpNode = tmpNode.next;
        end
    else
        print("Msg not contain msg ==="..msg.msgId);
    end
end

-- 是否包含Key
function LManagerBase:FindKey (dict, key)
    for k, v in pairs(dict) do
        if k == key then
            return true;
        end
    end
    return false;
end

function LManagerBase:Destroy()
    keys= {}
    keyCount = 0;
    for k, v in pairs(self.eventTree) do
        keys[keyCount] = k;
        keyCount = keyCount + 1;
    end
    for i=1, keyCount do
        self.eventTree[keys[i]] = nil;
    end
end
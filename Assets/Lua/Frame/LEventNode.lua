LEventNode = {
    data = nil,
    next = nil
}
LEventNode.__index = LEventNode;

function LEventNode:New(script)
    local self = {};
    setmetatable(self, LEventNode);
    self.data = script;
    self.next = nil
    return self;
end
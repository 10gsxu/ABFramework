#NAME# = {}
#NAME#.__index = #NAME#;

function #NAME#:New()
    local self = {};
    setmetatable(self, #NAME#);
    
    return self;
end
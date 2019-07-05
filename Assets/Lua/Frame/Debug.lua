Debug = {}
Debug.__index = Debug;

function Debug:New()
    local self = {};
    setmetatable(self, Debug);
    
    return self;
end

function Debug.LogTable(value)
	for k, v in pairs(value) do
		print(k .. " : " .. type(v));
	end
end

function Debug.Log(value)
    print(value);
end
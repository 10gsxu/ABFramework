LUIBase = {
    msgIds = {}
}
LUIBase.__index = LUIBase;

function LUIBase:New()
    local self = {};
    setmetatable(self, LUIBase);
    self.msgIds = {};
    return self;
end

function LUIBase:RegistSelf(script, msgs)
    LUIManager:RegistMsgs(script, msgs);
end

function LUIBase:UnRegistSelf(script, msgs)
    LUIManager:UnRegistSelf(script, msgs);
end

function LUIBase:Destroy()
    self:UnRegistSelf(self, self.msgIds);
end

function LUIBase:GetGameObject(objName)
    return LUIManager.GetGameObject(objName);
end

function LUIBase:GetUIComponent(objName, uiName)
    return LUIManager.GetUIComponent(objName, uiName);
end

function LUIBase:SendMsg(msg)
    LUIManager.SendMsg(msg);
end

--针对预设
function LUIBase:Instantiate(folderName, resName)
    return GameObject.Instantiate(AssetLoader:LoadObject(folderName, resName));
end

function LUIBase:SetName(name)
    self.transform.name = name;
end

function LUIBase:SetPosition(position)
    self.transform.localPosition = position;
end

function LUIBase:SetActive(active)
    self.gameObject:SetActive(active);
end

function LUIBase:Reset()
    self.transform.localPosition = Vector3.zero;
    self.transform.localEulerAngles = Vector3.zero;
    self.transform.localScale = Vector3.one;
end

function LUIBase:SetParent(parent)
    self.transform:SetParent(parent);
end
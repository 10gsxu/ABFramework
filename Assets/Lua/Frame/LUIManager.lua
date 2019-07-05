LUIManager = LManagerBase:New();
local this = LUIManager;

function LUIManager.Awake()
    print("LUIManager.Awake");

    this.objDict = {};
end

function LUIManager.SendMsg(msg)
    if msg:GetManager() == LManagerID.LUIManager then
        this:ProcessEvent(msg);
    else
        LMsgCenter.SendMsg(msg);
    end
end

function LUIManager.GetParentName(objTran)
    local parentName = "";
    local panelName = "";
    local itemName = nil;
    local objName = objTran.name;
    repeat
        objTran = objTran.parent;
        if string.find(objTran.name, "Item") ~= nil then
            itemName = objTran.name;
            itemName = string.gsub(itemName, "%(Clone%)", "");
        end
        if string.find(objTran.name, "Panel") ~= nil then
            panelName = objTran.name;
            panelName = string.gsub(panelName, "%(Clone%)", "");
        end
    until string.find(objTran.name, "Panel") ~= nil;
    parentName = panelName;
    if itemName ~= nil then
        parentName = parentName.."."..itemName;
    end
    parentName = parentName.."."..objName;
    return parentName;
end

function LUIManager.RegistGameObject(gameObject)
    local parentName = this.GetParentName(gameObject.transform);
    this.objDict[parentName] = gameObject;
end

function LUIManager.GetGameObject(objName)
    if this.objDict[objName] == nil then
        print(objName.." is nil");
    end
    return this.objDict[objName];
end

function LUIManager.GetUIComponent(objName, uiName)
    return this.GetGameObject(objName):GetComponent(uiName);
end
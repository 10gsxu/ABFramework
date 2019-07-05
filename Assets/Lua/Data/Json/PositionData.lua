PositionData = IJsonData:New()
local this = PositionData

function PositionData.Init()
    this:ReadData("PositionData");
end

function PositionData.GetPositionList(level)
    return this.fileData[level]
end

function PositionData.GetTrashPosition()
    return this.fileData["Trash"]
end
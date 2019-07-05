UserLevelData = ICsvData:New()
local this = UserLevelData

function UserLevelData.Init()
    this:InitData("UserLevelData");
end

function UserLevelData.GetProperty(id, title)
    return this:GetCsvProperty(id, title)
end

function UserLevelData.GetDataRow()
    return this:GetCsvDataRow()
end
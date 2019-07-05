AchievementData = ICsvData:New()
local this = AchievementData

function AchievementData.Init()
    this:InitData("AchievementData");
end

function AchievementData.GetProperty(id, title)
    return this:GetCsvProperty(id, title)
end

function AchievementData.GetDataRow()
    return this:GetCsvDataRow()
end
LanguageData = ICsvData:New()
local this = LanguageData

function LanguageData.Init()
    this:InitData("LanguageData");
end

function LanguageData.GetProperty(id, title)
    return this:GetCsvProperty(id, title)
end

function LanguageData.GetDataRow()
    return this:GetCsvDataRow()
end
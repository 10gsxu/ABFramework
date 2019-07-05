CarData = ICsvData:New()
local this = CarData

function CarData.Init()
    this:InitData("CarData");
end

function CarData.GetProperty(id, title)
    return this:GetCsvProperty(id, title)
end

function CarData.GetDataRow()
    return this:GetCsvDataRow()
end
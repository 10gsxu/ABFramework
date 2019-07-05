ICsvData = {
    fileData = {};
};
ICsvData.__index = ICsvData;

function ICsvData:New()
    self = {};
    setmetatable(self, ICsvData);
    self.fileData = {};
    return self;
end

function ICsvData:InitData(fileName)
    textAsset = AssetLoader:LoadTextAsset("Data", fileName);
    if textAsset == nil then
        print(fileName.."不存在")
        return;
    end
    -- 按行划分
    local lineStr = ICsvData.Split(textAsset.text, '\n\r');
    --[[
        第一行是字段，第二行是类型，第三行是描述
    ]]
    local titles = string.split(lineStr[1], ",");
    local types = string.split(lineStr[2], ",");
    self.fileData.Row = #lineStr - 3;
    local fillContent;
    for i = 4, #lineStr, 1 do
        -- 一行中，每一列的内容
        local content = string.split(lineStr[i], ",");
        -- 以标题作为索引，保存每一列的内容，取值的时候这样取：arrs.Id.Title
        self.fileData[content[1]] = {};
        for j = 2, #titles, 1 do
        	if types[j] == "string" then
        		fillContent = content[j]
        	else 
        		fillContent = tonumber(content[j] == "" and "0" or content[j])
        	end
            self.fileData[content[1]][titles[j]] = fillContent
        end
    end
end

--  匹配任何非reps的字符
--  +表示匹配前一字符1次或多次
function ICsvData.Split(str, reps)
    local resultStrList = {}
    string.gsub(str, '[^'..reps..']+', function (w)
        table.insert(resultStrList, w)
    end)
    return resultStrList;
end

function ICsvData:GetCsvProperty(id, title)
    return self.fileData[id][title]
end

function ICsvData:GetCsvDataRow()
    return self.fileData.Row
end
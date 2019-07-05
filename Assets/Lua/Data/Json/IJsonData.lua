IJsonData = {
};
IJsonData.__index = IJsonData;

function IJsonData:New()
    self = {}
    setmetatable(self, IJsonData);
    self.fileName = "";
    self.fileContent = "";
    return self;
end

function IJsonData:ReadData(fileName)
    local textAsset = AssetLoader:LoadTextAsset("Data", fileName);
    self.fileContent = textAsset.text;
    self.fileData = json.decode(self.fileContent)
end

function IJsonData:InitData(fileName)
    self.fileName = fileName
    if FileTools.IsFileExists(self.fileName) then
        --self.fileContent = FileTools.ReadFile(self.fileName);
        local file = io.open(FileTools.RootPath..self.fileName, "rb")
        self.fileContent = file:read("*a")
        file:close()
    else
        local textAsset = AssetLoader:LoadTextAsset("Data", fileName);
        if textAsset == nil then
            print(self.fileName.."不存在")
            return;
        end
        self.fileContent = textAsset.text;
        --FileTools.CreateOrWriteFile(self.fileName, self.fileContent);
        local file = io.open(FileTools.RootPath..self.fileName, "wb")
        file:write(self.fileContent)
        file:close()
    end
    self.fileData = json.decode(self.fileContent)
end

function IJsonData:SaveData()
    self.fileContent = json.encode(self.fileData)
    --FileTools.CreateOrWriteFile(self.fileName, self.fileContent);
    local file = io.open(FileTools.RootPath..self.fileName, "wb")
    file:write(self.fileContent)
    file:close()
end
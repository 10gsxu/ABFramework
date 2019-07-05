ShopItem = LUIBase:New();
ShopItem.__index = ShopItem;

function ShopItem:New()
    local self = {};
    setmetatable(self, ShopItem);

    self.objDict = {};
    self.gameObject = self:Instantiate("UIPanel", "ShopItem");
    self.transform = self.gameObject.transform;

    return self;
end

function ShopItem:Init(parent, index)
    self:SetParent(parent);
    self:Reset(self.transform);

    self.index = index;
    --遍历子对象，将子对象放入容器中
    GetChilds(self.transform, self.objDict);
    local btnBuy = self.objDict["BtnBuy"].gameObject;
    local btnScript = AddComponent(btnBuy, LuaUIBehaviour);
    btnScript:AddButtonListener(ShopPanel.BuyOnClick);
    local numberText = GetComponent(self.objDict["NumberText"], "Text");
    numberText.text = tostring(self.index);
    local nameText = GetComponent(self.objDict["NameText"], "Text");
    local neme = CarData.GetProperty(tostring(self.index), "neme");
    nameText.text = LanguageData.GetProperty(neme, Language);
    local iconName = CarData.GetProperty(tostring(self.index), "icon");
    local carImage = GetComponent(self.objDict["CarImage"], "Image");
    carImage.sprite = AssetLoader:LoadSprite("Car", iconName);

    --EventHandler.on("CoinCount", self.SetName, self);
end

function ShopItem:ProcessEvent(msg)

end
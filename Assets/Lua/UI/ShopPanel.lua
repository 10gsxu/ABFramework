ShopPanel = LUIBase:New();
local this = ShopPanel;

function ShopPanel.Init()
    this.gameObject = this:Instantiate("UIPanel", "ShopPanel");
    this:SetActive(false);
    this.transform = this.gameObject.transform;
    this:SetParent(DialogCanvas);
    this:Reset(this.transform);
        
    local luaScript = this:GetUIComponent("ShopPanel.BtnClose", "LuaUIBehaviour");
    luaScript:AddButtonListener(this.CloseOnClick);

    this.itemContainer = this:GetGameObject("ShopPanel.Content").transform;
    local itemCount = CarData.GetDataRow();
    local rectTran = this.itemContainer:GetComponent("RectTransform");
    rectTran.sizeDelta = Vector2.New(500, itemCount * 130 + 10);
    local ItemLength = 130;
    for i=1, itemCount, 1 do
        local shopItem = GameObjectFactory.Create("ShopItem");
        shopItem:Init(this.itemContainer, i);
        shopItem:SetName("ShopItem"..i);
        shopItem:SetPosition(Vector3.New(0, ItemLength*(1-i) - 70, 0));
    end
end

function ShopPanel.Show()
    this:SetActive(true);
    local scalTime = 0.05
    local m_scale = this.transform:DOScale(Vector3(1.1, 1.1, 1), scalTime)
end

function ShopPanel.Hide()
    this:SetActive(false);
end

function ShopPanel.CloseOnClick(go)
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.HidePanel, "ShopPanel");
    this:SendMsg(msg);
end

function ShopPanel.BuyOnClick(go)
    print(go.transform.parent.name);
    local msg = LPanelMsg:New(LUIEvent.HidePanel, "ShopPanel");
    this:SendMsg(msg);
end
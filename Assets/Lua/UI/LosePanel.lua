LosePanel = LUIBase:New();
local this = LosePanel;

function LosePanel.Init()
    this.gameObject = this:Instantiate("UIPanel", "LosePanel");
    this:SetActive(false);
    this.transform = this.gameObject.transform;
    this:SetParent(UICanvas);
    this:Reset(this.transform);

    --金币
    this.coinText = this:GetUIComponent("MainPanel.CoinText", "Text");
    this.coinText.text = tostring(PlayerData.GetCoinCount());
    EventHandler.on("CoinCount", this.CoinCountChange);
    --宝石
    this.jewelText = this:GetUIComponent("MainPanel.JewelText", "Text");
    this.jewelText.text = tostring(PlayerData.GetJewelCount());
    EventHandler.on("JewelCount", this.JewelCountChange);
end

function MainPanel.CoinCountChange(coinCount)
    this.coinText.text = tostring(coinCount);
end

function MainPanel.JewelCountChange(jewelCount)
    this.jewelText.text = tostring(jewelCount);
end

function LosePanel:ProcessEvent(msg)
    
end

function LosePanel.Show()
    this:SetActive(true);
    local scalTime = 0.05
    local m_scale = this.transform:DOScale(Vector3(1.1, 1.1, 1), scalTime)
end

function LosePanel.Hide()
    this:SetActive(false);
end
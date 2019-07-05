require "UI.SeatPanel"

MainPanel = LUIBase:New();
local this = MainPanel;

function MainPanel.Init()
    this.gameObject = this:Instantiate("UIPanel", "MainPanel");
    this:SetActive(false);
    this.transform = this.gameObject.transform;
    this:SetParent(UICanvas);
    this:Reset(this.transform);

    local levelScript = this:GetUIComponent("MainPanel.BtnLevel", "LuaUIBehaviour");
    levelScript:AddButtonListener(this.LevelOnClick);
    local rankScript = this:GetUIComponent("MainPanel.BtnRank", "LuaUIBehaviour");
    rankScript:AddButtonListener(this.RankOnClick);
    local shopScript = this:GetUIComponent("MainPanel.BtnShop", "LuaUIBehaviour");
    shopScript:AddButtonListener(this.ShopOnClick);
    --金币
    this.coinText = this:GetUIComponent("MainPanel.CoinText", "Text");
    this.coinText.text = tostring(PlayerData.GetCoinCount());
    EventHandler.on("CoinCount", this.CoinCountChange);
    --宝石
    this.jewelText = this:GetUIComponent("MainPanel.JewelText", "Text");
    this.jewelText.text = tostring(PlayerData.GetJewelCount());
    EventHandler.on("JewelCount", this.JewelCountChange);
    --玩家经验
    this.progressImage = this:GetUIComponent("MainPanel.ProgressImage", "Image");
    local maxExp = UserLevelData.GetProperty(tostring(PlayerData.GetPlayerLevel()), "level_up_exp");
    --this.progressImage.fillAmount = PlayerData.GetExpCount() / maxExp;
    EventHandler.on("ExpCount", this.ExpCountChange);
    --玩家等级
    this.levelText = this:GetUIComponent("MainPanel.LevelText", "Text");
    this.levelText.text = tostring(PlayerData.GetPlayerLevel());
    EventHandler.on("PlayerLevel", this.PlayerLevelChange);

    SeatPanel.Awake();
end

function MainPanel.Show()
    this:SetActive(true);
end

function MainPanel.Hide()
    this:SetActive(false);
end

function MainPanel.CoinCountChange(coinCount)
    this.coinText.text = tostring(coinCount);
end

function MainPanel.JewelCountChange(jewelCount)
    this.jewelText.text = tostring(jewelCount);
end

function MainPanel.ExpCountChange(expCount)
    local maxExp = UserLevelData.GetProperty(tostring(PlayerData.GetPlayerLevel()), "level_up_exp");
    if expCount >= maxExp then
        PlayerData.UpgradePlayerLevel();
        PlayerData.SetExpCount(expCount-maxExp);
    else
        this.progressImage.fillAmount = expCount / maxExp;
    end
end

function MainPanel.PlayerLevelChange(level)
    this.levelText.text = tostring(level);
end

function MainPanel.LevelOnClick(go)
    AudioManager.PlaySound(SoundName.Click);
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.HidePanel, "MainPanel");
    this:SendMsg(msg);
    local msg = LPanelMsg:New(LUIEvent.ShowPanel, "GamePanel");
    this:SendMsg(msg);
    --C# Msg
    local msg = StartGameMsg.New(LGameEvent.StartGame, PlayerData.GetGameCarId(), PlayerData.GetGameLevel(), 1);
    this:SendMsg(msg);
end

function MainPanel.RankOnClick(go)
    AudioManager.PlaySound(SoundName.Click);
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.ShowPanel, "RankPanel");
    this:SendMsg(msg);
end

function MainPanel.ShopOnClick(go)
    AudioManager.PlaySound(SoundName.Click);
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.ShowPanel, "ShopPanel");
    this:SendMsg(msg);
end
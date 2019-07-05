RankPanel = LUIBase:New();
local this = RankPanel;

function RankPanel.Init()
    this.gameObject = this:Instantiate("UIPanel", "RankPanel");
    this:SetActive(false);
    this.transform = this.gameObject.transform;
    this:SetParent(DialogCanvas);
    this:Reset(this.transform);

    local luaScript = this:GetUIComponent("RankPanel.BtnClose", "LuaUIBehaviour");
    luaScript:AddButtonListener(this.CloseOnClick);
end

function RankPanel.Show()
    this:SetActive(true);
end

function RankPanel.Hide()
    this:SetActive(false);
end

function RankPanel.CloseOnClick(go)
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.HidePanel, "RankPanel");
    this:SendMsg(msg);
end
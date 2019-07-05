GamePanel = LUIBase:New();
local this = GamePanel;

function GamePanel.Init()
    this.gameObject = this:Instantiate("UIPanel", "GamePanel");
    this:SetActive(false);
    this.transform = this.gameObject.transform;
    this:SetParent(UICanvas);
    this:Reset(this.transform);

    this.percentText = this:GetUIComponent("GamePanel.PercentText", "Text");
    this.percentText.text = "0%";

    this.msgIds[1] = LUIEvent.GameWin;
    this.msgIds[2] = LUIEvent.GameLose;
    this.msgIds[3] = LUIEvent.GameProgress;
    this:RegistSelf(this, this.msgIds);
end

function GamePanel:ProcessEvent(msg)
    if msg.msgId == LUIEvent.GameWin then
    elseif msg.msgId == LUIEvent.GameLose then
    elseif msg.msgId == LUIEvent.GameProgress then
        print(msg.data);
        this.percentText.text = Mathf.Ceil(msg.data*100) .. "%";
    end
end

function GamePanel.Show()
    this:SetActive(true);
    local scalTime = 0.05
    local m_scale = this.transform:DOScale(Vector3(1.1, 1.1, 1), scalTime)
end

function GamePanel.Hide()
    this:SetActive(false);
end
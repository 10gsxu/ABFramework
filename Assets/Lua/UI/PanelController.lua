require "Data.Csv.ICsvData"
require "Data.Csv.AchievementData"
require "Data.Csv.UserLevelData"
require "Data.Csv.LanguageData"
require "Data.Csv.CarData"
require "Data.Json.IJsonData"
require "Data.Json.PlayerData"
require "Data.Json.SettingData"
require "Data.Json.PositionData"
require "UI.MainPanel"
require "UI.ShopPanel"
require "UI.RankPanel"
require "UI.GamePanel"
require "UI.WinPanel"
require "UI.LosePanel"

PanelController = LUIBase:New();
local this = PanelController;

function PanelController.Awake()
    this.msgIds[1] = LUIEvent.ShowPanel;
    this.msgIds[2] = LUIEvent.HidePanel;
    this:RegistSelf(this, this.msgIds);

    --UI面板
    Canvas = GameObject.Find("Canvas").transform;
    UICanvas = Find(Canvas, "UICanvas");
    DialogCanvas = Find(Canvas, "DialogCanvas");

    this.dataDict = {
        --Data
        PlayerData = PlayerData,
        AchievementData = AchievementData,
        UserLevelData = UserLevelData,
        LanguageData = LanguageData,
        CarData = CarData,
        SettingData = SettingData,
        PositionData = PositionData,

        --Audio
        AudioManager = AudioManager
    }
    --Panel
    this.panelDict = {
        MainPanel = MainPanel,
        RankPanel = RankPanel,
        ShopPanel = ShopPanel,
        GamePanel = GamePanel,
        WinPanel = WinPanel,
        LosePanel = LosePanel
    };

    local dataCount = 0;
    for k, v in pairs(this.dataDict) do
        dataCount = dataCount + 1;
    end

    local panelCount = 0;
    for k, v in pairs(this.panelDict) do
        panelCount = panelCount + 1;
    end
    this.totalCount = #BundleList + dataCount + panelCount;
    this.loadCount = 0;

    this.finishCount = 0;
    for i=1, #BundleList, 1 do
        AssetLoader:LoadAssetBundle(BundleList[i], this.LoadFinish);
    end
end

function PanelController.LoadFinish(bundleName)
    this.finishCount = this.finishCount + 1;
    this.loadCount = this.loadCount + 1;
    local msg = StringMsg.New(LUIEvent.Update_Message, this.loadCount.."/"..this.totalCount);
    this:SendMsg(msg);
    if(this.finishCount == #BundleList) then
        print("Bundle加载完成");
        this.Init();
    end
end

function PanelController:ProcessEvent(msg)
    if msg.msgId == LUIEvent.ShowPanel then
        this.ShowPanel(msg.panelName)
    elseif msg.msgId == LUIEvent.HidePanel then
        this.HidePanel(msg.panelName)
    end
end

--初始化
function PanelController.Init()
    for k, v in pairs(this.dataDict) do
        v.Init();
        this.loadCount = this.loadCount + 1;
        local msg = StringMsg.New(LUIEvent.Update_Message, this.loadCount.."/"..this.totalCount);
        this:SendMsg(msg);
    end
    for k, v in pairs(this.panelDict) do
        v.Init();
        this.loadCount = this.loadCount + 1;
        local msg = StringMsg.New(LUIEvent.Update_Message, this.loadCount.."/"..this.totalCount);
        this:SendMsg(msg);
    end
    --C# Msg
    local msg = MsgBase.New(LUIEvent.LoadFinish);
    this:SendMsg(msg);
    this.ShowPanel("MainPanel");
end

function PanelController.ShowPanel(panelName)
    if this.panelDict[panelName] ~= nil then
        this.panelDict[panelName].Show();
    end
end

function PanelController.HidePanel(panelName)
    if this.panelDict[panelName] ~= nil then
        this.panelDict[panelName].Hide();
    end
end
require "Frame.Define"
require "Frame.LMsgBase"
require "Frame.LEventNode"
require "Frame.LManagerBase"
require "Frame.LUIManager"
require "Frame.LMsgCenter"
require "Frame.LUIEvent"
require "Frame.LPanelMsg"
require "Frame.Debug"
require "Frame.LGameEvent"
json = require 'cjson'
Mathf = require "Mathf"
require "Public.Util";
require "Public.GameObjectFactory";
require "Public.AudioManager";
require "Public.SoundName";
require "Public.EventHandler";
require "Frame.LUIBase"
require "UI.ShopItem";
require "UI.SeatItem";
require "UI.CarItem";
require "UI.PanelController";

--主入口函数。从这里开始lua逻辑
function Main()					
	Debug.Log("logic start");
	GameObjectFactory.Init();
	LMsgCenter.Awake();
	PanelController.Awake();
	LUIManager.Awake();
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end
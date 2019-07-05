WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;
Object = UnityEngine.Object;
SceneManager = UnityEngine.SceneManagement.SceneManager;
RectTransformUtility = UnityEngine.RectTransformUtility;

MsgSpan = 3000;
MsgStart = 32768;
LManagerID = {
	--Lua Msg
	LUIManager = 0,
	LGameManager = MsgSpan * 1,
	LuaManager = MsgSpan * 2,
	LNPCManager = MsgSpan * 3,
	LNetManager = MsgSpan * 4,
	LAssetManager = MsgSpan * 5,
	LDataManager = MsgSpan * 6,
	LAudioManager = MsgSpan * 7,
	--C# Msg
	GameManager = MsgStart,
	UIManager = MsgStart + MsgSpan * 1
}

MsgCenter = U3DEventFrame.MsgCenter;
MsgBase = U3DEventFrame.MsgBase;
StringMsg = U3DEventFrame.StringMsg;

AssetLoader = LuaLoadRes.Instance;

BundleList = {
	"audio",
	"data",
	"uipanel"
}

--Language = "English";
Language = "Chinese";
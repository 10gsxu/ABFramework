using System.Collections;
using System.Collections.Generic;

public enum ManagerID {
    LUIManager = 0,
    LGameManger = FrameTools.MsgSpan,

    GameManager = FrameTools.MsgStart + 0,
	UIManager = FrameTools.MsgStart + FrameTools.MsgSpan,
	AudioManager = FrameTools.MsgStart + FrameTools.MsgSpan * 2,
	ThreadManager = FrameTools.MsgStart + FrameTools.MsgSpan * 3,
	CharactorManager = FrameTools.MsgStart + FrameTools.MsgSpan * 4,
	AssetManager = FrameTools.MsgStart + FrameTools.MsgSpan * 5,
	NetManager = FrameTools.MsgStart + FrameTools.MsgSpan * 6,
}

public class FrameTools {

    public const int MsgStart = 32768;//65535/2
	public const int MsgSpan = 3000;

}
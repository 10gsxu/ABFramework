using UnityEngine;

namespace U3DEventFrame {
	public class MsgBase {
		//表示65535个消息，占两个字节，int占4个字节
		public ushort msgId;

		public int GetManager() {
            if(msgId > FrameTools.MsgStart)
            {
                int tmpId = (msgId - FrameTools.MsgStart) / FrameTools.MsgSpan;
                return FrameTools.MsgStart + tmpId * FrameTools.MsgSpan;
            }
			else
            {
                int tmpId = msgId / FrameTools.MsgSpan;
                return tmpId * FrameTools.MsgSpan;
            }
		}

        public MsgBase()
        {
            this.msgId = 0;
        }

        public MsgBase(ushort msgId)
        {
            this.msgId = msgId;
        }
	}
}
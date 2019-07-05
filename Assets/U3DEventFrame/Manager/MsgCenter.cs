using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class MsgCenter : MonoBehaviour {

		public static MsgCenter Instance;

		private Dictionary<ManagerID, ManagerBase> managerDict = new Dictionary<ManagerID, ManagerBase>();

		void Awake() {
			Instance = this;
			managerDict.Add(ManagerID.UIManager, gameObject.AddComponent<UIManager> ());
            managerDict.Add(ManagerID.GameManager, gameObject.AddComponent<GameManager>());
            managerDict.Add(ManagerID.AudioManager, gameObject.AddComponent<AudioManager>());
            gameObject.AddComponent<LuaEventProcess>();
        }

        public void ProcessEvent(MsgBase msg) {
            //msgId < 32768 --> Lua框架
            if (msg.msgId < FrameTools.MsgStart)
            {
                LuaEventProcess.Instance.ProcessEvent(msg);
            }
            else
            {
                ManagerBase baseManager = managerDict[(ManagerID)msg.GetManager()];
                if (baseManager == null)
                {
                    Debug.LogError("Manager不存在");
                    return;
                }
                baseManager.ProcessEvent(msg);
            }
        }

	}
}
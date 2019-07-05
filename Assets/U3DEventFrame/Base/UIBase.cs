using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class UIBase : MonoBase {

		public override void SendMsg(MsgBase msg) {
			UIManager.Instance.SendMsg (msg);
		}

		public override void ProcessEvent (MsgBase tmpMsg)
		{
			//throw new System.NotImplementedException ();
		}

		public void RegistSelf(MonoBase mono, params ushort[] msgs) {
			UIManager.Instance.RegistMsg (mono, msgs);
		}

		public void UnRegistSelf(MonoBase mono, params ushort[] msgs) {
			UIManager.Instance.UnRegistMsg (mono, msgs);
		}

		public ushort[] msgIds;

		void OnDestroy() {
			if (msgIds != null) {
				UnRegistSelf (this, msgIds);
			}
		}

        public GameObject GetGameObject(string objName)
        {
            return UIManager.Instance.GetGameObject(objName);
        }

        public T GetUIComponent<T>(string objName)
        {
            return GetGameObject(objName).GetComponent<T>();
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class MonoBase : MonoBehaviour {
		public virtual void SendMsg(MsgBase msg) {
		}

		public virtual void ProcessEvent(MsgBase tmpMsg) {
		}
	}
}

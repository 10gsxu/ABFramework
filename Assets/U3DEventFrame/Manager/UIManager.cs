using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	
	public class UIManager : ManagerBase {

		public static UIManager Instance;

		private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();

		void Awake() {
			Instance = this;
		}

		void Start () {
			
		}
		
		void Update () {
			
		}

		public override void SendMsg(MsgBase msg) {
			if ((ManagerID)msg.GetManager () == ManagerID.UIManager) {
				//ManagerBase 本模块自己处理
				ProcessEvent (msg);
			} else {
				MsgCenter.Instance.ProcessEvent (msg);
			}
		}

		public GameObject GetGameObject(string name) {
			if (sonMembers.ContainsKey (name)) {
				return sonMembers [name];
			}
			return null;
		}

		public void RegistGameObject(string name, GameObject gameObject) {
			if(!sonMembers.ContainsKey(name)) {
				sonMembers.Add (name, gameObject);
			}
		}

		public void UnRegistGameObject(string name) {
			if(sonMembers.ContainsKey(name)) {
				sonMembers.Remove(name);
			}
		}
	}
}
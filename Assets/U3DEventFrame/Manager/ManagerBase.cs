using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame {
	public class ManagerBase : MonoBase {

		//存储注册消息 key value
		public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

		/// <summary>
		/// Regists the message.
		/// </summary>
		/// <param name="mono">要注册的脚本</param>
		/// <param name="msgs">脚本 可以注册多个msg</param>
		public void RegistMsg(MonoBase mono, params ushort[] msgs) {
			for (int i = 0; i < msgs.Length; ++i) {
				EventNode tmp = new EventNode (mono);
				RegistMsg (msgs [i], tmp);
			}
		}

		public void RegistMsg(ushort id, EventNode node) {
			//数据链路里 没有这个消息id
			if (!eventTree.ContainsKey (id)) {
				eventTree.Add (id, node);
			} else {
				EventNode tmp = eventTree [id];
				//找到最后一个车厢
				while (tmp.next != null) {
					tmp = tmp.next;
				}
				tmp.next = node;
			}
		}

		public void UnRegistMsg(MonoBase mono, params ushort[] msgs)
		{
			for (int i = 0; i < msgs.Length; ++i) {
				UnRegistMsg (msgs [i], mono);
			}
		}

		public void UnRegistMsg(ushort id, MonoBase mono) {
			if (!eventTree.ContainsKey (id)) {
				Debug.LogWarning ("not contain id ==" + id);
				return;
			} else {
				EventNode tmp = eventTree [id];
				//去掉头部 包含两种情况
				if (tmp.data == mono) {
					EventNode header = tmp;
					//后面多个节点和只有一个节点的情况
					if (header.next != null) {
						header.data = tmp.next.data;
						header.next = tmp.next.next;
					} else {
						eventTree.Remove (id);
					}
				} else {
					while (tmp.next != null) {
						EventNode curNode = tmp.next;
						if (curNode.data == mono) {
							tmp.next = curNode.next;
							break;
						} else {
							tmp = curNode;
						}
					}
				}
			}
		}

		public override void SendMsg (MsgBase msg)
		{
		}

		public override void ProcessEvent (MsgBase tmpMsg)
		{
			//throw new System.NotImplementedException ();
			if (!eventTree.ContainsKey (tmpMsg.msgId)) {
				Debug.LogWarning ("no msgId : " + tmpMsg.msgId);
				return;
			}
			EventNode tmp = eventTree [tmpMsg.msgId];
			tmp.data.ProcessEvent (tmpMsg);
			while (tmp.next != null) {
				tmp = tmp.next;
				tmp.data.ProcessEvent (tmpMsg);
			}
		}

	}
}
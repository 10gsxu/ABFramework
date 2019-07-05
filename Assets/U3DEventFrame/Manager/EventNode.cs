namespace U3DEventFrame {
	public class EventNode {
		//当前数据
		public MonoBase data;

		//下一个节点
		public EventNode next;

		public EventNode(MonoBase tmpMono) {
			this.data = tmpMono;
			this.next = null;
		}
	}
}
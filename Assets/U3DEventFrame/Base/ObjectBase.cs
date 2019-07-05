namespace U3DEventFrame
{
    public class ObjectMsg<T> : MsgBase
    {
        public T data = default(T);
        public ObjectMsg(ushort msgId, T data)
        {
            this.msgId = msgId;
            this.data = data;
        }
    }
}
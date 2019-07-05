namespace U3DEventFrame
{
    public class ArrayMsg<T> : MsgBase
    {
        public T[] data = null;
        public ArrayMsg(ushort msgId, T[] data)
        {
            this.msgId = msgId;
            this.data = data;
        }
    }
}
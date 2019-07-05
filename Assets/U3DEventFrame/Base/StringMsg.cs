namespace U3DEventFrame
{
    public class StringMsg : MsgBase
    {
        public string data;
        public StringMsg(ushort msgId, string data)
        {
            this.msgId = msgId;
            this.data = data;
        }
    }
}
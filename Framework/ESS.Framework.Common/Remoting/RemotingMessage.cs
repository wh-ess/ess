namespace ESS.Framework.Common.Remoting
{
    public class RemotingMessage
    {
        public RemotingMessage(int code, long sequence, byte[] body)
        {
            Code = code;
            Sequence = sequence;
            Body = body;
        }

        public long Sequence { get; private set; }
        public int Code { get; private set; }
        public byte[] Body { get; private set; }
    }
}
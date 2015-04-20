namespace ESS.Framework.Common.Remoting
{
    public interface IChannel
    {
        string RemotingAddress { get; }
        void Close();
    }
}
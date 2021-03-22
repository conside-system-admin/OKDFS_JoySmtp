namespace SmtpServer
{
    public interface IServer
    {
        IServerBehaviour Behaviour { get; }
    }
}
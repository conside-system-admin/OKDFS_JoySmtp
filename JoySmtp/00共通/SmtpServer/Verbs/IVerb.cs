using System;
namespace SmtpServer.Verbs
{
    public interface IVerb
    {
        void Process(SmtpServer.IConnection connection, SmtpServer.SmtpCommand command);
    }
}

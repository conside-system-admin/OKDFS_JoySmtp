#region

using SmtpServer.Verbs;

#endregion

namespace SmtpServer
{
    public class QuitVerb : IVerb
    {
        public void Process(IConnection connection, SmtpCommand command)
        {
            connection.WriteResponse(new SmtpResponse(StandardSmtpResponseCode.ClosingTransmissionChannel,
                                                               "See you later aligator"));
            connection.CloseConnection();
            connection.Session.CompletedNormally = true;
        }
    }
}
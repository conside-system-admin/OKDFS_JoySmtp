#region

using SmtpServer.Verbs;

#endregion

namespace SmtpServer
{
    public class HeloVerb : IVerb
    {
        public void Process(IConnection connection, SmtpCommand command)
        {
            if (!string.IsNullOrEmpty(connection.Session.ClientName))
            {
                connection.WriteResponse(new SmtpResponse(StandardSmtpResponseCode.BadSequenceOfCommands,
                                                                   "You already said HELO"));
                return;
            }

            connection.Session.ClientName = command.ArgumentsText;
            connection.WriteResponse(new SmtpResponse(StandardSmtpResponseCode.OK, "Nice to meet you"));
        }
    }
}
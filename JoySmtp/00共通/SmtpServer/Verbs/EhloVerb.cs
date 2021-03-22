#region

using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmtpServer.Verbs;

#endregion

namespace SmtpServer
{
    public class EhloVerb : IVerb
    {
        public void Process(IConnection connection, SmtpCommand command)
        {
            if (!string.IsNullOrEmpty(connection.Session.ClientName))
            {
                connection.WriteResponse(new SmtpResponse(StandardSmtpResponseCode.BadSequenceOfCommands,
                                                                   "You already said HELO"));
                return;
            }

            StringBuilder text = new StringBuilder();
            text.AppendLine("Nice to meet you.");

            foreach (string extnName in connection.ExtensionProcessors.SelectMany(extn => extn.EHLOKeywords))
            {
                text.AppendLine(extnName);
            }

            connection.WriteResponse(new SmtpResponse(StandardSmtpResponseCode.OK, text.ToString().TrimEnd()));
        }
    }
}
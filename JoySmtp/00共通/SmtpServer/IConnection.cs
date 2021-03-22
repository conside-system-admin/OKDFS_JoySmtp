#region

using System;
using System.IO;
using System.Text;
using SmtpServer.Extensions;
using SmtpServer.Verbs;

#endregion

namespace SmtpServer
{
    public interface IConnection
    {
        IServer Server { get; }
        IExtensionProcessor[] ExtensionProcessors { get; }
        VerbMap VerbMap { get; }
        MailVerb MailVerb { get; }
        ISession Session { get; }
        Message CurrentMessage { get; }
        Encoding ReaderEncoding { get; }
        void SetReaderEncoding(Encoding encoding);
        void SetReaderEncodingToDefault();
        void CloseConnection();
        void ApplyStreamFilter(Func<Stream, Stream> filter);
        void WriteLine(string text, params object[] arg);
        void WriteResponse(SmtpResponse response);
        string ReadLine();
        Message NewMessage();
        void CommitMessage();
        void AbortMessage();
    }
}
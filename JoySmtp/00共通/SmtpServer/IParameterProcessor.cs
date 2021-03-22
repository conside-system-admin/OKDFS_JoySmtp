namespace SmtpServer
{
    public interface IParameterProcessor
    {
        void SetParameter(string key, string value);
    }
}
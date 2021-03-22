using System;
using System.IO;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using MimeKit;
//

namespace JoySmtp._00共通.Common
{
    public class SendErrorMail
    {
    public static void SendErrorMail(string[] args)
    {
      //認証情報
      string client_id = "368840236683-7sn1dflrlhemun7mdh3ff4a4h3mpphsq.apps.googleusercontent.com";
      string client_secret = "vh2Z51fU3YPf8dUVcwILP7ER";
      string[] scopes = {GmailService.Scope.GmailSend};
      string app_name = "Google.Apis.Gmail.v1 Sample";
       
      //メール情報
      string mail_from_name = "差出人";
      string mail_from_address = "(差出人アドレス)";
      string mail_to_name = "宛先";
      string mail_to_address = "(宛先アドレス)";
      string mail_subject = "テストメール";
      string mail_body = @"テストメールを送信します。
受信確認できましたら、返信をお願いします。
 
テスト太郎";
       
      //認証
      UserCredential credential;
      string token_folder_path = Path.Combine(Path.GetTempPath(), "Google.Apis.Gmail.Token");
      credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        new ClientSecrets
        {
          ClientId = client_id,
          ClientSecret = client_secret
        },
        scopes,
        "user",
        CancellationToken.None,
        new FileDataStore(token_folder_path)
      ).Result;
       
      var service = new GmailService(new BaseClientService.Initializer()
      {
        ApplicationName = app_name,
        HttpClientInitializer = credential
      });
       
      //メール作成
      var mime_message = new MimeMessage();
      mime_message.From.Add(new MailboxAddress(mail_from_name, mail_from_address));
      mime_message.To.Add(new MailboxAddress(mail_to_name, mail_to_address));
      mime_message.Subject = mail_subject;
      var text_part = new TextPart(MimeKit.Text.TextFormat.Plain);
      text_part.SetText(Encoding.GetEncoding("iso-2022-jp"), mail_body);
      mime_message.Body = text_part;
       
      byte[] bytes = Encoding.UTF8.GetBytes(mime_message.ToString());
      string raw_message = Convert.ToBase64String(bytes)
        .Replace('+', '-')
        .Replace('/', '_')
        .Replace("=", "");
       
      //メール送信
      var result = service.Users.Messages.Send(
        new Message()
        {
          Raw = raw_message
        },
        "me"
      ).Execute();
       
      Console.WriteLine("Message ID: {0}", result.Id);
      Console.ReadKey(true);
    }
  }
}

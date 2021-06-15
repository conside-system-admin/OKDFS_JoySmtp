using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JoySmtp
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Mutex名を決める（必ずアプリケーション固有の文字列に変更すること！）
            string mutexName = "JoySmtpApplication";

            //Mutex名の先頭に「Global\」を付けて、Global Mutexにする
            mutexName = "Global\\" + mutexName;
            //すべてのユーザーにフルコントロールを許可するMutexSecurityを作成する
            System.Security.AccessControl.MutexAccessRule rule =
                new System.Security.AccessControl.MutexAccessRule(
                    new System.Security.Principal.SecurityIdentifier(
                        System.Security.Principal.WellKnownSidType.WorldSid, null),
                        System.Security.AccessControl.MutexRights.FullControl,
                        System.Security.AccessControl.AccessControlType.Allow);
            System.Security.AccessControl.MutexSecurity mutexSecurity =
                new System.Security.AccessControl.MutexSecurity();
            mutexSecurity.AddAccessRule(rule);
            
            //Mutexオブジェクトを作成する
            bool createdNew;
            System.Threading.Mutex mutex =
                new System.Threading.Mutex(true, mutexName, out createdNew);

            //ミューテックスの初期所有権が付与されたか調べる
            if (createdNew == false)
            {
                //されなかった場合は、すでに起動していると判断して終了
                MessageBox.Show("多重起動はできません。");
                mutex.Close();
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Application.Run(new FormStart(args));
                Application.Run(new FormMain(args));

            }
            finally
            {
                //ミューテックスを解放する
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
    }
}

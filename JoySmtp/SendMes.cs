using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
//using System.CodeDom.Compiler;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using anmar.SharpMimeTools;
using Microsoft.Win32;
//using Rnwood.Smtp4dev.MessageInspector;
//using Rnwood.Smtp4dev.Properties;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

using SmtpServer;
using Message = SmtpServer.Message;

namespace JoySmtp
{
    public class SendMes
    {
        //private readonly SessionViewModel _session;
        ///// <summary>
        ///// 受信データ
        ///// </summary>
        //private readonly BindingList<MessageViewModel> _messages = new BindingList<MessageViewModel>();

        ///// <summary>
        ///// 送信データ
        ///// </summary>
        //private readonly BindingList<MessageViewModel> _send = new BindingList<MessageViewModel>();

        //private Server _server;
        //private bool _quitting;

        ////初期表示フラグ
        //private bool _firstShown = true;

        //public SendMes()
        //{
        //    if (_firstShown)
        //    {

        //        Visible = true;
        //        Visible = !Settings.Default.StartInTray;

        //        if (Settings.Default.ListenOnStartup)
        //        {
        //            StartServer();
        //        }
        //    }

        //    _firstShown = false;        
        //}

        ///// <summary>
        ///// 現在の表示メッセージ
        ///// </summary>
        //public MessageViewModel SelectedMessage
        //{
        //    get
        //    {
        //        if (messageGrid.SelectedRows.Count != 1)
        //        {
        //            return null;
        //        }

        //        return
        //            messageGrid.SelectedRows.Cast<DataGridViewRow>().Select(row => (MessageViewModel)row.DataBoundItem)
        //                .Single();
        //    }
        //}

        ///// <summary>
        ///// 受信メッセージ群
        ///// </summary>
        //public MessageViewModel[] SelectedMessages
        //{
        //    get
        //    {
        //        return
        //            messageGrid.SelectedRows.Cast<DataGridViewRow>().Select(row => (MessageViewModel)row.DataBoundItem)
        //                .ToArray();
        //    }
        //}
        ///// <summary>
        ///// 現在の選択セッション
        ///// </summary>
        //public SessionViewModel SelectedSession
        //{
        //    get
        //    {
        //        return this._session;
        //    }
        //}

        /////// <summary>
        /////// セッション群
        /////// </summary>
        ////public SessionViewModel[] SelectedSessions
        ////{
        ////    get
        ////    {
        ////        return
        ////            sessionsGrid.SelectedRows.Cast<DataGridViewRow>().Select(row => (SessionViewModel)row.DataBoundItem)
        ////                .ToArray();
        ////    }
        ////}

        ///// <summary>
        ///// 受信スレッド開始
        ///// </summary>
        //private void StartServer()
        //{
        //    new Thread(ServerWork).Start();

        //}

        ///// <summary>
        ///// 新規セッション受信した際の、スレッド開始処理
        ///// </summary>
        //private void ServerWork()
        //{
        //    try
        //    {
        //        Application.DoEvents();

        //        ServerBehaviour b = new ServerBehaviour();
        //        b.MessageReceived += OnMessageReceived;
        //        b.SessionCompleted += OnSessionCompleted;

        //        _server = new Server(b);





        //        _server.Run();
        //    }
        //    catch (Exception exception)
        //    {
        //        Invoke((MethodInvoker)(() =>
        //        {

        //            StopServer();

        //            statusLabel.Text = "Server failed: " + exception.Message;

        //            trayIcon.ShowBalloonTip(3000, "Server failed", exception.Message,
        //                                    ToolTipIcon.Error);
        //        }));
        //    }
        //}

        ///// <summary>
        ///// セッション終了時の処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnSessionCompleted(object sender, SessionEventArgs e)
        //{
        //    Invoke((MethodInvoker)(() => { _sessions.Add(new SessionViewModel(e.Session)); }));
        //}

        ///// <summary>
        ///// メッセージの受信時処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnMessageReceived(object sender, MessageEventArgs e)
        //{
        //    MessageViewModel message = new MessageViewModel(e.Message);

        //    Invoke((MethodInvoker)(() =>
        //    {
        //        _messages.Add(message);

        //        if (Settings.Default.MaxMessages > 0)
        //        {
        //            while (_messages.Count > Settings.Default.MaxMessages)
        //            {
        //                _messages.RemoveAt(0);
        //            }
        //        }

        //        if (Settings.Default.AutoViewNewMessages ||
        //            Settings.Default.AutoInspectNewMessages)
        //        {
        //            if (Settings.Default.AutoViewNewMessages)
        //            {
        //                ViewMessage(message);
        //            }

        //            if (Settings.Default.AutoInspectNewMessages)
        //            {
        //                InspectMessage(message);
        //            }
        //        }
        //        else if (!Visible && Settings.Default.BalloonNotifications)
        //        {
        //            string body =
        //                string.Format(
        //                    "From: {0}\nTo: {1}\nSubject: {2}\n<Click here to view more details>",
        //                    message.From,
        //                    message.To,
        //                    message.Subject);

        //            trayIcon.ShowBalloonTip(3000, "Message Recieved", body, ToolTipIcon.Info);
        //        }

        //        if (Visible && Settings.Default.BringToFrontOnNewMessage)
        //        {
        //            BringToFront();
        //            Activate();
        //        }
        //    }));
        //}
    }

}

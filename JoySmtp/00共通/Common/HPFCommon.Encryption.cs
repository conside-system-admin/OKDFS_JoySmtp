using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using JoySmtp.CLogOut;

namespace JoySmtp.JoyCommon
{
    public partial class Common
    {
        /// <summary>
        ///         ''' 文字列をDESの形式に暗号化します
        ///         ''' </summary>
        ///         ''' <param name="strText">文字列（暗号化の対象）</param>
        ///         ''' <param name="strPwdKey">暗号化用のキー</param>
        ///         ''' <returns>
        ///         ''' 暗号の文字列
        ///         ''' </returns>
        ///         ''' <remarks></remarks>
        public string EncryptDECString(string strText, string strPwdKey)
        {
            try
            {
                // 文字列をバイト配列に変換
                byte[] BytText = Encoding.UTF8.GetBytes(strText);
                // DESCryptoServiceProviderオブジェクトの作成
                DESCryptoServiceProvider ObjDes = new DESCryptoServiceProvider();
                // パスワードをバイト配列に変換
                byte[] BytPassword = Encoding.UTF8.GetBytes(strPwdKey);

                // 共有キーと初期化ベクタを設定
                ObjDes.Key = ResizeBytesArray(BytPassword, ObjDes.Key.Length);
                ObjDes.IV = ResizeBytesArray(BytPassword, ObjDes.IV.Length);

                // 暗号化されたデータを書き出すためのMemoryStream
                MemoryStream ObjOut = new MemoryStream();
                // DES暗号化オブジェクトの作成
                ICryptoTransform ObjDesdecrypt = ObjDes.CreateEncryptor();
                // 書き込むためのCryptoStreamの作成
                CryptoStream ObjCryptStreem = new CryptoStream(ObjOut, ObjDesdecrypt, CryptoStreamMode.Write);

                // 書き込む
                ObjCryptStreem.Write(BytText, 0, BytText.Length);
                ObjCryptStreem.FlushFinalBlock();

                // 暗号化されたデータを取得
                byte[] BytOut = ObjOut.ToArray();

                // オブジェクトを閉じる
                ObjCryptStreem.Close();
                ObjOut.Close();

                // Base64で文字列に変更して結果を返す
                return Convert.ToBase64String(BytOut);
            }
            catch (Exception ex)
            {
                // エラー処理
                LogOut.ErrorOut(ex.Message, "HPFCommon.Encryption", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' DESの形式で暗合された文字列を復号化する
        ///         ''' </summary>
        ///         ''' <param name="strText">文字列（復号化の対象）</param>
        ///         ''' <param name="strPwdKey">暗号化用のキー</param>
        ///         ''' <returns>
        ///         ''' 暗号化の文字列
        ///         ''' </returns>
        ///         ''' <remarks></remarks>
        public string DecryptDESString(string strText, string strPwdKey)
        {
            try
            {
                // DESCryptoServiceProviderオブジェクトの作成
                DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
                // パスワードをバイト配列にする
                byte[] Password = Encoding.UTF8.GetBytes(strPwdKey);
                // 共有キーと初期化ベクタを設定
                dsp.Key = ResizeBytesArray(Password, dsp.Key.Length);
                dsp.IV = ResizeBytesArray(Password, dsp.IV.Length);
                // Base64で文字列をバイト配列に戻す
                byte[] byte64 = Convert.FromBase64String(strText);
                // 暗号化されたデータを読み込むためのMemoryStream
                MemoryStream memory = new MemoryStream(byte64);
                // DES復号化オブジェクトの作成
                ICryptoTransform transform = dsp.CreateDecryptor();
                // 読み込むためのCryptoStreamの作成
                CryptoStream stream = new CryptoStream(memory, transform, System.Security.Cryptography.CryptoStreamMode.Read);
                // 復号化されたデータを取得するためのStreamReader
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                // 復号化されたデータを取得
                string result = reader.ReadToEnd();
                // オブジェクトを閉じる
                reader.Close();
                stream.Close();
                memory.Close();

                // 結果を返す
                return result;
            }
            catch (Exception ex)
            {
                // エラー処理
                LogOut.ErrorOut(ex.Message, "HPFCommon.Encryption", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' バイト配列のサイズを変更する
        ///         ''' </summary>
        ///         ''' <param name="bytes">バイト配列 Byte</param>
        ///         ''' <param name="newSize">変更後のサイズ Integer</param>
        ///         ''' <returns>変更後のバイト配列 Byte()</returns>
        ///         ''' <remarks></remarks>
        private byte[] ResizeBytesArray(byte[] bytes, int newSize)
        {
            try
            {
                byte[] newBytes;
                newBytes = new byte[newSize - 1 + 1];
                if (bytes.Length <= newSize)
                {
                    for (int i = 0; i <= bytes.Length - 1; i++)
                        newBytes[i] = bytes[i];
                }
                else
                {
                    int pos = 0;
                    for (int i = 0; i <= bytes.Length - 1; i++)
                    {
                        newBytes[pos] = System.Convert.ToByte(newBytes[pos] ^ bytes[i]);
                        pos += 1;
                        if (pos >= newBytes.Length)
                            pos = 0;
                    }
                }
                return newBytes;
            }
            catch (Exception ex)
            {
                // エラー処理
                LogOut.ErrorOut(ex.Message, "HPFCommon.Encryption", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }
}

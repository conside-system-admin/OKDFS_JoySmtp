using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;


namespace JoySmtp
{
    public class DnsConnect
    {
        /// <summary>
        /// 指定したDNSサーバーで、指定したレコードの検索を行う。
        /// </summary>
        /// <param name="hostOrAddress">検索するホスト名またはIPアドレス。</param>
        /// <param name="qType">レコード種別。
        /// A=1 NS=2 CNAME=5 PTR=12 MX=15 AAAA=28 のみに対応。</param>
        /// <param name="dnsServer">DNSサーバーのIPアドレスまたはホスト名。</param>
        /// <param name="dnsPort">DNSサーバーのポート番号。</param>
        /// <returns>回答結果。ホスト名かIPアドレス。</returns>
        public static string[] LookupDns(string hostOrAddress, byte qType,
            string dnsServer, int dnsPort)
        {
            //送信するデータを作成する
            byte[] req = CreateRequestMessage(hostOrAddress, qType);
            Console.WriteLine(BitConverter.ToString(req));

            byte[] res = null;
            using (UdpClient udpc = new UdpClient(dnsServer, dnsPort))
            {
                //UDPでリクエストメッセージを送信する
                udpc.Send(req, req.Length);

                //レスポンスを受信する
                IPEndPoint ep = null;
                res = udpc.Receive(ref ep);
            }
            Console.WriteLine(BitConverter.ToString(res));

            //受信したデータから回答を取り出す
            return GetAnswersFromResponse(res, req.Length);
        }

        //リクエストメッセージを作成する
        private static byte[] CreateRequestMessage(string qName, byte qType)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                //Header section
                //ID（ここでは、「00 00」に固定）
                ms.WriteByte(0);
                ms.WriteByte(0);
                //QR、Opcode、AA、TC、RD（ここでは、RDのみ1としている）
                ms.WriteByte(1);
                //RA、Z、RCODE
                ms.WriteByte(0);
                //QDCOUNT（質問数）（質問数は、1つ）
                ms.WriteByte(0);
                ms.WriteByte(1);
                //ANCOUNT（回答数）
                ms.WriteByte(0);
                ms.WriteByte(0);
                //NSCOUNT
                ms.WriteByte(0);
                ms.WriteByte(0);
                //ADCOUNT
                ms.WriteByte(0);
                ms.WriteByte(0);

                //Question section
                //QNAME を作成
                if (qType == 12)
                {
                    //PTRの時は、arpaアドレスに変換
                    qName = ConvertToArpaAddress(qName);
                }
                //ホスト名を"."で分割
                string[] buf1 = qName.Split('.');
                foreach (string s in buf1)
                {
                    //文字列をバイト配列に変換
                    byte[] bs1 = Encoding.ASCII.GetBytes(s);
                    //長さと文字列データを書き込む
                    ms.WriteByte((byte)bs1.Length);
                    ms.Write(bs1, 0, bs1.Length);
                }
                ms.WriteByte(0);
                //QTYPE
                ms.WriteByte(0);
                ms.WriteByte(qType);
                //QCLASS（ここでは、IN(Internet)の1に固定）
                ms.WriteByte(0);
                ms.WriteByte(1);

                return ms.ToArray();
            }
        }

        //IPアドレスを、逆引きのためのarpaアドレスに変換する
        private static string ConvertToArpaAddress(string ipString)
        {
            StringBuilder sb = new StringBuilder();
            //IPAddressオブジェクトを作成
            IPAddress ip = IPAddress.Parse(ipString);
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                //IPv4の時
                //バイト配列に変換
                byte[] bs = ip.GetAddressBytes();
                //逆順に並び替えて、in-addr.arpaを付ける
                for (int i = bs.Length - 1; 0 <= i; i--)
                {
                    sb.Append(bs[i]).Append('.');
                }
                sb.Append("in-addr.arpa");
            }
            else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                //IPv6の時
                byte[] bs = ip.GetAddressBytes();
                //逆順に並び替えて、ip6.arpaを付ける
                for (int i = bs.Length - 1; 0 <= i; i--)
                {
                    sb.AppendFormat("{0:x}", bs[i] & 0xF).Append('.').
                        AppendFormat("{0:x}", (bs[i] >> 4) & 0xF).Append('.');
                }
                sb.Append("ip6.arpa");
            }

            return sb.ToString();

            //または、IPv4の場合だけならば、以下のようにもできる
            //return System.Text.RegularExpressions.Regex.Replace(
            //    ipString, @"(\d+)\.(\d+)\.(\d+)\.(\d+)",
            //    "$4.$3.$2.$1.in-addr.arpa");
        }

        //レスポンスメッセージから回答を取得する
        private static string[] GetAnswersFromResponse(byte[] data, int startPos)
        {
            //データの読み込み位置
            int pos = 0;

            //RCODE (0=No error condition)
            int resCode = data[3] & 0xF;
            Console.WriteLine("Response Code:{0}", resCode);
            //ANCOUNT（回答数）
            pos = 6;
            int anCount = ConvertToShort(data, ref pos);
            Console.WriteLine("Answers Count:{0}", anCount);
            //NSCOUNT
            int nsCount = ConvertToShort(data, ref pos);
            //ARCOUNT
            int arCount = ConvertToShort(data, ref pos);
            //Questionを飛ばす
            pos = startPos;

            List<string> list = new List<string>();
            //回答数だけ繰り返す
            for (int i = 0; i < anCount; i++)
            {
                //ドメイン名を取り出す
                string domainName = GetDomainName(data, ref pos);
                Console.WriteLine("Domain:{0}", domainName);
                //TYPE
                short ppType = ConvertToShort(data, ref pos);
                //CLASS
                short ppClass = ConvertToShort(data, ref pos);
                //TTL
                int ttl = ConvertToInt(data, ref pos);
                Console.WriteLine("TTL:{0}", ttl);
                //RDLENGTH
                short rdLength = ConvertToShort(data, ref pos);

                string str = string.Empty;
                switch (ppType)
                {
                    case 1:
                    //A
                    case 28:
                        //AAAA
                        //IPアドレスを抜き出す
                        str = GetIPAddress(data, rdLength, ref pos);
                        break;
                    case 2:
                    //NS
                    case 5:
                    //CNAME
                    case 12:
                        //PTR
                        //ホスト名を抜き出す
                        str = GetDomainName(data, ref pos);
                        break;
                    case 15:
                        //MX
                        //Preference（優先度）
                        short preference = ConvertToShort(data, ref pos);
                        Console.WriteLine("Preference:{0}", preference);
                        //ホスト名を抜き出す
                        str = GetDomainName(data, ref pos);
                        break;
                    default:
                        str = string.Format("(対応していないType({0})の回答)",
                            ppType);
                        pos += rdLength;
                        break;
                }
                list.Add(str);
                Console.WriteLine(str);
            }
            //Answerのみを解析し、AuthorityとAdditionalは無視

            return list.ToArray();
        }

        //データの指定位置からDomain名を取得する
        private static string GetDomainName(byte[] data, ref int pos)
        {
            StringBuilder sb = new StringBuilder();

            while (pos < data.Length)
            {
                //labelの長さを取得
                int len = data[pos++];
                //長さが0の時、終了
                if (len == 0) { break; }
                if ((len & 0xC0) == 0xC0)
                {
                    //Message compressionの時
                    //新たな位置を取得
                    int newpos = ConvertToShort((byte)(len & 0x3F), data[pos++]);
                    //新たな位置から取得したDomainを追加
                    if (sb.Length > 0) { sb.Append('.'); }
                    sb.Append(GetDomainName(data, ref newpos));
                    break;
                }

                //byteを文字列に変換して、labelを取得し、追加する
                if (sb.Length > 0) { sb.Append('.'); }
                sb.Append(Encoding.ASCII.GetString(data, pos, len));
                pos += len;
            }

            return sb.ToString();
        }

        //データの指定位置からIPアドレスを取得する
        private static string GetIPAddress(byte[] data, int len, ref int pos)
        {
            byte[] bs = new byte[len];
            Array.Copy(data, pos, bs, 0, len);
            pos += len;
            IPAddress ip = new IPAddress(bs);
            return ip.ToString();

            //または、IPv4の時は、
            //return string.Format("{0}.{1}.{2}.{3}",
            //    data[pos++], data[pos++], data[pos++], data[pos++]);
        }


        //2つのバイトからInt16を作成
        private static short ConvertToShort(byte b1, byte b2)
        {
            int i = 0;
            return ConvertToShort(new byte[] { b1, b2 }, ref i);
        }
        private static short ConvertToShort(byte[] data, ref int pos)
        {
            short r = IPAddress.NetworkToHostOrder(
                BitConverter.ToInt16(data, pos));
            pos += 2;
            return r;

            //または、
            //return (short)(data[post++] << 8 | data[post++]);
        }

        //4つのバイトからInt32を作成
        private static int ConvertToInt(byte[] data, ref int pos)
        {
            int r = IPAddress.NetworkToHostOrder(
                BitConverter.ToInt32(data, pos));
            pos += 4;
            return r;
        }
    }
}

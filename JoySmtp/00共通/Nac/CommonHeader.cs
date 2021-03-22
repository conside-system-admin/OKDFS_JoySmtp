using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoySmtp.Nac
{
    /// <summary>
    /// 出力共通項目
    /// </summary>
    public class CommonHeader
    {
        /// <summary>
        /// 輸入申告番号
        /// </summary>
        public string YunyuShinkokuNo { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetCode()
        {
            return null;
        }

    }

}

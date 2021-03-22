using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoySmtp.Nac
{
    abstract class NaccsMessage
    {
        public List<INaccsCollumn> ListData;

        public NaccsMessage()
        {
            SetInitData();
        }
        abstract public void SetInitData();

        public byte[] GetByteData()
        {
            if (this.ListData != null)
            {
                byte[] senddata = null;
                foreach (INaccsCollumn col in this.ListData)
                {

                }
                return senddata;
            }
            else
            {
                return null;
            }
        }
    }
}

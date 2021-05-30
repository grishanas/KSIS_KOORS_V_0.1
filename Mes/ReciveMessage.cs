using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1.Mes
{
    class ReciveMessage : Imessage
    {
        public byte Type { get { return Type; } set { } }
        public byte[] Content;

        public ReciveMessage(byte[] content)
        {
            Type = 1;
            Content = content;
        }

        public byte[] GetBytes()
        {
            var data = new byte[Content.Length + 1];
            data[0] = Type;
            Content.CopyTo(data, 1);
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1
{
    class Message
    {
		public byte Type;
		public string Content;

		public Message(byte type, string content)
		{
			Type = type;
			Content = content;
		}

		public byte[] GetBytes()
		{
			var paylod = Encoding.UTF8.GetBytes(Content);
			var data = new byte[paylod.Length + 1];
			data[0] = Type;
			paylod.CopyTo(data, 1);
			return data;
		}
	}
}

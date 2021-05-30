using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace KSIS_KOORS_V_0._1
{
    class Server
    {
		User myUser;
		public List<ConnectedUser> users = new List<ConnectedUser>();
		public UdpClient nameReceiver = new UdpClient();
		public TcpListener listener;
		private bool ServerAccept = true;

		private int udpPort = 5555;
		private int tcpPort = 5556;


		public Factory.IFactory[] FactoryMes =
		{
			new Factory.ReciveNameFactory(),
			new Factory.ReciveMessageFactory(),
			new Factory.RecivePointFactory()
		};

		public int FindPeople(ConnectedUser user)
        {
			return users.IndexOf(user);

		}

		Thread listenThread;
		Thread nameReciveThread;


		public Server(User user)
		{
			this.myUser = user;
			listenThread = new Thread(new ThreadStart(Listen));
			listenThread.Start();
			nameReciveThread = new Thread(new ThreadStart(ReceiveNewUser));
			nameReciveThread.Start();
		}

		protected bool IsMyIp(IPAddress ip)
		{
			string host = Dns.GetHostName();
			IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;
			foreach (var address in addresses)
			{
				if (address.Equals(ip))
				{
					return true;
				}
			}
			return false;

		}

		void ReceiveNewUser()
		{
			nameReceiver = new UdpClient(udpPort);
			nameReceiver.EnableBroadcast = true;
			IPEndPoint remoteIp = null;
			try
			{
				while (ServerAccept)
				{
					byte[] data = nameReceiver.Receive(ref remoteIp);
					string str = Encoding.UTF8.GetString(data);
					if (!IsMyIp(remoteIp.Address))
					{
						var Name = new Mes.ReciveName("01"+str);
						users.Add(new ConnectedUser(remoteIp.Address, Name.ToString(), myUser));
						//yUser.form1.Message.Enqueue(Name);
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
				//Console.WriteLine(ex.Message);
			}
			finally
			{
				nameReceiver.Close();
			}
		}

		void Listen()
		{
			try
			{
				listener = new TcpListener(IPAddress.Any, tcpPort);
				listener.Start();
				while (ServerAccept)
				{
					var tcpClient = listener.AcceptTcpClient();
					users.Add(new ConnectedUser(tcpClient, myUser));
				}
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.Message);
			}
			finally
			{
				if (listener != null)
				{
					listener.Stop();
				}
			}
		}

		public void Disconnect(ConnectedUser connectedUser)
		{
			if (users.Contains(connectedUser))
			{
				//this.myUser.form1.richTextBox1.Text += connectedUser.Name + "покинул чат";
				users.Remove(connectedUser);

			}
		}

		public void SendTo(int index,byte[] data)
        {
			var user = users[index];
			user.WriteToStream(data);
        }

		public void SendAllUsers(byte[] data)
		{
			foreach (var user in users)
			{
				user.WriteToStream(data);
			}
		}

		public void ServDispose()
        {
			listener.Stop();
			nameReceiver.Close();
        }
	}
}

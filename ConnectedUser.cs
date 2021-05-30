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
    class ConnectedUser
    {
		private string Name;

		private IPAddress IP;

        private int TCPport;
		private TcpClient tcpClient;
		private Thread userThread;
		private User user;
		private NetworkStream stream = null;

		private bool CanStartNewGame = true;

		private int tcpPort = 5556;
		public ConnectedUser(IPAddress ip, string name, User User)
		{
			IP = ip;
			user = User;
			Name = name;
			var iPEndPoint = new IPEndPoint(IP, tcpPort);
			tcpClient = new TcpClient();
			tcpClient.Connect(iPEndPoint);
			userThread = new Thread(new ThreadStart(Listen));
			userThread.Start();
		}

		public ConnectedUser(TcpClient tcpClient, User user)
		{
			this.tcpClient = tcpClient;
			this.user = user;
			IP = IPAddress.Parse(tcpClient.Client.RemoteEndPoint.ToString().Split(':')[0]);
			Name = "undef";
			userThread = new Thread(new ThreadStart(Listen));
			userThread.Start();
		}

		Factory.IFactory[] FactoryMes =
		{
			new Factory.ReciveNameFactory(),
			new Factory.ReciveMessageFactory(),
			new Factory.RecivePointFactory()
		};

		private void StartUiThread(User user, int code)
        {
			user.PlayGround = new PlayGround(user, code);
			user.PlayGround.ShowDialog();
        }

		// 0- имя,1 байт: 0- удалить имя,1- добавить,2-противник сдался
		// 1 - Запрос на подключение. content == 1 значит подключение разрешено, 0 - нет
		// 2 - Пересылка сообщений между пользователями. 0 байт- код,1-цвет,2-5- координата по X,6-9- координаты по Y
		private void Listen()
        {
			try
			{
				stream = tcpClient.GetStream();
				var PrepaereName = Encoding.UTF8.GetBytes(user.Name);
				var data = new byte[2 + PrepaereName.Length];
				data[0] = 0;
				data[1] = 1;
				PrepaereName.CopyTo(data, 2);
				stream.Write(data, 0, data.Length);
				while (true)
				{
					var packet = GetMessage();
					switch(packet[0])
                    {
						case 0:
                            {
             
								if (packet[1] == 0)
                                {
									if(user.ChooseUser == this)
                                    {
										user.ChooseUser = null;
										user.PlayGround.EnemyLeav();
										user.CanStartNewGame = true;
									}
									throw new Exception("");
                                }
								if(packet[1]==1)
                                {
									user.form1.Message.Enqueue(FactoryMes[packet[0]].Create(packet));
								}
								if(packet[1]==2)
                                {
									user.ChooseUser = null;
									user.PlayGround.EnemyLeav();
									user.CanStartNewGame = true;
									//throw new Exception("");
								}

								break;
                            }
						case 1:
                            {
								byte[] answer;

								if (user.CanStartNewGame)
								{
									user.CanStartNewGame = false;
									var choos = MessageBox.Show(
										"Игрок"+this.Name.ToString() + ",приглашает в игру. Принять приглашение?",
										"Сообщение", MessageBoxButtons.YesNo,
										MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

									if (choos == DialogResult.Yes)
									{
										answer = (new Mes.Answer(1,1)).GetBytes();
										user.ChooseUser = this;
										new Thread(() => StartUiThread(user, 0)).Start();
										
										
										//new Thread(() =>user.PlayGround.ShowDialog()).Start();

									}
									else
									{
										answer = (new Mes.Answer(1, 0)).GetBytes();
										user.CanStartNewGame = true;
									}
									WriteToStream(answer);

								}
								else if (user.ChooseUser == this)
								{
									switch (packet[1])
									{
										case 0:
                                            {
												MessageBox.Show("Пользователь отклонил вашу заявку");
												user.CanStartNewGame = true;
												user.ChooseUser = null;		
												break;
                                            }
										case 1:
											{
												MessageBox.Show("Пользователь принял вашу заявку");
												new Thread(() => StartUiThread(user, 1)).Start();
												break;
											}
									}
								}
                                else
                                {
									answer = (new Mes.Answer(1, 0)).GetBytes();
									WriteToStream(answer);
								}
							

								break;
                            }
						case 2:
                            {
								var request = FactoryMes[packet[0]].Create(packet);
								user.PlayGround.Message.Enqueue(request);


								break;
                            }
                    }
				}
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.Message);
			}
			finally
			{
				if(user.PlayGround!=null)
                {
					user.PlayGround.BeginInvoke(new Action(()=> user.PlayGround.Close()));
                }
				if (stream != null)
				{
					stream.Close();
				}
				var index=user.server.FindPeople(this);
				user.form1.Message.Enqueue(new Mes.ReciveName("00"+Convert.ToString(index)));

				tcpClient.Close();
				user.server.Disconnect(this);
			}
		}

		private byte[] GetMessage()
		{
			byte[] data = new byte[64];
			var packet = new List<byte>();
			StringBuilder builder = new StringBuilder();
			int bytes = 0;
			do
			{
				bytes = stream.Read(data, 0, data.Length);
				packet.AddRange(data.Take(bytes));
			}
			while (stream.DataAvailable);

			return packet.ToArray();
		}

		public void WriteToStream(byte[] data)
		{
			if (stream != null)
			{
				stream.Write(data, 0, data.Length);
			}
		}

	}
}

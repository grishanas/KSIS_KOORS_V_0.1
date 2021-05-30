using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace KSIS_KOORS_V_0._1
{
    class User
    {
        public string Name;
        public IPAddress IP;
        public PlayGround PlayGround;

        protected int UDPSender = 5555;
        protected UdpClient UDPClient;
        public Form1 form1;
        public Server server;

        protected ConcurrentQueue<Mes.Imessage> message;

        public bool CanStartNewGame;

        public ConnectedUser ChooseUser;

        public User(string name,Form1 form)
        {
            Name = name;
            form1 = form;
            CanStartNewGame = true;
            server = new Server(this);
            IP = GetCurrrentHostIp();
            SendName();
        }

        public IPAddress GetCurrrentHostIp()
        {
            string host = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;
            foreach (var address in addresses)
            {
                if (address.GetAddressBytes().Length == 4)
                {
                    return address;
                }
            }
            return null;
        }

        private void SendName()
        {
            UDPClient = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, UDPSender);
            UDPClient.EnableBroadcast = true;
            var data = new Mes.ReciveName("01" + Name);
            var udpMassege = data.GetBytes();
            UDPClient.Send(udpMassege, udpMassege.Length, endPoint);
            UDPClient.Close();
        }

        public void SendRequest(int index, byte code,string message)
        {
            try
            {
                byte[] data = (new Message(code, message).GetBytes());
                server.users[index].WriteToStream(data);
            }
            catch
            {

            }
        }


        public void SendMessage(string message)
        {
            try
            {
                byte[] data = (new Message(1, message)).GetBytes();
                server.SendAllUsers(data);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

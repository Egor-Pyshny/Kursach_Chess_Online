using System;
using System.Net;
using System.Net.Sockets;


namespace Kursach2.Client
{
    public class ClientMethods
    {
        private static volatile TcpClient user = null;
        private static bool ConnectionToServer = false;
        private static bool ConnectionToUser = false;
        public static int user_ID { get; set; }
        public static int my_id{ get; set; }
        public TcpClient User() {
            if (user == null) {
                user = new TcpClient();
            }
            return user;
        }

        public async void ConnectToServer()
        {
            TcpClient client = User();
            if (!client.Connected)
            {
                try
                {
                    client.Connect(IPAddress.Parse("192.168.0.104"), 8001);
                    ConnectionToServer = true;
                    client.GetStream().Write(new byte[] { 0 }, 0, 1);
                    byte[] temp = new byte[1];
                    client.GetStream().Read(temp, 0, 1);
                    my_id = temp[0];
                }
                catch (Exception) { }
            }
        }

        public byte user_ip() { return (byte)user_ID; }

        public void set_user_ip(byte b) { user_ID = b; }
        public byte my_ip() { return (byte)my_id; }
        public async void ConnectToUser(byte ind) {
            //код 2 запрос на подключение
            user_ID = ind;
            TcpClient client = User();
            NetworkStream stream = client.GetStream();
            if (ConnectionToServer && client.Connected) {
                stream.Write(new byte[] { 2, ind },0,2);
                byte[] response = new byte[4096];
                int count;
                try
                {
                    count = stream.Read(response, 0, response.Length);
                }
                catch (System.ObjectDisposedException) { return; }
                //Task.Delay(1000);
                if (count != 0 && response[0] == 254)
                {
                    ConnectionToUser = true;
                }
            }
        }

        public async void AcceptUser()
        {
            
        }
    }
}

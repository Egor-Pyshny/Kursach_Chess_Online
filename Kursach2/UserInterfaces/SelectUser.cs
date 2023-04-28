using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Net.Sockets;

namespace Kursach2.UserInterfaces
{
    public class SelectUser
    {
        private RenderWindow selection;
        private static TcpClient user;
        private Text text = new Text();
        private int count;
        private static int selected=-1;
        private string[] ipAdresses;
        private static byte[] data;
        private static byte __user_id__;
        private static int color=0;
        public int ShowWindow(RenderWindow parentWindow)
        {
            int res=-1;
            parentWindow.SetVisible(false);
            selection = new RenderWindow(new VideoMode(500, 600), "Selection", Styles.Titlebar | Styles.Close);
            text.Font = new Font("tmr.ttf");
            text.CharacterSize = 24;
            text.Color = Color.Black;
            selection.Closed += _Close;
            selection.MouseButtonPressed += pressed;
            LIST();
            ipAdresses = ParseIP(data);
            while (selection.IsOpen)
            {
                selection.Clear(Color.White);


                ReadStream();
                DrawUsersID(ipAdresses);
                AddRefreshButton();
                selection.DispatchEvents();
                if (selected != -1) {
                    res = selected;
                    Program.player = Convert.ToBoolean(color);
                    selected = -1;
                    selection.Close();
                }
                selection.Display();
            }
            parentWindow.SetVisible(true);
            return res;
        }

        private void LIST() {
            NetworkStream stream = user.GetStream();
            stream.Write(new byte[2] { 1,Program.client.my_ip() }, 0, 2);
            byte[] temp = new byte[1];
            stream.Read(temp, 0, 1);
            data = new byte[temp[0]];
            if(temp[0]!=0)
                count = stream.Read(data, 0, data.Length);
        }

        private static async void ReadStream()
        {
            TcpClient tcpClient = user;
            int count=0;
            try
            {
                if (tcpClient.GetStream().DataAvailable)
                {
                    data = new byte[2];
                    count =  await tcpClient.GetStream().ReadAsync(data, 0, data.Length);
                }
                if (data.Length!=0 && data[0]==255) {
                    selected = data[1];
                    color = 0;
                    byte[] msg = new byte[2] { 255, Program.client.user_ip() };
                    tcpClient.GetStream().Write(msg, 0, msg.Length);
                    
                }

            }
            catch (System.ObjectDisposedException) { return; }
        }

        private void AddRefreshButton()
        {
            RectangleShape btn = new RectangleShape(new Vector2f(40, 40));
            btn.Position = new Vector2f(410, 45);
            btn.FillColor = new Color(255,255,255,255);
            btn.OutlineColor = new Color(105, 105, 105, 105);
            btn.OutlineThickness = 2;
            Texture refresh = new Texture("refresh.png");
            Sprite spr = new Sprite(refresh);
            spr.Position = new Vector2f(410, 45);
            selection.Draw(btn);     
            selection.Draw(spr);  
        }

        private void pressed(object sender, MouseButtonEventArgs e)
        {
            int x = 50, y = 100;
            int amount = ipAdresses.Length;
            for (int i = 0; i < amount; i++)
            {

                if (new IntRect(x, y, 400, 60).Contains(e.X, e.Y))
                {
                    selected = i;
                    color = 1;
                    break;
                }
                y += 65;
            }
            if (new IntRect(410, 45, 40, 40).Contains(e.X, e.Y)) {
                LIST();
                ipAdresses = ParseIP(data);
            }
        }

        private void DrawUsersID(string[] ipAdresses)
        {
            int amount = ipAdresses.Length;
            float xrect = 50, yrect = 100;
            RectangleShape listitem = new RectangleShape(new Vector2f(400,60));
            text.DisplayedString = "Your id: " + user.Client.LocalEndPoint.ToString();
            text.Position = new Vector2f(50, 50);
            selection.Draw(text);
            for (int i = 0; i < amount; i++)
            {
                listitem.Position = new Vector2f(xrect, yrect);
                listitem.FillColor = new Color(105, 105, 105, 100);
                selection.Draw(listitem);
                text.DisplayedString = ipAdresses[i];
                float textW = text.GetGlobalBounds().Width;
                text.Position = new Vector2f(50+(400 - textW) / 2, yrect+(60-32)/2);
                selection.Draw(text);
                yrect += 65;
            }
        }

        private string[] ParseIP(byte[] data)
        {
            List<string> res = new List<string>(0);
            int amount = data.Length / 21;
            for (int i = 0; i < amount; i++) {
                byte[] temp = new byte[21];
                Array.Copy(data, i * 21, temp, 0, 21);
                res.Add(Encoding.UTF8.GetString(temp));
            }
            return res.ToArray(); 
        }

        public SelectUser(TcpClient tcp) {
            user = tcp;
        }

        private void _Close(object sender, EventArgs e)
        {
            selection.Close();
        }
    }
}

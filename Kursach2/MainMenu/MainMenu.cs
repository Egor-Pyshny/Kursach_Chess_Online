using System;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Kursach2.UserInterfaces;

namespace Kursach2.MainMenu
{
    public class Menu
    {
        private static RenderWindow menu;
        private static bool res = true;
        private static Text text = new Text();
        static void Main(string[] args)
        {
            menu = new RenderWindow(new VideoMode(1250, 840), "Chess", Styles.Titlebar | Styles.Close);
            Texture bgtex = new Texture(Environment.CurrentDirectory + "\\bg.png");
            Sprite bgspr = new Sprite(bgtex);
            text.Font = new Font("tmr.ttf");
            text.CharacterSize = 24;
            text.Color = Color.White;
            menu.Closed += _Close;
            menu.MouseButtonPressed += pressed;
            while (menu.IsOpen) {
                menu.Clear(Color.White);
                menu.Draw(bgspr);
                DrawBtns();
                menu.DispatchEvents();
                menu.Display();
            }
        }

        private static void pressed(object sender, MouseButtonEventArgs e)
        {
            int x = 575, y = 190, step = 120,btnnum=-1;
            for (int i = 0; i < 4; i++)
            {
                if (new IntRect(x, y, 200, 100).Contains(e.X, e.Y)) {
                    btnnum = i;
                    break;
                }
                y += 120;
            }
            switch (btnnum) {
                case 0: 
                    {
                        Program.online = false;
                        res = true;
                        Program.Main(menu);
                        break;
                    }
                case 1:
                    {
                        SelectUser select = new SelectUser(Program.client.User());
                        Program.client.ConnectToServer();
                        int temp = select.ShowWindow(menu);
                        if (temp != -1)
                        {
                            Program.usernum = temp;
                            res = true;
                            Program.online = true;
                            temp = -1;
                            Program.Main(menu);
                            
                        }
                        break;
                    }
                case 2:
                    {
                        Rules rules = new Rules();
                        rules.ShowWindow(menu);
                        break;
                    }
                case 3:
                    {
                        res = false;
                        menu.Close();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void DrawBtns()
        {
            RectangleShape btn = new RectangleShape(new Vector2f(200, 100));
            btn.FillColor = new Color(0, 0, 0, 190);
            float x = 575, y = 190,step=120;
            Dictionary<int, string> dict = new Dictionary<int, string>() {
                {0,"Start Game"},
                {1,"Online Game"},
                {2,"Rules"},
                {3,"Exit"},
            };
            for (int i = 0; i < 4; i++) {
                btn.Position = new Vector2f(x,y);
                text.DisplayedString = dict[i];
                FloatRect temp = text.GetGlobalBounds();
                text.Position = new Vector2f(x + (200 - temp.Width) / 2, y + 31);
                y += step;
                menu.Draw(btn);
                menu.Draw(text);
            }
        }

        private static void _Close(object sender, EventArgs e)
        {
            res = false;
            menu.Close();
        }
    }
}

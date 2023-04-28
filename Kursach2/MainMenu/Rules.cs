using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.IO;

namespace Kursach2.MainMenu
{
    
    public class Rules
    {
        private RenderWindow rules;
        private View view = new View(new FloatRect(0, 0, 525, 500));
        private Text text = new Text();
        private float topview = 0;
        public void ShowWindow(RenderWindow parentwindow) {
            parentwindow.SetVisible(false);
            rules = new RenderWindow(new VideoMode(525, 500), "Chess",Styles.Titlebar | Styles.Close);
            rules.SetView(view);
            rules.Closed += _Close;
            rules.MouseWheelMoved += _Wheel;
            text.Font = new Font("tmr.ttf");
            text.CharacterSize = 24;
            text.Color = Color.Black;
            LoadRules();
            while (rules.IsOpen)
            {
                rules.Clear(Color.White);
                rules.DispatchEvents();
                rules.Draw(text);
                rules.Display();
            }
            parentwindow.SetVisible(true);
        }

        private void LoadRules()
        {
            string rul = File.ReadAllText(Environment.CurrentDirectory + "\\rules.txt");
            rul = rul.Replace("\r", "");
            text.DisplayedString = rul;
        }

        private void _Wheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta > 0) {
                if (topview >= 50)
                {
                    view.Move(new Vector2f(0, -50));
                    topview -= 50;
                }
            }
            else
            {
                if (topview <= 1050)
                {
                    topview += 50;
                    view.Move(new Vector2f(0, +50));
                }
            }
            rules.SetView(view);
        }

        private void _Close(object sender, EventArgs e)
        {
            rules.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Kursach2.DialogWindows
{
    public class CheckWindow
    {
        private static RenderWindow window;
        private static int color;
        private static Text txt;
        public static void DrawCheckWindow(int check_color,RenderWindow renderWindow)
        {
            //check_color - цвет которому шах
            txt = txt = new Text();
            color = check_color;
            txt.CharacterSize = 32;
            txt.Font = new Font("tmr.ttf");
            txt.Color = Color.Black;
            window = new RenderWindow(new VideoMode(400, 100), "", Styles.Titlebar);
            window.Position = new Vector2i(renderWindow.Position.X + 400, renderWindow.Position.Y + 450);
            window.Closed += close;
            window.RequestFocus();
            window.LostFocus += close;
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.White);
                WriteText();
                window.Display();
            }
        }

        private static void WriteText()
        {
            if (color == 1)
            {
                txt.DisplayedString = "Check to white";
            }
            else
            {
                txt.DisplayedString = "Check to black";
            }
            txt.Position = new Vector2f((window.Size.X - txt.GetLocalBounds().Width) / 2, 0);
            window.Draw(txt);
            txt.CharacterSize = 18;
            txt.DisplayedString = "To close this window, click anywhere outside of it.";
            txt.Position = new Vector2f((window.Size.X - txt.GetLocalBounds().Width) / 2, 45);
            window.Draw(txt);
        }

        private static void close(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}

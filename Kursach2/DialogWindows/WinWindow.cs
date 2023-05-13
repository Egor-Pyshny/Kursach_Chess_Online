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
    public class WinWindow
    {
        private static RenderWindow window;
        private static int color;
        private static Text txt;
        public static void DrawWinWindow(int win_color) {
            color = win_color;
            txt.CharacterSize=35;
            txt.Color=Color.Black;
            window = new RenderWindow(new VideoMode(400, 250), "", Styles.Titlebar);
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
            if (color==1)
            {
                txt.DisplayedString="White win";
            }
            else
            {
               txt.DisplayedString="Black win";
            }
            txt.Position = new Vector2f((window.Size.X - txt.GetLocalBounds().Width) / 2, 0);
            window.Draw(txt);
            txt.CharacterSize =18;
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

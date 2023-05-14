using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Kursach2.Figures;
using Kursach2.Figures.Factory;
using Kursach2.Client;
using Kursach2.MainMenu;
using System.Net.Sockets;
using Kursach2.UserInterfaces;
using Kursach2.DialogWindows;

namespace Kursach2
{
    class Program
    {
        private static int[] field = new int[64] {
            7,8,9,10,11,9,8,7,
            12,12,12,12,12,12,12,12,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            6,6,6,6,6,6,6,6,
            1,2,3,4,5,3,2,1,
        };
        private static Figure f = new Figure();
        private static Figure SelectedFigure = null;
        private static Figure PrevSelectedFigure;
        public static bool player=true;
        public static bool current_player=true;
        private static bool checktoWhite = false;
        private static bool checktoBlack = false;
        private static bool showcheckwindow_b;
        private static bool showcheckwindow_w;
        private static bool whitewin = false;
        private static bool blackwin = false;
        private static bool whiterotateLF = true;
        private static bool whiterotateRF = true;
        private static bool blackrotateLF = true;
        private static bool blackrotateRF = true;
        private static bool switching = false;
        public static bool online = false;
        public static int usernum;
        private static int switchcolor = -1;
        private static int switchind = -1;
        private static Figure switchedfig = null;
        public static ClientMethods client = new ClientMethods();
        //white = true 
        //black=false

        //Добавить обмен пешки и добавить сбитые фигуры в пакет сети
        private static int[] whitefig = new int[6] { 1, 2, 3, 4, 5, 6 };
        private static int[] blackfig = new int[6] { 7, 8, 9, 10, 11, 12 };
        public static RenderWindow window;
        private static int curentbrik = -1;
        private static int pressedbrik = -1;
        private static byte[] data = new byte[0];
        public static void Main(RenderWindow parentWindow)
        {
            parentWindow.SetVisible(false);           
            Text text = new Text();
            text.Font = new Font("tmr.ttf");
            window = new RenderWindow(new VideoMode(1200, 1000), "Chess", Styles.Titlebar | Styles.Close);
            InitVars();
            FillFigArr();
            if (online)
            {
                if (player)
                {
                    client.ConnectToUser((byte)usernum);
                }
                else
                {
                    client.set_user_ip((byte)usernum);
                }
            }
            window.Closed += _Close;
            window.MouseButtonPressed += pressed;
            window.MouseMoved += moved;
            while (window.IsOpen) {
                CheckFlags();
                window.DispatchEvents();
                window.Clear(Color.White);
                DrawPlayers(text);
                //TcpClient dasdas = client.User();
                if (online)
                {
                    ReadStream();
                    int count = data.Length;
                    if (count != 0 && data[0] == 253)
                    {
                        byte id = client.user_ip();
                        client.User().GetStream().Write(new byte[] { 253, id }, 0, 2);
                        /*client.User().Close();*/
                        window.Close();
                        Array.Clear(data, 0, data.Length);
                    }
                    else if (count == 4096 && data[0] == 3)
                    {
                        ParseResponse(data, count);
                        FillFigArr();
                        Array.Clear(data, 0, data.Length);
                    }
                }
                /*if (tcpClient.GetStream().CanRead)
                    data = new byte[4096];
                    count = tcpClient.GetStream().Read(data, 0, data.Length);*/                
                DrawField(window);
                DrawFigure(window);
                DrawDownedFigure(window);
                if (!switching)
                {
                    if (CheckMove())
                    {
                        if (online && !switching)
                        {
                            SendRequest(CreateRequest());
                            current_player = !current_player;
                        }
                    }
                }
                CheckSwitch();
                window.Display();
            }
            parentWindow.SetVisible(true);
        }

        private static void CheckFlags()
        {
            if (whitewin)
            {
                WinWindow.DrawWinWindow(1, window);
                window.Close();
            }
            else if (blackwin)
            {
                WinWindow.DrawWinWindow(0, window);
                window.Close();
            }
            else if (checktoWhite && showcheckwindow_w)
            {
                CheckWindow.DrawCheckWindow(1,window);
                showcheckwindow_w = false;
            }
            else if (checktoBlack && showcheckwindow_b) 
            {
                CheckWindow.DrawCheckWindow(0, window);
                showcheckwindow_b = false;
            }
        }

        private static void InitVars()
        {
            field = new int[64] {
                7,8,9,10,11,9,8,7,
                12,12,12,12,12,12,12,12,
                0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,
                6,6,6,6,6,6,6,6,
                1,2,3,4,5,3,2,1,
            };
            f = new Figure();
            SelectedFigure = null;
            if (!online)
            {
                player = true;
                online = false;
            }
            current_player = true;
            checktoWhite = false;
            checktoBlack = false;
            whitewin = false; 
            blackwin = false;
            whiterotateLF = true;
            whiterotateRF = true;
            blackrotateLF = true;
            blackrotateRF = true;
            switching = false;          
            switchcolor = -1;
            switchind = -1;
            switchedfig = null;
            client = new ClientMethods();
            //white = true 
            //black=false

            //Добавить обмен пешки и добавить сбитые фигуры в пакет сети
            whitefig = new int[6] { 1, 2, 3, 4, 5, 6 };
            blackfig = new int[6] { 7, 8, 9, 10, 11, 12 };
            curentbrik = -1;
            pressedbrik = -1;
            data = new byte[0];
        }

        private static void CheckSwitch()
        {
            if (switching && switchind!=-1 && switchedfig != null) {
                MainFactory factory = new MainFactory();
                int ftype = 0;
                if (switchcolor == 1) ftype = whitefig[switchind]; else ftype = blackfig[switchind] - 6;
                Figure fig = factory.Produce(ftype, switchcolor, switchedfig.x, switchedfig.y);
                f.FigureList[switchedfig.ind] = fig;
                switchind = -1;
                switching = false;
                switchcolor = -1;
                switchedfig = null;
            }
        }

        private static void DrawPlayers(Text text)
        {
            text.CharacterSize = 24;
            text.Position = new Vector2f(870, 500);
            StringBuilder str = new StringBuilder("Current move: ");
            text.Color = Color.Black;
            if (current_player)
                text.DisplayedString = str.Append("White").ToString();
            else
                text.DisplayedString = str.Append("Black").ToString();
            window.Draw(text);
            if (online) {
                string temp;
                if (player) { temp = "White"; } else { temp = "Black"; }
                text.DisplayedString = "You are playing for" + temp;
                text.Position = new Vector2f(870, 525);
                window.Draw(text);
            }
        }

        private static async void ReadStream()
        {
            TcpClient tcpClient = client.User();
            data = new byte[0];
            try
            {
                if (tcpClient.GetStream().DataAvailable)
                {
                    data = new byte[4096];
                    await tcpClient.GetStream().ReadAsync(data, 0, data.Length);
                }
            }
            catch (System.ObjectDisposedException) { return; }
        }


        private static void SendRequest(byte[] data)
        {
            TcpClient tcpClient = client.User();
            tcpClient.GetStream().Write(data, 0, data.Length);
        }

        private static byte[] CreateRequest()
        {
            byte[] request = new byte[4096];
            List<byte> list = new List<byte>();
            list.Add(3);
            list.Add((byte)ClientMethods.user_ID);
            list.Add(Convert.ToByte(!player));
            list.Add(Convert.ToByte(checktoWhite));
            list.Add(Convert.ToByte(checktoBlack));
            list.Add(Convert.ToByte(whitewin));
            list.Add(Convert.ToByte(blackwin));
            list.Add(Convert.ToByte(whiterotateLF));
            list.Add(Convert.ToByte(whiterotateRF));
            list.Add(Convert.ToByte(blackrotateLF));
            list.Add(Convert.ToByte(blackrotateRF));
            for (int i = 0; i < 64; i++) {
                list.Add(Convert.ToByte(f[i].ftype));
            }
            int count = f.whitedeletedfigure.Count,count2 = f.blackdeletedfigure.Count;
            list.Add((byte)count);
            list.Add((byte)count2);
            for (int i = 0; i < count; i++) {
                list.Add((byte)f.whitedeletedfigure[i].ftype);
            }
            for (int i = 0; i < count2; i++)
            {
                list.Add((byte)(f.blackdeletedfigure[i].ftype-6));
            }
            byte[] arr = list.ToArray();
            Array.Copy(arr, 0, request, 0, arr.Length);
            return request;
        }

        private static void ParseResponse(byte[] data, int count)
        {
            current_player = Convert.ToBoolean(data[2]);
            checktoWhite = Convert.ToBoolean(data[3]);
            checktoBlack = Convert.ToBoolean(data[4]);
            whitewin = Convert.ToBoolean(data[5]);
            blackwin = Convert.ToBoolean(data[6]);
            whiterotateLF = Convert.ToBoolean(data[7]);
            whiterotateRF = Convert.ToBoolean(data[8]);
            blackrotateLF = Convert.ToBoolean(data[9]);
            blackrotateRF = Convert.ToBoolean(data[10]);
            if (checktoBlack) { showcheckwindow_b = true; }
            if (checktoWhite) { showcheckwindow_w = true; }
            int j = 10;
            for (int i = 0; i < 64; i++)
            {
                field[i] = (int)data[i + 11];
                j++;
            }
            j++;
            byte tmp = data[j];
            j++;
            byte tmp2 = data[j];
            j++;
            MainFactory factory = new MainFactory();
            List<Figure> Wdeleted = new List<Figure>();
            List<Figure> Bdeleted = new List<Figure>();
            for (int i = 0; i < tmp; i++)
            {
                Wdeleted.Add(factory.Produce((int)data[j],1,-200,-200));
                j++;
            }
            f.whitedeletedfigure = Wdeleted;
            for (int i = 0; i < tmp2; i++)
            {
                Bdeleted.Add(factory.Produce((int)data[j], 0, -200, -200));
                j++;
            }
            f.blackdeletedfigure = Bdeleted;
        }

        private static void moved(object sender, MouseMoveEventArgs e)
        {
            if (player == current_player)
            {
                if (new IntRect(50, 100, 800, 800).Contains(e.X, e.Y))
                {
                    curentbrik = ((e.Y - 100) / 100) * 8 + (e.X - 50) / 100;
                }
            }
            else
            {
                curentbrik = -1;
            }
        }

        private static bool CheckMove() {
            bool res=false;
            if (PrevSelectedFigure != null && SelectedFigure != null)
            {
                if (player && player==current_player && whitefig.Contains(PrevSelectedFigure.ftype))
                {
                    if ((PrevSelectedFigure.ftype == 1 && SelectedFigure.ftype == 4) || (PrevSelectedFigure.ftype == 4 && SelectedFigure.ftype == 1))
                    {
                        List<Figure> list = new List<Figure>(f.FigureList);
                        if ((whiterotateLF && list[57] is NullFigure && list[58] is NullFigure) || (whiterotateRF && list[60] is NullFigure && list[61] is NullFigure && list[62] is NullFigure))
                        {
                            int ind1 = PrevSelectedFigure.ind, ind2 = SelectedFigure.ind;
                            int ind = SelectedFigure.ind, x = SelectedFigure.x, y = SelectedFigure.y;
                            SelectedFigure.ind = PrevSelectedFigure.ind;
                            SelectedFigure.x = PrevSelectedFigure.x;
                            SelectedFigure.y = PrevSelectedFigure.y;
                            PrevSelectedFigure.ind = ind;
                            PrevSelectedFigure.x = x;
                            PrevSelectedFigure.y = y;
                            list[ind1] = SelectedFigure;
                            list[ind2] = PrevSelectedFigure;
                            if (UnderAttack(player, list))
                            {
                                list[PrevSelectedFigure.ind] = SelectedFigure;
                                list[SelectedFigure.ind] = PrevSelectedFigure;
                            }
                            else
                            {
                                f.FigureList = list;
                                res = true;
                                if (PrevSelectedFigure is WhiteKing)
                                {
                                    whiterotateLF = false;
                                    whiterotateRF = false;
                                }
                                else if (PrevSelectedFigure is WhiteRook)
                                {
                                    if (PrevSelectedFigure.ind == 56) whiterotateLF = false;
                                    if (PrevSelectedFigure.ind == 63) whiterotateRF = false;
                                }
                                blackrotateLF = false;
                                blackrotateRF = false;
                                PrevSelectedFigure = null;
                                SelectedFigure = null;
                            }
                        }
                    }
                    else if (!whitefig.Contains(SelectedFigure.ftype))
                    {
                        int ind1 = PrevSelectedFigure.ind, ind2 = SelectedFigure.ind;
                        List<Figure> list = new List<Figure>(f.FigureList);
                        Figure deletedFigure = new NullFigure(0, 0);
                        if (PrevSelectedFigure.CanMove(ind1, ind2, list))
                        {
                            deletedFigure = PrevSelectedFigure.Move(ind1, ind2, ref list);
                            res = true;
                        }
                        if (UnderAttack(player, list))
                        {
                            PrevSelectedFigure.Undo(ind2, ind1, ref list, deletedFigure);
                            res = false;
                        }
                        else
                        {
                            f.FigureList = list;
                            if ((PrevSelectedFigure is WhitePawn) && (PrevSelectedFigure.ind >= 0 && PrevSelectedFigure.ind <= 7)) {
                                switchedfig = PrevSelectedFigure;
                                switching = true;
                                switchcolor = 1;
                            }
                            if(!(deletedFigure is NullFigure))
                            {
                                f.blackdeletedfigure.Add(deletedFigure);
                            }                          
                            if (UnderAttack(!player, list)) {
                                checktoBlack = true;
                                showcheckwindow_b = true;
                                Console.WriteLine("CheckBlack");
                                whitewin = IsWin(player, list);
                                if (whitewin) {
                                    Console.WriteLine("WhiteWin");
                                } else
                                {
                                    Console.WriteLine("WhiteDontWin");
                                }
                            }
                            if (!online && res) { 
                                player = !player;
                                current_player = !current_player;
                            }
                            //
                        }

                        PrevSelectedFigure = null;
                        SelectedFigure = null;
                    }
                }
                else if (!player && player == current_player &&  blackfig.Contains(PrevSelectedFigure.ftype)) {
                    if ((PrevSelectedFigure.ftype == 7 && SelectedFigure.ftype == 10) || (PrevSelectedFigure.ftype == 10 && SelectedFigure.ftype == 7))
                    {
                        List<Figure> list = new List<Figure>(f.FigureList);
                        if ((blackrotateLF && list[1] is NullFigure && list[2] is NullFigure) || (blackrotateRF && list[4] is NullFigure && list[5] is NullFigure && list[6] is NullFigure)) {
                            int ind1 = PrevSelectedFigure.ind, ind2 = SelectedFigure.ind;
                            int ind=SelectedFigure.ind,x=SelectedFigure.x,y=SelectedFigure.y;
                            SelectedFigure.ind = PrevSelectedFigure.ind;
                            SelectedFigure.x = PrevSelectedFigure.x;
                            SelectedFigure.y = PrevSelectedFigure.y;
                            PrevSelectedFigure.ind = ind;
                            PrevSelectedFigure.x = x;
                            PrevSelectedFigure.y = y;
                            list[ind1] = SelectedFigure;
                            list[ind2] = PrevSelectedFigure;
                            if (UnderAttack(player, list))
                            {
                                list[PrevSelectedFigure.ind] = SelectedFigure;
                                list[SelectedFigure.ind] = PrevSelectedFigure;
                            }
                            else {
                                f.FigureList = list;
                                res = true;
                                if (PrevSelectedFigure is WhiteKing)
                                {
                                    blackrotateLF = false;
                                    blackrotateRF = false;
                                }
                                else if (PrevSelectedFigure is WhiteRook)
                                {
                                    if (PrevSelectedFigure.ind == 56) blackrotateLF = false;
                                    if (PrevSelectedFigure.ind == 63) blackrotateRF = false;
                                }
                                blackrotateLF = false;
                                blackrotateRF = false;
                                PrevSelectedFigure = null;
                                SelectedFigure = null;
                            }
                        }
                    }
                    else if (!blackfig.Contains(SelectedFigure.ftype))
                    {
                        int ind1 = PrevSelectedFigure.ind, ind2 = SelectedFigure.ind;
                        List<Figure> list = new List<Figure>(f.FigureList);
                        Figure deletedFigure = new NullFigure(0,0);
                        if (PrevSelectedFigure.CanMove(ind1, ind2, list))
                        {
                            deletedFigure = PrevSelectedFigure.Move(ind1, ind2, ref list);
                            res = true;
                        }
                        if (UnderAttack(player, list))
                        {
                            PrevSelectedFigure.Undo(ind2, ind1, ref list, deletedFigure);
                            res = false;
                        }
                        else
                        {
                            f.FigureList = list;
                            if ((PrevSelectedFigure is BlackPawn) && (PrevSelectedFigure.ind >= 56 && PrevSelectedFigure.ind <= 63))
                            {
                                switchedfig = PrevSelectedFigure;
                                switching = true;
                                switchcolor = 0;
                            }
                            if (PrevSelectedFigure is BlackKing)
                            {
                                blackrotateLF = false;
                                blackrotateRF = false;
                            }
                            else if (PrevSelectedFigure is BlackKing) {
                                if (PrevSelectedFigure.ind == 0) blackrotateLF = false;
                                if (PrevSelectedFigure.ind == 7) blackrotateRF = false;
                            }
                            if (!(deletedFigure is NullFigure))
                            {
                                f.whitedeletedfigure.Add(deletedFigure);
                            }
                            if (UnderAttack(!player, list))
                            {
                                checktoWhite = true;
                                showcheckwindow_w = true;
                                Console.WriteLine("CheckWhite");
                                blackwin = IsWin(player, list);
                                if (blackwin)
                                {
                                    Console.WriteLine("BlackWin");
                                }
                                else
                                {
                                    Console.WriteLine("BlackDontWin");
                                }
                            }
                            if (!online && res)
                            {
                                player = !player;
                                current_player = !current_player;
                            }
                        }

                        PrevSelectedFigure = null;
                        SelectedFigure = null;
                    }
                }
            }
            return res;
        }

        private static bool UnderAttack(bool color, List<Figure> list) {
            //color = true проверка атаки на белых
            int[] arr = new int[5];
            if (!color) { arr = whitefig; } else { arr = blackfig; }
            int kingpos=-1;
            foreach (Figure item in list)
            {
                if ((item is WhiteKing && color) || (item is BlackKing && !color)) {
                    kingpos = item.ind;
                    break;
                }
            }
            foreach (Figure item in list)
            {
                if (arr.Contains(item.ftype) && !(item is WhiteKing) && !(item is BlackKing))
                {
                    if (item.CanMove(item.ind, kingpos, list)) return true;
                }
            }
            return false;
        }

        public static void DrawDownedFigure(RenderWindow w) {
            int x = 875, y = 100,step=40,i=0;
            Sprite spr;
            foreach (Figure item in f.whitedeletedfigure) {
                spr = item.getsmallspr();
                if (spr != null)
                {
                    spr.Position = new Vector2f(x, y);
                    i++;
                    if (i % 5 == 0)
                    {
                        x = 875;
                        y += step;
                    }
                    else x += step;
                    w.Draw(spr);
                }
            }
            x = 875; y = 780; step = 40; i = 0;
            foreach (Figure item in f.blackdeletedfigure)
            {
                spr = item.getsmallspr();
                if (spr != null)
                {
                    spr.Position = new Vector2f(x, y);
                    i++;
                    if (i % 5 == 0)
                    {
                        x = 875;
                        y += step;
                    }
                    else x += step;
                    w.Draw(spr);
                }
            }
        }

        public static bool IsWin(bool color, List<Figure> list)
        {
            //проверяет на победу того цвета который передан в color
            int[] arr = new int[5];
            int[] defarr = new int[5];
            int kingtype=0;
            int[] shifts = new int[] { 1, -1, 8, -8, 7, -7, 9, -9 };
            if (color) {
                defarr = blackfig;
                arr = whitefig;
                kingtype = 4;
            } else {
                defarr = whitefig;
                arr = blackfig;
                kingtype = 10;
            }
            int kingpos = -1;
            foreach (Figure item in list)
            {
                if ((item is WhiteKing && !color) || (item is BlackKing && color))
                {
                    kingpos = item.ind;
                    break;
                }
            }
            for (int i = 0; i < shifts.Length; i++)
            {
                int newInd = kingpos + shifts[i];
                bool canmove = true;
                if (newInd >= 0 && newInd <= 63)
                {
                    foreach (Figure item in list)
                    {
                        if (arr.Contains(item.ftype) && item.ftype != kingtype && item.CanMove(item.ind, newInd, list))
                        {
                            canmove = false;
                            break;
                        }
                        else if (item.ind == newInd && !(item is NullFigure))
                        {
                            canmove = false;
                            break;
                        }
                    }
                    if (canmove)
                        return false;
                }
            }
            Figure attacker = new Figure();
            int count = 0;
            foreach (Figure item in list)
            {
                if (arr.Contains(item.ftype) && item.CanMove(item.ind, kingpos, list)) {
                    count++;
                    if (count > 1)
                    {
                        return true;
                    }
                    else {
                        attacker = item;
                    }
                }
            }
            int[] path = attacker.GetPath(attacker.ind,kingpos);
            for (int i = 0; i < path.Length; i++) {
                int checkind = path[i];
                bool res = true;
                foreach (Figure item in list)
                {
                    if (defarr.Contains(item.ftype) && item.ftype != kingtype && item.CanMove(item.ind, checkind, list))
                    {
                        res = false;
                        break;
                    }
                }
                if (!res)
                    return res;
            }
            return false;
        }

        private static void DrawFigure(RenderWindow w) {
            List<Figure> list = f.FigureList;
            Sprite spr;
            foreach (Figure item in list) {
                spr = item.getspr();
                if (spr != null)
                {                    
                    spr.Position = new Vector2f(item.x, item.y);
                    w.Draw(spr);
                }
            }
            Figure temp;
            MainFactory factory = new MainFactory();
            if (switching && switchcolor == 1)
            {
                int x = 350, y = 30;
                for (int i = 0; i <= 4; i++) {
                    if (i != 3)
                    {
                        temp = factory.Produce(whitefig[i], 1, 0, 0);
                        spr = temp.getsmallspr();
                        spr.Position = new Vector2f(x, y);
                        w.Draw(spr);
                        x += 40;
                    }
                }
            }
            else if (switching && switchcolor == 0) 
            {
                int x = 350, y = 930;
                for (int i = 0; i <= 4; i++)
                {
                    if (i != 3)
                    {
                        temp = factory.Produce(blackfig[i] - 6, 0, 0, 0);
                        spr = temp.getsmallspr();
                        spr.Position = new Vector2f(x, y);
                        w.Draw(spr);
                        x += 40;
                    }
                }
            }
        }

        private static void FillFigArr() {
            int figtype=0, color=0,j=0,i1=0,x=60,y=110,step=100;
            MainFactory factory = new MainFactory();
            List<Figure> list = new List<Figure>();
            for (int i = 1; i <= 64; i++) {
                if (whitefig.Contains(field[i-1]))
                {
                    figtype = field[i - 1];
                    color = 1;
                }
                else if (blackfig.Contains(field[i - 1]))
                {
                    figtype = field[i - 1] - 6;
                    color = -1;
                }
                else{
                    figtype = 0;
                }
                list.Add(factory.Produce(figtype, color, x, y));
                if (i % 8 == 0)
                {
                    y += step;
                    x = 60;
                    j++;
                    i1 = 0;
                }
                else
                {
                    x += step;
                    i1++;
                }
            }
            f.FigureList = list;
        }


        //опробовать добавить подсветку возможных ходов фигур
        private static void pressed(object sender, MouseButtonEventArgs e)
        {
            if (player == current_player)
            {
                if (new IntRect(50, 100, 800, 800).Contains(e.X, e.Y))
                {
                    pressedbrik = ((e.Y - 100) / 100) * 8 + (e.X - 50) / 100;
                    if (player)
                    {
                        if (whitefig.Contains(f[pressedbrik].ftype))
                        {
                            PrevSelectedFigure = SelectedFigure;
                            SelectedFigure = f[pressedbrik];
                        }
                        else
                        {
                            PrevSelectedFigure = SelectedFigure;
                            SelectedFigure = f[pressedbrik];
                            pressedbrik = -1;
                        }
                    }
                    else
                    {
                        if (blackfig.Contains(f[pressedbrik].ftype))
                        {
                            PrevSelectedFigure = SelectedFigure;
                            SelectedFigure = f[pressedbrik];
                        }
                        else
                        {
                            PrevSelectedFigure = SelectedFigure;
                            SelectedFigure = f[pressedbrik];
                            pressedbrik = -1;
                        }
                    }
                }
            }
            else
            {
                pressedbrik = -1;
                SelectedFigure = null;
                PrevSelectedFigure = null;
            }
            if (switching) {
                int x = 350,y = 0;
                if (switchcolor == 1) y = 30; else y = 930;
                if (new IntRect(x, y, 200, 40).Contains(e.X, e.Y))
                {
                    switchind = (int)((e.X - 350) / 40);
                    if (switchind == 3) switchind++;
                }
            }
        }

        public static void DrawField(RenderWindow w) {
            RectangleShape brik = new RectangleShape(new Vector2f(100, 100));
            bool color = true;
            float x = 50, y = 100, step = 100;
            //white = true black = false
            for (int i = 1; i <= 64; i++) {
                if (pressedbrik == i-1)
                {
                    Color c = new Color(17, 120, 61, 150);
                    brik.FillColor = c;
                }
                else {
                    if (curentbrik == i-1)
                    {
                        Color c = new Color(18, 5, 255,150);
                        brik.FillColor = c;
                    }
                    else
                    {
                        if (color)
                        {
                            brik.FillColor = Color.White;
                        }
                        else
                        {
                            Color c = new Color(105, 105, 105);
                            brik.FillColor = c;
                        }
                    }
                }
                brik.OutlineThickness = 2;
                brik.OutlineColor = Color.Black;               
                brik.Position = new Vector2f(x, y);
                if (i % 8 == 0)
                {
                    y += step;
                    x = 50;
                }
                else {
                    x += step;
                    color = !color;
                }
                w.Draw(brik);
            }
        }

        private static void _Close(object sender, EventArgs e)
        {
            if (online)
            {
                byte id = client.user_ip();
                client.User().GetStream().Write(new byte[] { 253, id }, 0, 2);
            }
            Console.WriteLine("kek");
            window.Close();
        }
    }
}

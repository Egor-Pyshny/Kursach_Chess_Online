using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class BlackRook  : Figure
    {
        public Sprite spr { get; private set; }
        public Sprite smallspr { get; private set; }
        private Texture tex, smalltex;

        private string path = Environment.CurrentDirectory + "\\brook.png";
        private string smallpath = Environment.CurrentDirectory + "\\smallbrook.png";
        public BlackRook(int x, int y) : base(x, y,7)
        {
            this.tex = new Texture(path);
            this.spr = new Sprite(tex);
            this.smalltex = new Texture(smallpath);
            this.smallspr = new Sprite(smalltex);
        }

        public override Sprite getspr()
        {
            return spr;
        }
        public override Sprite getsmallspr()
        {
            return smallspr;
        }
        public override int[] GetPath(int ind, int newInd)
        {
            List<Int32> path = new List<int>();
            path.Add(ind);
            int yo, yn;
            yo = (int)(ind / 8) * 100 + 110;
            yn = (int)(newInd / 8) * 100 + 110;
            int tmp = 0;
            if ((newInd - ind) % 8 == 0)
            {
                if (newInd < ind) { tmp = -8; } else { tmp = 8; }
            }
            if (yo == yn)
            {
                if (newInd < ind) { tmp = -1; } else { tmp = 1; }
            }
            int i = ind + tmp;
            while (Math.Abs(i) != newInd && i >= 0 && i <= 63)
            {
                path.Add(ind);
                i += tmp;
            }
            return path.ToArray();
        }

        public override Figure Move(int ind, int newInd, ref List<Figure> list)
        {
            int oldx = this.x, oldy = this.y;
            this.y = (int)(newInd / 8) * 100 + 110;
            this.x = (newInd % 8) * 100 + 60;
            this.ind = newInd;
            Figure f = new Figure();
            f = list[newInd];
            list[newInd] = this;
            list[ind] = new NullFigure(oldx, oldy);
            return f;
        }

        public override void Undo(int ind, int newInd, ref List<Figure> list, Figure deletedFigure)
        {
            this.y = (int)(newInd / 8) * 100 + 110;
            this.x = (newInd % 8) * 100 + 60;
            this.ind = newInd;
            list[newInd] = this;
            list[ind] = deletedFigure;
        }

        public override bool CanMove(int ind, int newInd, List<Figure> list)
        {
            int yo, yn;
            yo = (int)(ind / 8) * 100 + 110;
            yn = (int)(newInd / 8) * 100 + 110;
            int tmp = 0;
            if ((newInd - ind) % 8 == 0)
            {
                if (newInd < ind) { tmp = -8; } else { tmp = 8; }
            }
            if (yo == yn)
            {
                if (newInd < ind) { tmp = -1; } else { tmp = 1; }
            }
            int i = ind + tmp;
            while (Math.Abs(i) != newInd && i >= 0 && i <= 63)
            {
                if (!(list[i] is NullFigure)) return false;
                i += tmp;
            }
            return true;
        }
    }
}

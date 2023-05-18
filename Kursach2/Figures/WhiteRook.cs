using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class WhiteRook : Figure
    {
        public Sprite spr { get; private set; }
        public Sprite smallspr { get; private set; }
        private Texture tex, smalltex;
        private string path = Environment.CurrentDirectory + "\\wrook.png";
        private string smallpath = Environment.CurrentDirectory + "\\smallwrook.png";
        public WhiteRook(int x, int y) : base(x, y, 1)
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

        public override int[] GetPath(int ind, int ind2)
        {
            List<Int32> path = new List<int>();
            path.Add(ind);
            int yo, yn;
            yo = (int)(ind / 8) * 100 + 110;
            yn = (int)(ind2 / 8) * 100 + 110;
            int tmp = 0;
            if ((ind2 - ind) % 8 == 0)
            {
                tmp = (ind2 < ind) ? (-8) : (8);
                if (ind2 < ind) { tmp = -8; } else { tmp = 8; }
            }
            if (yo == yn)
            {
                if (ind2 < ind) { tmp = -1; } else { tmp = 1; }
            }
            int i = ind + tmp;
            while (Math.Abs(i) != ind2 && tmp != 0 &&  i >= 0 && i <= 63)
            {
                path.Add(i);
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

        public override bool CanMove(int ind, int newInd, List<Figure> list) {
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

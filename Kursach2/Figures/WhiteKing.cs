using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class WhiteKing : Figure
    {
        public Sprite spr { get; private set; }
        public Sprite smallspr { get; private set; }
        private Texture tex, smalltex;
        private string path = Environment.CurrentDirectory + "\\wking.png";
        private string smallpath = Environment.CurrentDirectory + "\\smallwhorse.png";
        public WhiteKing(int x, int y) : base(x, y,4)
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

        public override bool CanMove(int ind1, int newInd, List<Figure> list)
        {
            int tmp1, tmp2;
            tmp1 = (int)(newInd / 8) * 100 + 110;
            tmp2 = (int)(ind1 / 8) * 100 + 110;
            if ((Math.Abs(newInd - ind1) == 1 && Math.Abs(tmp1 - tmp2) == 0) || ((Math.Abs(newInd - ind1) == 7 || Math.Abs(newInd - ind1) == 8 || Math.Abs(newInd - ind1) == 9) && Math.Abs(tmp1 - tmp2) == 100))
            {
                return  true;
            }
            return false;
        }
    }
}

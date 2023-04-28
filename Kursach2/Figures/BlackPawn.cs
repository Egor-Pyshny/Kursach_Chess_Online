using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Kursach2.Figures
{
    public class BlackPawn : Figure
    {
        public Sprite spr { get; private set; }
		public Sprite smallspr { get; private set; }
		private Texture tex, smalltex;
		private string path = Environment.CurrentDirectory + "\\bpawn.png";
		private string smallpath = Environment.CurrentDirectory + "\\smallbpawn.png";
		public BlackPawn(int x, int y) : base(x, y,12) {
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
		public override int[] GetPath(int ind1, int ind2)
		{
			return new int[1] { ind1 };
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
			bool res = false;
			if (list[newInd] is NullFigure)
			{
				if (newInd - ind == 8)
				{
					res = true;
				}
				if (((newInd <= 31) && (newInd >= 24)) && (newInd - ind == 16) && (list[newInd - 8] is NullFigure))
				{
					res = true;
				}
			}
			else
			{
				int tmp1, tmp2;
				tmp1 = (int)(newInd / 8) * 100 + 110;
				tmp2 = (int)(ind / 8) * 100 + 110;
				if (Math.Abs(tmp2 - tmp1) == 100 && (newInd - ind == 9 || newInd - ind == 7))
				{
					res = true;
				}
			}
			return res;
		}
	}
}

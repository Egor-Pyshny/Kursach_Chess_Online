using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class WhiteBishop : Figure
	{
        public Sprite spr { get; private set; }
		public Sprite smallspr { get; private set; }
		private Texture tex,smalltex;
        private string path = Environment.CurrentDirectory + "\\wbishop.png";
		private string smallpath = Environment.CurrentDirectory + "\\smallwbishop.png";
		public WhiteBishop(int x, int y) : base(x, y,3)
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
			int i1, i2;
			float sft;
			i1 = ind2 / 8;
			i2 = ind / 8;
			sft = Math.Abs(i1 - i2);
			int tmp = 0;
			if (ind > ind2)
			{
				if ((float)((ind + sft - ind2) / 8) == sft)
					tmp = -7;
				if ((float)((ind - sft - ind2) / 8) == sft)
					tmp = -9;
			}
			else if (ind < ind2)
			{
				if ((float)((ind2 - sft - ind) / 8) == sft)
					tmp = 9;
				if ((float)((ind2 + sft - ind) / 8) == sft)
					tmp = 7;
			}
			int i = ind + tmp;
			while (i != ind2 && i >= 0 && i <= 63)
			{
				path.Add(i);
				i = i + tmp;
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
			bool res = false;
			if (Math.Abs(newInd -ind) % 8 >= 1 && Math.Abs(newInd - ind) % 8 <= 7)
			{
				int i1, i2;
				float sft;
				i1 = newInd / 8;
				i2 = ind / 8;
				sft = Math.Abs(i1 - i2);
				int tmp = 0;
				if (ind > newInd)
				{
					if ((float)((ind + sft - newInd) / 8) == sft)
						tmp = -7;
					if ((float)((ind - sft - newInd) / 8) == sft)
						tmp = -9;					
				}
				else if (ind < newInd)
				{
					if ((float)((newInd - sft - ind) / 8) == sft)
						tmp = 9;
					if ((float)((newInd + sft - ind) / 8) == sft)
						tmp = 7;					
				}
				int i = ind + tmp;
				while (i != newInd && i >= 0 && i <= 63)
				{
					if (!(list[i] is NullFigure)) return false;
					i = i + tmp;
				}
				res = true;
			}
			return res;
        }
    }
}

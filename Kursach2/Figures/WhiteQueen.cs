using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class WhiteQueen : Figure
    {
        public Sprite spr { get; private set; }
		public Sprite smallspr { get; private set; }
		private Texture tex, smalltex;
		private string path = Environment.CurrentDirectory + "\\wqueen.png";
		private string smallpath = Environment.CurrentDirectory + "\\smallwqueen.png";
		public WhiteQueen(int x, int y) : base(x, y,5)
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
			int tmp1 = 0;
			if ((ind2 - ind) % 8 == 0)
			{
				if (ind2 < ind) { tmp1 = -8; } else { tmp1 = 8; }
			}
			if (yo == yn)
			{
				if (ind2 < ind) { tmp1 = -1; } else { tmp1 = 1; }
			}
			int i3 = ind + tmp1;
			if (tmp1 != 0)
			{
				while (Math.Abs(i3) != ind2 && i3 >= 0 && i3 <= 63)
				{
					path.Add(ind);
					i3 += tmp1;
				}
			}
			bool res = true;
			if (Math.Abs(ind2 - ind) % 8 >= 1 && Math.Abs(ind2 - ind) % 8 <= 7)
			{
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
					int i = ind + tmp;
					while (i > ind2 && i >= 0 && i <= 63)
					{
						path.Add(ind);
						i = i + tmp;
					}
				}
				if (ind < ind2)
				{
					if ((float)((ind2 - sft - ind) / 8) == sft)
						tmp = 9;
					if ((float)((ind2 + sft - ind) / 8) == sft)
						tmp = 7;
					int i = ind + tmp;
					while (i < ind2 && i >= 0 && i <= 63)
					{
						path.Add(ind);
						i = i + tmp;
					}
				}
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
			bool rookmove = true;
			int yo, yn;
			yo = (int)(ind / 8) * 100 + 110;
			yn = (int)(newInd / 8) * 100 + 110;
			int tmp1 = 0;
			if ((newInd - ind) % 8 == 0)
			{
				if (newInd < ind) { tmp1 = -8; } else { tmp1 = 8; }
			}
			if (yo == yn)
			{
				if (newInd < ind) { tmp1 = -1; } else { tmp1 = 1; }
			}
			int i3 = ind + tmp1;
			while (Math.Abs(i3) != newInd && i3 >= 0 && i3 <= 63)
			{
				if (!(list[i3] is NullFigure)) {
					rookmove = false;
					break;
				}
				i3 += tmp1;
			}
			bool res=true;
			if (Math.Abs(newInd - ind) % 8 >= 1 && Math.Abs(newInd - ind) % 8 <= 7)
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
					int i = ind + tmp;
					while (i > newInd && i >= 0 && i <= 63)
					{
						if (!(list[i] is NullFigure))
						{
							res = false;
							break;
						}
						i = i + tmp;
					}
				}
				if (ind < newInd)
				{
					if ((float)((newInd - sft - ind) / 8) == sft)
						tmp = 9;
					if ((float)((newInd + sft - ind) / 8) == sft)
						tmp = 7;
					int i = ind + tmp;
					while (i < newInd && i >= 0 && i <= 63)
					{
						if (!(list[i] is NullFigure))
						{
							res = false;
							break;
						}
						i = i + tmp;
					}

				}
			}
			else {
				res = false;
			}
			return res||rookmove;       
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    public class NullFigure : Figure
    {
        public Sprite spr { get; }
        public NullFigure(int x, int y) : base(x, y, 0)
        {
        }

        public override Sprite getspr()
        {
            return null;
        }

        public override Sprite getsmallspr()
        {
            return null;
        }
    }
}

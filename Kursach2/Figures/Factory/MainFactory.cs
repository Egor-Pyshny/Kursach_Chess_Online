using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kursach2.Figures.Factory
{
    public class MainFactory
    {
        private Figure f;
        private IFactory factory;

        private IFactory GetFactory(int ind)
        {
            switch (ind)
            {
                case 6: return new PawnFactory();
                case 1: return new RookFactory();
                case 2: return new HorseFactory();
                case 3: return new BishopFactory();
                case 5: return new QueenFactory();
                case 4: return new KingFactory();
                case 0: return new NullFactoty();
                default: throw new Exception("Type error");
            }
        }

        public Figure Produce(int ftype, int color, int x, int y)
        {
            factory = GetFactory(ftype);
            f = factory.CreateFigure(color, x, y);
            return f;
        }
    }
}

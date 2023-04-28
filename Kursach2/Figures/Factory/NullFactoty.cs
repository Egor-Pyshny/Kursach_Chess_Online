using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kursach2.Figures.Factory
{
    public class NullFactoty : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            return new NullFigure(x, y); 
        }
    }
}

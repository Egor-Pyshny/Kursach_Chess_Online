using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kursach2.Figures.Factory
{
    public interface IFactory
    {
        Figure CreateFigure(int index,int x,int y);
    }
}

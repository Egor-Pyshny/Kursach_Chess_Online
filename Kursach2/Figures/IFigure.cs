using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Kursach2.Figures
{
    interface IFigure
    {
        Sprite getspr();
        bool CanMove(int ind1, int ind2,List<Figure> list);
        Figure Move(int ind1, int ind2,ref List<Figure> list);
        void Undo(int ind1, int ind2, ref List<Figure> list, Figure deletedFigure);
        int[] GetPath(int ind1, int ind2);
    }
}

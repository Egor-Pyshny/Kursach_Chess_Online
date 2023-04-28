namespace Kursach2.Figures.Factory
{
    public class QueenFactory : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            if (index == 1)
            {
                return new WhiteQueen(x, y);
            }
            else
            {
                return new BlackQueen(x, y);
            }
        }
    }
}

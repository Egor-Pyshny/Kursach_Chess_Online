namespace Kursach2.Figures.Factory
{
    public class KingFactory : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            if (index == 1)
            {
                return new WhiteKing(x, y);
            }
            else
            {
                return new BlackKing(x, y);
            }
        }
    }
}

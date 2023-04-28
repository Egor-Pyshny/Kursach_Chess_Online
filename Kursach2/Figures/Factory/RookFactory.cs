namespace Kursach2.Figures.Factory
{
    public class RookFactory : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            if (index == 1)
            {
                return new WhiteRook(x, y);
            }
            else
            {
                return new BlackRook(x, y);
            }
        }
    }
}

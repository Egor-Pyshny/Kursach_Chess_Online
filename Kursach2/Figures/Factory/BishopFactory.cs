namespace Kursach2.Figures.Factory
{
    public class BishopFactory : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            if (index == 1)
            {
                return new WhiteBishop(x, y);
            }
            else
            {
                return new BlackBishop(x, y);
            }
        }
    }
}

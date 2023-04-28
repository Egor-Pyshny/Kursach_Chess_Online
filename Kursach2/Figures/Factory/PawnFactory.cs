namespace Kursach2.Figures.Factory
{
    public class PawnFactory : IFactory
    {
        public Figure CreateFigure(int index,int x,int y) {
            if (index == 1)
            {
                return new WhitePawn(x, y);
            }
            else {
                return new BlackPawn(x, y);
            }
        }
    }
}

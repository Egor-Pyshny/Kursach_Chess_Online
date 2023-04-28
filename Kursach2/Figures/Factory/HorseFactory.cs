namespace Kursach2.Figures.Factory
{
    public class HorseFactory : IFactory
    {
        public Figure CreateFigure(int index, int x, int y)
        {
            if (index == 1)
            {
                return new WhiteHorse(x, y);
            }
            else
            {
                return new BlackHorse(x, y);
            }
        }
    }
}

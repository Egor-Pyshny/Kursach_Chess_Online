using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Kursach2.Figures
{
    public class Figure : IFigure
    {
        public int x { get; set; }
        public int y { get; set; }
        public int ind { get; set; }
        public int ftype { get; set; }
        private int amount = 0;
        public int Amount { get { return amount; } private set { amount = value; } }
        private List<Figure> figurelist = new List<Figure>();
        public List<Figure> whitedeletedfigure = new List<Figure>();
        public List<Figure> blackdeletedfigure = new List<Figure>();
        internal List<Figure> FigureList { get { return figurelist; } set { figurelist = value; } }
        public Figure this[int index]
        {
            get
            {
                if (index < figurelist.Count)
                {
                    return figurelist[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (index < figurelist.Count)
                {
                    figurelist[index] = value;
                }
                else if (index < figurelist.Count + 1)
                {
                    figurelist.Add(value);
                    amount++;
                }
                else throw new Exception("Index out of bounds");
            }
        }
        public Figure(int x,int y,int type)
        {
            this.x = x;
            this.y = y;
            this.ftype = type;
            this.ind = ((y - 100) / 100) * 8 + (x - 50) / 100;           
        }

        public Figure(){}

        public virtual Sprite getspr() { return null;}
        public virtual bool CanMove(int ind, int newInd, List<Figure> list) { return false; }
        public virtual Figure Move(int ind, int newInd, ref List<Figure> list) { return null; }
        public virtual void Undo(int ind1, int ind2, ref List<Figure> list, Figure deletedFigure) { }
        public virtual int[] GetPath(int ind1, int ind2) { return null; }
        public virtual Sprite getsmallspr(){return null;}
    }
}

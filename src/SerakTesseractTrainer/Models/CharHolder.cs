using System;
using System.Drawing;

namespace SerakTesseractTrainer.Models
{
    public class CharHolder
    {
        public char Character { get; set; }

        public Rectangle DrawRect { get; set; }

        public Rectangle ImageRectangle { get; set; }

        public CharHolder()
        {
            Character = (char)new Random().Next(65, 91);
        }

        public Rectangle GetRelativePosition(Rectangle newRect)
        {
            return new Rectangle(PercentageCalculator(DrawRect.X, ImageRectangle.Width, newRect.Width),
                PercentageCalculator(DrawRect.Y, ImageRectangle.Height, newRect.Height),
                PercentageCalculator(DrawRect.Width, ImageRectangle.Width, newRect.Width),
                PercentageCalculator(DrawRect.Height, ImageRectangle.Height, newRect.Height));
        }

        private int PercentageCalculator(int value, int originalPercentage, int newPercentage)
        {
            if (originalPercentage <= 0 || newPercentage <= 0)
                return value;

            return (value * newPercentage) / originalPercentage;
        }
    }
}

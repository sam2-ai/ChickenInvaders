using System;
using System.Drawing;

namespace ChickenInvaders
{
    public class Star
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; set; }
        public float Size { get; set; }
        public int Brightness { get; set; }
        public float TwinklePhase { get; set; }
        
        private static Random rng = new Random();
        
        public Star(int maxWidth, int maxHeight)
        {
            Reset(maxWidth, maxHeight, true);
        }
        
        public void Reset(int maxWidth, int maxHeight, bool randomY = false)
        {
            X = rng.Next(0, maxWidth);
            Y = randomY ? rng.Next(0, maxHeight) : -5;
            Speed = (float)(rng.NextDouble() * 2 + 0.5);
            Size = (float)(rng.NextDouble() * 2 + 1);
            Brightness = rng.Next(100, 255);
            TwinklePhase = (float)(rng.NextDouble() * Math.PI * 2);
        }
        
        public void Update(int maxHeight, int maxWidth)
        {
            Y += Speed;
            TwinklePhase += 0.1f;
            
            if (Y > maxHeight)
            {
                Reset(maxWidth, maxHeight, false);
            }
        }
        
        public Color GetColor()
        {
            int twinkle = (int)(Math.Sin(TwinklePhase) * 30);
            int finalBrightness = Math.Max(50, Math.Min(255, Brightness + twinkle));
            return Color.FromArgb(finalBrightness, finalBrightness, finalBrightness + 20);
        }
    }
}

using System;
using System.Drawing;

namespace ChickenInvaders
{
    public class Particle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Life { get; set; }
        public float MaxLife { get; set; }
        public float Size { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public ParticleType Type { get; set; }
        
        public bool IsAlive => Life > 0;
        
        private static Random rng = new Random();
        
        public Particle(float x, float y, ParticleType type)
        {
            X = x;
            Y = y;
            Type = type;
            
            switch (type)
            {
                case ParticleType.Explosion:
                    float angle = (float)(rng.NextDouble() * Math.PI * 2);
                    float speed = (float)(rng.NextDouble() * 6 + 2);
                    VelocityX = (float)Math.Cos(angle) * speed;
                    VelocityY = (float)Math.Sin(angle) * speed;
                    MaxLife = Life = (float)(rng.NextDouble() * 20 + 15);
                    Size = (float)(rng.NextDouble() * 6 + 3);
                    StartColor = Color.FromArgb(255, 255, 200, 50);
                    EndColor = Color.FromArgb(0, 255, 50, 0);
                    break;
                    
                case ParticleType.Feather:
                    VelocityX = (float)(rng.NextDouble() * 4 - 2);
                    VelocityY = (float)(rng.NextDouble() * 3 + 1);
                    MaxLife = Life = (float)(rng.NextDouble() * 40 + 30);
                    Size = (float)(rng.NextDouble() * 8 + 4);
                    StartColor = Color.FromArgb(255, 255, 240, 200);
                    EndColor = Color.FromArgb(0, 255, 220, 150);
                    break;
                    
                case ParticleType.EngineTrail:
                    VelocityX = (float)(rng.NextDouble() * 2 - 1);
                    VelocityY = (float)(rng.NextDouble() * 2 + 2);
                    MaxLife = Life = (float)(rng.NextDouble() * 10 + 5);
                    Size = (float)(rng.NextDouble() * 4 + 2);
                    StartColor = Color.FromArgb(200, 255, 200, 100);
                    EndColor = Color.FromArgb(0, 255, 100, 0);
                    break;
                    
                case ParticleType.Spark:
                    float sparkAngle = (float)(rng.NextDouble() * Math.PI * 2);
                    float sparkSpeed = (float)(rng.NextDouble() * 8 + 4);
                    VelocityX = (float)Math.Cos(sparkAngle) * sparkSpeed;
                    VelocityY = (float)Math.Sin(sparkAngle) * sparkSpeed;
                    MaxLife = Life = (float)(rng.NextDouble() * 10 + 5);
                    Size = (float)(rng.NextDouble() * 3 + 1);
                    StartColor = Color.FromArgb(255, 255, 255, 200);
                    EndColor = Color.FromArgb(0, 255, 150, 50);
                    break;
            }
        }
        
        public void Update()
        {
            X += VelocityX;
            Y += VelocityY;
            Life -= 1;
            
            if (Type == ParticleType.Feather)
            {
                VelocityX += (float)(rng.NextDouble() * 0.4 - 0.2);
                VelocityY *= 0.98f;
            }
            else if (Type == ParticleType.Explosion)
            {
                VelocityX *= 0.95f;
                VelocityY *= 0.95f;
            }
        }
        
        public Color GetCurrentColor()
        {
            float t = Life / MaxLife;
            int a = (int)(StartColor.A * t + EndColor.A * (1 - t));
            int r = (int)(StartColor.R * t + EndColor.R * (1 - t));
            int g = (int)(StartColor.G * t + EndColor.G * (1 - t));
            int b = (int)(StartColor.B * t + EndColor.B * (1 - t));
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, a)),
                Math.Max(0, Math.Min(255, r)),
                Math.Max(0, Math.Min(255, g)),
                Math.Max(0, Math.Min(255, b))
            );
        }
        
        public float GetCurrentSize()
        {
            float t = Life / MaxLife;
            return Size * t;
        }
    }
    
    public enum ParticleType
    {
        Explosion,
        Feather,
        EngineTrail,
        Spark
    }
}

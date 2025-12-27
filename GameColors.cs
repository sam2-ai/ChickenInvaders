using System.Drawing;

namespace ChickenInvaders
{
    public static class GameColors
    {
        // Background colors
        public static Color SpaceBackground = Color.FromArgb(10, 10, 25);
        public static Color SpaceBackgroundLight = Color.FromArgb(20, 20, 40);
        public static Color PanelBackground = Color.FromArgb(25, 25, 45);
        public static Color HeaderBackground = Color.FromArgb(15, 20, 35);
        
        // Accent colors
        public static Color NeonGreen = Color.FromArgb(0, 255, 136);
        public static Color NeonBlue = Color.FromArgb(0, 200, 255);
        public static Color NeonPurple = Color.FromArgb(180, 100, 255);
        public static Color NeonPink = Color.FromArgb(255, 100, 180);
        public static Color NeonOrange = Color.FromArgb(255, 150, 50);
        public static Color NeonYellow = Color.FromArgb(255, 230, 50);
        
        // UI colors
        public static Color ButtonPrimary = Color.FromArgb(0, 200, 100);
        public static Color ButtonSecondary = Color.FromArgb(80, 80, 120);
        public static Color ButtonDanger = Color.FromArgb(220, 50, 80);
        public static Color ButtonWarning = Color.FromArgb(255, 180, 50);
        public static Color ButtonInfo = Color.FromArgb(50, 150, 255);
        
        // Text colors
        public static Color TextPrimary = Color.White;
        public static Color TextSecondary = Color.FromArgb(180, 180, 200);
        public static Color TextGold = Color.FromArgb(255, 215, 0);
        public static Color TextSilver = Color.FromArgb(192, 192, 192);
        public static Color TextBronze = Color.FromArgb(205, 127, 50);
        
        // Game element colors
        public static Color BulletColor = Color.FromArgb(255, 200, 50);
        public static Color BulletGlow = Color.FromArgb(255, 150, 0);
        public static Color ExplosionOuter = Color.FromArgb(255, 100, 50);
        public static Color ExplosionInner = Color.FromArgb(255, 255, 150);
        
        // Spaceship colors
        public static Color ShipBody = Color.FromArgb(100, 120, 180);
        public static Color ShipBodyLight = Color.FromArgb(150, 170, 220);
        public static Color ShipBodyDark = Color.FromArgb(60, 80, 140);
        public static Color ShipCockpit = Color.FromArgb(100, 200, 255);
        public static Color ShipEngine = Color.FromArgb(255, 150, 50);
        public static Color ShipEngineGlow = Color.FromArgb(255, 200, 100);
        
        // Chicken colors
        public static Color ChickenBody = Color.FromArgb(255, 220, 150);
        public static Color ChickenBodyDark = Color.FromArgb(220, 180, 100);
        public static Color ChickenWing = Color.FromArgb(255, 200, 120);
        public static Color ChickenBeak = Color.FromArgb(255, 150, 50);
        public static Color ChickenComb = Color.FromArgb(220, 50, 50);
        public static Color ChickenWattle = Color.FromArgb(200, 40, 40);
        public static Color ChickenEyeWhite = Color.White;
        public static Color ChickenEyePupil = Color.FromArgb(30, 30, 30);
        public static Color ChickenFeet = Color.FromArgb(255, 180, 80);
    }
}

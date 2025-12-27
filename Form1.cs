using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChickenInvaders
{
    public partial class Form1 : Form
    {
        // Game state
        private List<Bullet> bullets = new List<Bullet>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<Particle> particles = new List<Particle>();
        private List<Star> stars = new List<Star>();
        private Random rng = new Random();
        
        // Game settings
        private int playerSpeed = 10;
        private int bulletSpeed = 15;
        private int enemySpeed = 3;
        private int maxBullets = 8;
        private int spawnRate = 35;
        
        // Player state
        private float playerX;
        private float playerY;
        private int playerWidth = 64;
        private int playerHeight = 64;
        private bool leftDown, rightDown, spaceDown;
        private int shootCooldown = 0;
        private int maxShootCooldown = 8;
        
        // Game variables
        private int score = 0;
        private int lives = 3;
        private int spawnCounter = 0;
        private int level = 1;
        private int enemiesKilled = 0;
        private int enemiesPerLevel = 15;
        
        // Game screens
        private GameScreen currentScreen = GameScreen.Start;
        private string playerUsername = "";
        private bool gameStarted = false;
        private bool gamePaused = false;
        
        // Animation
        private float titleGlow = 0;
        private float engineFlicker = 0;

        public Form1()
        {
            InitializeComponent();
            
            // Initialize database
            DatabaseManager.InitializeDatabase();
            
            // Initialize stars
            InitializeStars();
            
            // Set up double buffering for smooth rendering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.DoubleBuffer, true);
            
            // Initialize player position
            playerX = (this.ClientSize.Width - playerWidth) / 2;
            playerY = this.ClientSize.Height - playerHeight - 80;
            
            // Ensure form can receive key events
            this.KeyPreview = true;
            this.Focus();
            
            // Start animation timer
            gameTimer.Start();
        }

        private void InitializeStars()
        {
            stars.Clear();
            for (int i = 0; i < 100; i++)
            {
                stars.Add(new Star(this.ClientSize.Width, this.ClientSize.Height));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Center the form on screen
            this.CenterToScreen();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (currentScreen == GameScreen.Playing)
            {
                playerY = this.ClientSize.Height - playerHeight - 80;
            }
            InitializeStars();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentScreen == GameScreen.Start)
            {
                // Handle username input
                if (e.KeyCode == Keys.Back && playerUsername.Length > 0)
                {
                    playerUsername = playerUsername.Substring(0, playerUsername.Length - 1);
                    txtUsername.Text = playerUsername;
                }
                else if (e.KeyCode == Keys.Enter && playerUsername.Length >= 2)
                {
                    StartGame();
                }
            }
            else if (currentScreen == GameScreen.Playing && !gamePaused)
            {
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) leftDown = true;
                if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) rightDown = true;
                if (e.KeyCode == Keys.Space) spaceDown = true;
                if (e.KeyCode == Keys.Escape) PauseGame();
            }
            else if (currentScreen == GameScreen.Playing && gamePaused)
            {
                if (e.KeyCode == Keys.Escape) ResumeGame();
            }

            // Prevent default arrow key behavior
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || 
                e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) leftDown = false;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) rightDown = false;
            if (e.KeyCode == Keys.Space) spaceDown = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (currentScreen == GameScreen.Playing && !gamePaused)
            {
                switch (keyData)
                {
                    case Keys.Left:
                    case Keys.A:
                        leftDown = true;
                        return true;
                    case Keys.Right:
                    case Keys.D:
                        rightDown = true;
                        return true;
                    case Keys.Space:
                        spaceDown = true;
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Update animation variables
            titleGlow += 0.05f;
            engineFlicker += 0.3f;
            
            // Update stars
            foreach (var star in stars)
            {
                star.Update(this.ClientSize.Height, this.ClientSize.Width);
            }
            
            if (currentScreen == GameScreen.Playing && !gamePaused)
            {
                UpdateGame();
            }
            
            // Update particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update();
                if (!particles[i].IsAlive)
                {
                    particles.RemoveAt(i);
                }
            }
            
            // Repaint
            this.Invalidate();
        }

        private void UpdateGame()
        {
            // Player movement
            if (leftDown)
            {
                playerX = Math.Max(10, playerX - playerSpeed);
            }
            if (rightDown)
            {
                playerX = Math.Min(this.ClientSize.Width - playerWidth - 10, playerX + playerSpeed);
            }

            // Shooting
            if (shootCooldown > 0) shootCooldown--;
            
            if (spaceDown && bullets.Count < maxBullets && shootCooldown == 0)
            {
                bullets.Add(new Bullet(playerX + playerWidth / 2 - 4, playerY - 10));
                shootCooldown = maxShootCooldown;
                
                // Add engine particles
                for (int i = 0; i < 2; i++)
                {
                    particles.Add(new Particle(playerX + playerWidth / 2, playerY + playerHeight, ParticleType.EngineTrail));
                }
            }

            // Update bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Y -= bulletSpeed;
                if (bullets[i].Y < -20)
                {
                    bullets.RemoveAt(i);
                }
            }

            // Spawn enemies
            spawnCounter++;
            int adjustedSpawnRate = Math.Max(15, spawnRate - (level - 1) * 3);
            if (spawnCounter > adjustedSpawnRate)
            {
                spawnCounter = 0;
                int x = rng.Next(20, this.ClientSize.Width - 80);
                enemies.Add(new Enemy(x, -60, level));
            }

            // Update enemies
            int currentEnemySpeed = enemySpeed + (level - 1);
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Y += currentEnemySpeed;
                enemies[i].WobblePhase += 0.1f;
                enemies[i].X += (float)Math.Sin(enemies[i].WobblePhase) * 1.5f;
                
                if (enemies[i].Y > this.ClientSize.Height)
                {
                    enemies.RemoveAt(i);
                    lives--;
                    if (lives <= 0) EndGame();
                }
            }

            // Collision detection
            Rectangle playerRect = new Rectangle((int)playerX + 10, (int)playerY + 10, playerWidth - 20, playerHeight - 20);
            
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Rectangle enemyRect = new Rectangle((int)enemies[i].X + 8, (int)enemies[i].Y + 8, 44, 44);
                bool enemyHit = false;
                
                // Check bullet collisions
                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    Rectangle bulletRect = new Rectangle((int)bullets[j].X, (int)bullets[j].Y, 8, 20);
                    
                    if (enemyRect.IntersectsWith(bulletRect))
                    {
                        // Create explosion particles
                        for (int p = 0; p < 15; p++)
                        {
                            particles.Add(new Particle(enemies[i].X + 30, enemies[i].Y + 30, ParticleType.Explosion));
                        }
                        // Create feather particles
                        for (int p = 0; p < 8; p++)
                        {
                            particles.Add(new Particle(enemies[i].X + 30, enemies[i].Y + 30, ParticleType.Feather));
                        }
                        
                        enemies.RemoveAt(i);
                        bullets.RemoveAt(j);
                        score += 100 * level;
                        enemiesKilled++;
                        
                        // Level up check
                        if (enemiesKilled >= enemiesPerLevel * level)
                        {
                            level++;
                        }
                        
                        enemyHit = true;
                        break;
                    }
                }
                
                if (enemyHit) continue;
                
                // Check player collision
                if (enemies.Count > i && enemyRect.IntersectsWith(playerRect))
                {
                    // Create explosion
                    for (int p = 0; p < 20; p++)
                    {
                        particles.Add(new Particle(enemies[i].X + 30, enemies[i].Y + 30, ParticleType.Explosion));
                    }
                    
                    enemies.RemoveAt(i);
                    lives--;
                    if (lives <= 0) EndGame();
                }
            }
        }

        private void StartGame()
        {
            currentScreen = GameScreen.Playing;
            gameStarted = true;
            gamePaused = false;
            ResetGame();
            this.Focus();
        }

        private void PauseGame()
        {
            gamePaused = true;
        }

        private void ResumeGame()
        {
            gamePaused = false;
            this.Focus();
        }

        private void ResetGame()
        {
            bullets.Clear();
            enemies.Clear();
            particles.Clear();
            
            lives = 3;
            score = 0;
            level = 1;
            enemiesKilled = 0;
            spawnCounter = 0;
            
            playerX = (this.ClientSize.Width - playerWidth) / 2;
            playerY = this.ClientSize.Height - playerHeight - 80;
            
            leftDown = rightDown = spaceDown = false;
        }

        private void EndGame()
        {
            gameStarted = false;
            currentScreen = GameScreen.GameOver;
            
            // Save score to database
            if (!string.IsNullOrEmpty(playerUsername))
            {
                DatabaseManager.SaveScore(playerUsername, score);
            }
        }

        private void ShowRankings()
        {
            currentScreen = GameScreen.Rankings;
        }

        private void BackToStart()
        {
            currentScreen = GameScreen.Start;
            ResetGame();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Draw background
            DrawBackground(g);
            
            // Draw stars
            DrawStars(g);
            
            // Draw particles (behind game objects)
            DrawParticles(g);
            
            switch (currentScreen)
            {
                case GameScreen.Start:
                    DrawStartScreen(g);
                    break;
                case GameScreen.Playing:
                    DrawGameScreen(g);
                    if (gamePaused) DrawPauseOverlay(g);
                    break;
                case GameScreen.GameOver:
                    DrawGameOverScreen(g);
                    break;
                case GameScreen.Rankings:
                    DrawRankingsScreen(g);
                    break;
            }
        }

        private void DrawBackground(Graphics g)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), 
                new Point(0, this.ClientSize.Height),
                GameColors.SpaceBackground,
                GameColors.SpaceBackgroundLight))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void DrawStars(Graphics g)
        {
            foreach (var star in stars)
            {
                using (SolidBrush brush = new SolidBrush(star.GetColor()))
                {
                    g.FillEllipse(brush, star.X, star.Y, star.Size, star.Size);
                }
            }
        }

        private void DrawParticles(Graphics g)
        {
            foreach (var particle in particles)
            {
                using (SolidBrush brush = new SolidBrush(particle.GetCurrentColor()))
                {
                    float size = particle.GetCurrentSize();
                    g.FillEllipse(brush, particle.X - size / 2, particle.Y - size / 2, size, size);
                }
            }
        }

        private void DrawStartScreen(Graphics g)
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            
            // Draw title with glow effect
            string title = "CHICKEN INVADERS";
            using (Font titleFont = new Font("Segoe UI", 48, FontStyle.Bold))
            {
                SizeF titleSize = g.MeasureString(title, titleFont);
                float titleX = centerX - titleSize.Width / 2;
                float titleY = 80;
                
                // Glow effect
                float glowIntensity = (float)(Math.Sin(titleGlow) * 0.3 + 0.7);
                Color glowColor = Color.FromArgb((int)(100 * glowIntensity), GameColors.NeonGreen);
                for (int i = 5; i > 0; i--)
                {
                    using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(20, glowColor)))
                    {
                        g.DrawString(title, titleFont, glowBrush, titleX - i, titleY - i);
                        g.DrawString(title, titleFont, glowBrush, titleX + i, titleY + i);
                    }
                }
                
                using (SolidBrush titleBrush = new SolidBrush(GameColors.NeonGreen))
                {
                    g.DrawString(title, titleFont, titleBrush, titleX, titleY);
                }
            }
            
            // Draw subtitle
            using (Font subFont = new Font("Segoe UI", 16))
            using (SolidBrush subBrush = new SolidBrush(GameColors.TextSecondary))
            {
                string subtitle = "Defend Earth from the Chicken Invasion!";
                SizeF subSize = g.MeasureString(subtitle, subFont);
                g.DrawString(subtitle, subFont, subBrush, centerX - subSize.Width / 2, 160);
            }
            
            // Draw decorative chicken
            DrawChicken(g, new Rectangle(centerX - 40, 200, 80, 80));
            
            // Draw username panel
            int panelWidth = 400;
            int panelHeight = 280;
            int panelX = centerX - panelWidth / 2;
            int panelY = 300;
            
            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(200, GameColors.PanelBackground)))
            {
                DrawRoundedRectangle(g, panelBrush, panelX, panelY, panelWidth, panelHeight, 15);
            }
            
            // Draw panel border
            using (Pen borderPen = new Pen(GameColors.NeonBlue, 2))
            {
                DrawRoundedRectangleBorder(g, borderPen, panelX, panelY, panelWidth, panelHeight, 15);
            }
            
            // Username label
            using (Font labelFont = new Font("Segoe UI", 14))
            using (SolidBrush labelBrush = new SolidBrush(GameColors.TextPrimary))
            {
                g.DrawString("Enter Your Username:", labelFont, labelBrush, panelX + 20, panelY + 20);
            }
            
            // Username input box visual
            Rectangle inputRect = new Rectangle(panelX + 20, panelY + 55, panelWidth - 40, 40);
            using (SolidBrush inputBg = new SolidBrush(Color.FromArgb(255, 30, 30, 50)))
            {
                g.FillRectangle(inputBg, inputRect);
            }
            using (Pen inputBorder = new Pen(GameColors.NeonBlue, 1))
            {
                g.DrawRectangle(inputBorder, inputRect);
            }
            
            // Draw username text
            using (Font inputFont = new Font("Segoe UI", 16))
            using (SolidBrush inputBrush = new SolidBrush(GameColors.TextPrimary))
            {
                string displayText = playerUsername + (DateTime.Now.Millisecond % 1000 < 500 ? "|" : "");
                g.DrawString(displayText, inputFont, inputBrush, inputRect.X + 10, inputRect.Y + 8);
            }
            
            // Instructions
            using (Font instrFont = new Font("Segoe UI", 11))
            using (SolidBrush instrBrush = new SolidBrush(GameColors.TextSecondary))
            {
                string[] instructions = {
                    "🎮 Controls:",
                    "← → or A/D - Move Spaceship",
                    "SPACE - Fire Weapons",
                    "ESC - Pause Game"
                };
                
                int y = panelY + 110;
                foreach (string line in instructions)
                {
                    g.DrawString(line, instrFont, instrBrush, panelX + 20, y);
                    y += 25;
                }
            }
            
            // Position and show username textbox
            txtUsername.Location = new Point(panelX + 20, panelY + 55);
            txtUsername.Size = new Size(panelWidth - 40, 40);
            txtUsername.Visible = true;
            
            // Position buttons
            btnStartGame.Location = new Point(panelX + 20, panelY + panelHeight - 60);
            btnStartGame.Size = new Size(170, 45);
            btnStartGame.Visible = true;
            
            btnViewRankings.Location = new Point(panelX + panelWidth - 190, panelY + panelHeight - 60);
            btnViewRankings.Size = new Size(170, 45);
            btnViewRankings.Visible = true;
            
            // Hide other buttons
            btnPause.Visible = false;
            btnResume.Visible = false;
            btnRestart.Visible = false;
            btnBackToMenu.Visible = false;
            btnPlayAgain.Visible = false;
        }

        private void DrawGameScreen(Graphics g)
        {
            // Draw player
            DrawSpaceship(g, new Rectangle((int)playerX, (int)playerY, playerWidth, playerHeight));
            
            // Draw engine trail
            if (gameStarted && !gamePaused)
            {
                float flickerIntensity = (float)(Math.Sin(engineFlicker) * 0.3 + 0.7);
                using (SolidBrush engineBrush = new SolidBrush(Color.FromArgb((int)(150 * flickerIntensity), GameColors.ShipEngineGlow)))
                {
                    g.FillEllipse(engineBrush, playerX + playerWidth / 2 - 8, playerY + playerHeight - 5, 16, 20);
                }
            }
            
            // Draw bullets
            foreach (var bullet in bullets)
            {
                DrawBullet(g, bullet);
            }
            
            // Draw enemies
            foreach (var enemy in enemies)
            {
                DrawChicken(g, new Rectangle((int)enemy.X, (int)enemy.Y, 60, 60));
            }
            
            // Draw HUD
            DrawHUD(g);
            
            // Hide start screen elements
            txtUsername.Visible = false;
            btnStartGame.Visible = false;
            btnViewRankings.Visible = false;
            btnBackToMenu.Visible = false;
            btnPlayAgain.Visible = false;
            
            // Show game buttons
            btnPause.Visible = !gamePaused;
            btnResume.Visible = gamePaused;
            btnRestart.Visible = true;
            
            btnPause.Location = new Point(this.ClientSize.Width - 200, 15);
            btnResume.Location = new Point(this.ClientSize.Width - 200, 15);
            btnRestart.Location = new Point(this.ClientSize.Width - 100, 15);
        }

        private void DrawHUD(Graphics g)
        {
            // Draw HUD background
            using (SolidBrush hudBg = new SolidBrush(Color.FromArgb(180, GameColors.HeaderBackground)))
            {
                g.FillRectangle(hudBg, 0, 0, this.ClientSize.Width, 60);
            }
            
            // Draw score
            using (Font scoreFont = new Font("Segoe UI", 18, FontStyle.Bold))
            using (SolidBrush scoreBrush = new SolidBrush(GameColors.NeonGreen))
            {
                g.DrawString($"SCORE: {score}", scoreFont, scoreBrush, 20, 15);
            }
            
            // Draw level
            using (Font levelFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (SolidBrush levelBrush = new SolidBrush(GameColors.NeonBlue))
            {
                string levelText = $"LEVEL {level}";
                SizeF levelSize = g.MeasureString(levelText, levelFont);
                g.DrawString(levelText, levelFont, levelBrush, this.ClientSize.Width / 2 - levelSize.Width / 2, 18);
            }
            
            // Draw lives
            using (Font livesFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (SolidBrush livesBrush = new SolidBrush(GameColors.NeonPink))
            {
                string livesText = "LIVES: ";
                g.DrawString(livesText, livesFont, livesBrush, 250, 18);
                
                // Draw heart icons
                for (int i = 0; i < lives; i++)
                {
                    DrawHeart(g, 330 + i * 30, 15, 25);
                }
            }
            
            // Draw username
            using (Font userFont = new Font("Segoe UI", 12))
            using (SolidBrush userBrush = new SolidBrush(GameColors.TextSecondary))
            {
                g.DrawString($"Player: {playerUsername}", userFont, userBrush, 450, 20);
            }
        }

        private void DrawHeart(Graphics g, int x, int y, int size)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y + size / 4, size / 2, size / 2, 180, 180);
                path.AddArc(x + size / 2, y + size / 4, size / 2, size / 2, 180, 180);
                path.AddLine(x + size, y + size / 2, x + size / 2, y + size);
                path.AddLine(x + size / 2, y + size, x, y + size / 2);
                path.CloseFigure();
                
                using (SolidBrush heartBrush = new SolidBrush(Color.FromArgb(255, 80, 100)))
                {
                    g.FillPath(heartBrush, path);
                }
            }
        }

        private void DrawPauseOverlay(Graphics g)
        {
            // Semi-transparent overlay
            using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                g.FillRectangle(overlayBrush, this.ClientRectangle);
            }
            
            // Pause text
            using (Font pauseFont = new Font("Segoe UI", 48, FontStyle.Bold))
            using (SolidBrush pauseBrush = new SolidBrush(GameColors.NeonYellow))
            {
                string pauseText = "PAUSED";
                SizeF pauseSize = g.MeasureString(pauseText, pauseFont);
                g.DrawString(pauseText, pauseFont, pauseBrush, 
                    this.ClientSize.Width / 2 - pauseSize.Width / 2,
                    this.ClientSize.Height / 2 - pauseSize.Height / 2 - 50);
            }
            
            using (Font instrFont = new Font("Segoe UI", 16))
            using (SolidBrush instrBrush = new SolidBrush(GameColors.TextSecondary))
            {
                string instrText = "Press ESC or Resume to continue";
                SizeF instrSize = g.MeasureString(instrText, instrFont);
                g.DrawString(instrText, instrFont, instrBrush,
                    this.ClientSize.Width / 2 - instrSize.Width / 2,
                    this.ClientSize.Height / 2 + 20);
            }
        }

        private void DrawGameOverScreen(Graphics g)
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            
            // Draw "Game Over" title
            using (Font titleFont = new Font("Segoe UI", 56, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(GameColors.NeonPink))
            {
                string title = "GAME OVER";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, titleBrush, centerX - titleSize.Width / 2, 60);
            }
            
            // Draw score panel
            int panelWidth = 500;
            int panelHeight = 450;
            int panelX = centerX - panelWidth / 2;
            int panelY = 150;
            
            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(220, GameColors.PanelBackground)))
            {
                DrawRoundedRectangle(g, panelBrush, panelX, panelY, panelWidth, panelHeight, 15);
            }
            using (Pen borderPen = new Pen(GameColors.NeonPurple, 2))
            {
                DrawRoundedRectangleBorder(g, borderPen, panelX, panelY, panelWidth, panelHeight, 15);
            }
            
            // Player stats
            using (Font statFont = new Font("Segoe UI", 20, FontStyle.Bold))
            {
                using (SolidBrush labelBrush = new SolidBrush(GameColors.TextSecondary))
                using (SolidBrush valueBrush = new SolidBrush(GameColors.NeonGreen))
                {
                    g.DrawString($"Player: {playerUsername}", statFont, labelBrush, panelX + 30, panelY + 20);
                    g.DrawString($"Final Score: {score}", statFont, valueBrush, panelX + 30, panelY + 55);
                    g.DrawString($"Level Reached: {level}", statFont, labelBrush, panelX + 30, panelY + 90);
                }
            }
            
            // Get player rank
            int playerRank = DatabaseManager.GetPlayerRank(score);
            using (Font rankFont = new Font("Segoe UI", 18, FontStyle.Bold))
            {
                Color rankColor = playerRank <= 3 ? GameColors.TextGold : GameColors.NeonBlue;
                using (SolidBrush rankBrush = new SolidBrush(rankColor))
                {
                    g.DrawString($"Your Rank: #{playerRank}", rankFont, rankBrush, panelX + 30, panelY + 125);
                }
            }
            
            // Draw leaderboard
            using (Font headerFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (SolidBrush headerBrush = new SolidBrush(GameColors.NeonYellow))
            {
                g.DrawString("🏆 TOP 10 LEADERBOARD", headerFont, headerBrush, panelX + 30, panelY + 165);
            }
            
            // Draw separator line
            using (Pen linePen = new Pen(GameColors.NeonBlue, 1))
            {
                g.DrawLine(linePen, panelX + 30, panelY + 195, panelX + panelWidth - 30, panelY + 195);
            }
            
            // Draw top scores
            var topScores = DatabaseManager.GetTopScores(10);
            using (Font scoreFont = new Font("Consolas", 12))
            {
                int y = panelY + 205;
                foreach (var ps in topScores)
                {
                    Color rowColor = GetRankColor(ps.Rank);
                    using (SolidBrush rowBrush = new SolidBrush(rowColor))
                    {
                        string rankStr = ps.Rank.ToString().PadLeft(2);
                        string nameStr = ps.Username.Length > 12 ? ps.Username.Substring(0, 12) : ps.Username.PadRight(12);
                        string scoreStr = ps.Score.ToString().PadLeft(8);
                        g.DrawString($"#{rankStr}  {nameStr}  {scoreStr} pts", scoreFont, rowBrush, panelX + 40, y);
                    }
                    y += 22;
                }
            }
            
            // Position buttons
            btnPlayAgain.Location = new Point(panelX + 30, panelY + panelHeight - 55);
            btnPlayAgain.Size = new Size(200, 45);
            btnPlayAgain.Visible = true;
            
            btnBackToMenu.Location = new Point(panelX + panelWidth - 230, panelY + panelHeight - 55);
            btnBackToMenu.Size = new Size(200, 45);
            btnBackToMenu.Visible = true;
            
            // Hide other elements
            txtUsername.Visible = false;
            btnStartGame.Visible = false;
            btnViewRankings.Visible = false;
            btnPause.Visible = false;
            btnResume.Visible = false;
            btnRestart.Visible = false;
        }

        private void DrawRankingsScreen(Graphics g)
        {
            int centerX = this.ClientSize.Width / 2;
            
            // Draw title
            using (Font titleFont = new Font("Segoe UI", 42, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(GameColors.NeonYellow))
            {
                string title = "🏆 HALL OF FAME 🏆";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, titleBrush, centerX - titleSize.Width / 2, 50);
            }
            
            // Draw rankings panel
            int panelWidth = 600;
            int panelHeight = 500;
            int panelX = centerX - panelWidth / 2;
            int panelY = 130;
            
            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(220, GameColors.PanelBackground)))
            {
                DrawRoundedRectangle(g, panelBrush, panelX, panelY, panelWidth, panelHeight, 15);
            }
            using (Pen borderPen = new Pen(GameColors.TextGold, 2))
            {
                DrawRoundedRectangleBorder(g, borderPen, panelX, panelY, panelWidth, panelHeight, 15);
            }
            
            // Draw header
            using (Font headerFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (SolidBrush headerBrush = new SolidBrush(GameColors.TextSecondary))
            {
                g.DrawString("RANK", headerFont, headerBrush, panelX + 40, panelY + 20);
                g.DrawString("PLAYER", headerFont, headerBrush, panelX + 120, panelY + 20);
                g.DrawString("SCORE", headerFont, headerBrush, panelX + 320, panelY + 20);
                g.DrawString("DATE", headerFont, headerBrush, panelX + 440, panelY + 20);
            }
            
            // Draw separator
            using (Pen linePen = new Pen(GameColors.NeonBlue, 1))
            {
                g.DrawLine(linePen, panelX + 30, panelY + 50, panelX + panelWidth - 30, panelY + 50);
            }
            
            // Draw scores
            var topScores = DatabaseManager.GetTopScores(10);
            using (Font scoreFont = new Font("Segoe UI", 14))
            {
                int y = panelY + 65;
                foreach (var ps in topScores)
                {
                    Color rowColor = GetRankColor(ps.Rank);
                    using (SolidBrush rowBrush = new SolidBrush(rowColor))
                    {
                        // Draw rank with medal for top 3
                        string rankDisplay = ps.Rank <= 3 ? GetMedal(ps.Rank) : $"#{ps.Rank}";
                        g.DrawString(rankDisplay, scoreFont, rowBrush, panelX + 40, y);
                        
                        // Draw name
                        string displayName = ps.Username.Length > 15 ? ps.Username.Substring(0, 15) : ps.Username;
                        g.DrawString(displayName, scoreFont, rowBrush, panelX + 120, y);
                        
                        // Draw score
                        g.DrawString(ps.Score.ToString("N0"), scoreFont, rowBrush, panelX + 320, y);
                        
                        // Draw date
                        g.DrawString(ps.PlayedAt.ToString("MM/dd/yy"), scoreFont, rowBrush, panelX + 440, y);
                    }
                    y += 40;
                }
                
                if (topScores.Count == 0)
                {
                    using (SolidBrush emptyBrush = new SolidBrush(GameColors.TextSecondary))
                    {
                        g.DrawString("No scores yet. Be the first to play!", scoreFont, emptyBrush, panelX + 150, panelY + 200);
                    }
                }
            }
            
            // Position back button
            btnBackToMenu.Location = new Point(centerX - 100, panelY + panelHeight + 20);
            btnBackToMenu.Size = new Size(200, 50);
            btnBackToMenu.Visible = true;
            
            // Hide other elements
            txtUsername.Visible = false;
            btnStartGame.Visible = false;
            btnViewRankings.Visible = false;
            btnPause.Visible = false;
            btnResume.Visible = false;
            btnRestart.Visible = false;
            btnPlayAgain.Visible = false;
        }

        private Color GetRankColor(int rank)
        {
            switch (rank)
            {
                case 1: return GameColors.TextGold;
                case 2: return GameColors.TextSilver;
                case 3: return GameColors.TextBronze;
                default: return GameColors.TextPrimary;
            }
        }

        private string GetMedal(int rank)
        {
            switch (rank)
            {
                case 1: return "🥇";
                case 2: return "🥈";
                case 3: return "🥉";
                default: return $"#{rank}";
            }
        }

        private void DrawBullet(Graphics g, Bullet bullet)
        {
            // Bullet glow
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(100, GameColors.BulletGlow)))
            {
                g.FillEllipse(glowBrush, bullet.X - 4, bullet.Y - 2, 16, 28);
            }
            
            // Bullet body
            using (LinearGradientBrush bulletBrush = new LinearGradientBrush(
                new Rectangle((int)bullet.X, (int)bullet.Y, 8, 24),
                GameColors.BulletColor,
                GameColors.BulletGlow,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(bulletBrush, bullet.X, bullet.Y, 8, 20);
            }
            
            // Bullet tip
            using (SolidBrush tipBrush = new SolidBrush(Color.White))
            {
                g.FillEllipse(tipBrush, bullet.X, bullet.Y - 4, 8, 8);
            }
        }

        private void DrawSpaceship(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            int cx = r.X + r.Width / 2;
            int cy = r.Y + r.Height / 2;
            
            // Main body gradient
            using (LinearGradientBrush bodyBrush = new LinearGradientBrush(
                r, GameColors.ShipBodyLight, GameColors.ShipBodyDark, LinearGradientMode.Horizontal))
            {
                // Main hull
                Point[] hull = {
                    new Point(cx, r.Y + 5),           // Nose
                    new Point(cx + 20, r.Y + 25),     // Right upper
                    new Point(cx + 25, r.Y + 45),     // Right middle
                    new Point(cx + 15, r.Bottom - 5), // Right lower
                    new Point(cx - 15, r.Bottom - 5), // Left lower
                    new Point(cx - 25, r.Y + 45),     // Left middle
                    new Point(cx - 20, r.Y + 25),     // Left upper
                };
                g.FillPolygon(bodyBrush, hull);
                
                // Hull outline
                using (Pen outlinePen = new Pen(GameColors.ShipBodyDark, 2))
                {
                    g.DrawPolygon(outlinePen, hull);
                }
            }
            
            // Wings
            Point[] leftWing = {
                new Point(cx - 25, r.Y + 40),
                new Point(r.X + 2, r.Y + 50),
                new Point(r.X + 5, r.Bottom - 8),
                new Point(cx - 15, r.Bottom - 10)
            };
            Point[] rightWing = {
                new Point(cx + 25, r.Y + 40),
                new Point(r.Right - 2, r.Y + 50),
                new Point(r.Right - 5, r.Bottom - 8),
                new Point(cx + 15, r.Bottom - 10)
            };
            
            using (LinearGradientBrush wingBrush = new LinearGradientBrush(
                r, GameColors.ShipBody, GameColors.ShipBodyDark, LinearGradientMode.Vertical))
            {
                g.FillPolygon(wingBrush, leftWing);
                g.FillPolygon(wingBrush, rightWing);
            }
            
            // Cockpit
            Rectangle cockpit = new Rectangle(cx - 10, r.Y + 15, 20, 25);
            using (LinearGradientBrush cockpitBrush = new LinearGradientBrush(
                cockpit, GameColors.ShipCockpit, Color.FromArgb(50, 100, 150), LinearGradientMode.Vertical))
            {
                g.FillEllipse(cockpitBrush, cockpit);
            }
            using (Pen cockpitPen = new Pen(Color.FromArgb(150, 220, 255), 1))
            {
                g.DrawEllipse(cockpitPen, cockpit);
            }
            
            // Cockpit reflection
            using (SolidBrush reflectBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
            {
                g.FillEllipse(reflectBrush, cx - 6, r.Y + 18, 8, 10);
            }
            
            // Engine glow
            using (SolidBrush engineBrush = new SolidBrush(GameColors.ShipEngine))
            {
                g.FillEllipse(engineBrush, cx - 8, r.Bottom - 12, 16, 10);
            }
            
            // Wing tips (weapons)
            using (SolidBrush weaponBrush = new SolidBrush(Color.FromArgb(80, 80, 100)))
            {
                g.FillRectangle(weaponBrush, r.X + 2, r.Y + 48, 6, 15);
                g.FillRectangle(weaponBrush, r.Right - 8, r.Y + 48, 6, 15);
            }
        }

        private void DrawChicken(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            int cx = r.X + r.Width / 2;
            int cy = r.Y + r.Height / 2;
            
            // Body shadow
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
                g.FillEllipse(shadowBrush, r.X + 8, r.Y + 12, r.Width - 12, r.Height - 16);
            }
            
            // Main body
            Rectangle bodyRect = new Rectangle(r.X + 6, r.Y + 8, r.Width - 12, r.Height - 18);
            using (LinearGradientBrush bodyBrush = new LinearGradientBrush(
                bodyRect, GameColors.ChickenBody, GameColors.ChickenBodyDark, LinearGradientMode.Vertical))
            {
                g.FillEllipse(bodyBrush, bodyRect);
            }
            
            // Body outline
            using (Pen bodyPen = new Pen(Color.FromArgb(180, 140, 80), 2))
            {
                g.DrawEllipse(bodyPen, bodyRect);
            }
            
            // Wings
            Point[] leftWing = {
                new Point(r.X + 8, cy - 5),
                new Point(r.X - 2, cy + 5),
                new Point(r.X + 5, cy + 15),
                new Point(r.X + 15, cy + 10)
            };
            Point[] rightWing = {
                new Point(r.Right - 8, cy - 5),
                new Point(r.Right + 2, cy + 5),
                new Point(r.Right - 5, cy + 15),
                new Point(r.Right - 15, cy + 10)
            };
            
            using (SolidBrush wingBrush = new SolidBrush(GameColors.ChickenWing))
            {
                g.FillPolygon(wingBrush, leftWing);
                g.FillPolygon(wingBrush, rightWing);
            }
            
            // Comb (on top)
            Point[] comb = {
                new Point(cx - 8, r.Y + 10),
                new Point(cx - 5, r.Y + 2),
                new Point(cx, r.Y + 8),
                new Point(cx + 5, r.Y),
                new Point(cx + 8, r.Y + 10)
            };
            using (SolidBrush combBrush = new SolidBrush(GameColors.ChickenComb))
            {
                g.FillPolygon(combBrush, comb);
            }
            
            // Eyes
            // Left eye
            g.FillEllipse(Brushes.White, cx - 14, r.Y + 18, 12, 14);
            g.FillEllipse(new SolidBrush(GameColors.ChickenEyePupil), cx - 11, r.Y + 22, 6, 8);
            g.FillEllipse(Brushes.White, cx - 10, r.Y + 23, 2, 2);
            
            // Right eye
            g.FillEllipse(Brushes.White, cx + 2, r.Y + 18, 12, 14);
            g.FillEllipse(new SolidBrush(GameColors.ChickenEyePupil), cx + 5, r.Y + 22, 6, 8);
            g.FillEllipse(Brushes.White, cx + 6, r.Y + 23, 2, 2);
            
            // Angry eyebrows
            using (Pen browPen = new Pen(Color.FromArgb(60, 40, 20), 2))
            {
                g.DrawLine(browPen, cx - 15, r.Y + 16, cx - 4, r.Y + 20);
                g.DrawLine(browPen, cx + 15, r.Y + 16, cx + 4, r.Y + 20);
            }
            
            // Beak
            Point[] beak = {
                new Point(cx, r.Y + 35),
                new Point(cx - 8, r.Y + 42),
                new Point(cx, r.Y + 50),
                new Point(cx + 8, r.Y + 42)
            };
            using (SolidBrush beakBrush = new SolidBrush(GameColors.ChickenBeak))
            {
                g.FillPolygon(beakBrush, beak);
            }
            
            // Wattle
            Point[] wattle = {
                new Point(cx, r.Y + 50),
                new Point(cx - 4, r.Y + 58),
                new Point(cx + 4, r.Y + 58)
            };
            using (SolidBrush wattleBrush = new SolidBrush(GameColors.ChickenWattle))
            {
                g.FillPolygon(wattleBrush, wattle);
            }
            
            // Feet
            using (SolidBrush feetBrush = new SolidBrush(GameColors.ChickenFeet))
            {
                // Left foot
                g.FillRectangle(feetBrush, cx - 12, r.Bottom - 10, 4, 10);
                g.FillEllipse(feetBrush, cx - 16, r.Bottom - 4, 12, 6);
                
                // Right foot
                g.FillRectangle(feetBrush, cx + 8, r.Bottom - 10, 4, 10);
                g.FillEllipse(feetBrush, cx + 4, r.Bottom - 4, 12, 6);
            }
        }

        private void DrawRoundedRectangle(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
                path.AddArc(x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(x, y + height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        private void DrawRoundedRectangleBorder(Graphics g, Pen pen, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
                path.AddArc(x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(x, y + height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                g.DrawPath(pen, path);
            }
        }

        // Button event handlers
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (playerUsername.Length >= 2)
            {
                StartGame();
            }
            else
            {
                MessageBox.Show("Please enter a username (at least 2 characters)", "Username Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
            }
        }

        private void btnViewRankings_Click(object sender, EventArgs e)
        {
            ShowRankings();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            PauseGame();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            ResumeGame();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            ResetGame();
            gamePaused = false;
            gameStarted = true;
            this.Focus();
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            currentScreen = GameScreen.Playing;
            ResetGame();
            gameStarted = true;
            this.Focus();
        }

        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            BackToStart();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            playerUsername = txtUsername.Text.Trim();
        }
    }

    public enum GameScreen
    {
        Start,
        Playing,
        GameOver,
        Rankings
    }

    public class Bullet
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        public Bullet(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class Enemy
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Level { get; set; }
        public float WobblePhase { get; set; }
        
        public Enemy(float x, float y, int level)
        {
            X = x;
            Y = y;
            Level = level;
            WobblePhase = 0;
        }
    }
}

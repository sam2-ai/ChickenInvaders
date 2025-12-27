using System.Drawing.Drawing2D;

namespace ChickenInvaders
{
    public partial class Form1 : Form
    {
        // Game state
        private List<PictureBox> bullets = new();
        private List<PictureBox> enemies = new();
        private Random rng = new();
        private int playerSpeed = 8;
        private int bulletSpeed = 12;
        private int enemySpeed = 2;
        private bool leftDown, rightDown, spaceDown;
        private int score = 0;
        private int lives = 3;
        private int spawnCounter = 0;
        private bool gameStarted = false;

        public Form1()
        {
            InitializeComponent();

            // center player at bottom - higher position so it's fully visible
            playerPictureBox.Left = (gamePanel.Width - playerPictureBox.Width) / 2;
            playerPictureBox.Top = gamePanel.Height - playerPictureBox.Height - 60;

            // Make sure timer is stopped and game hasn't started
            gameTimer.Stop();
            gameTimer.Enabled = false;

            // Prevent all controls from taking focus so arrow keys work
            SetControlsNotFocusable(this);
            
            // Ensure form can receive key events
            this.KeyPreview = true;
            this.Focus();

            // Show welcome panel and hide game elements
            ShowWelcomeScreen();
        }

        private void SetControlsNotFocusable(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.TabStop = false;
                }
                if (ctrl.HasChildren)
                {
                    SetControlsNotFocusable(ctrl);
                }
            }
        }

        private void ShowWelcomeScreen()
        {
            // Center the welcome panel
            welcomePanel.Left = (this.ClientSize.Width - welcomePanel.Width) / 2;
            welcomePanel.Top = (this.ClientSize.Height - welcomePanel.Height) / 2;
            welcomePanel.Visible = true;
            welcomePanel.BringToFront();
            
            // Hide game elements
            playerPictureBox.Visible = false;
        }

        private void HideWelcomeScreen()
        {
            welcomePanel.Visible = false;
            playerPictureBox.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            // keep player above bottom - fully visible
            if (playerPictureBox != null && gamePanel != null)
            {
                playerPictureBox.Top = gamePanel.Height - playerPictureBox.Height - 60;
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            // Don't allow keyboard input if game hasn't started
            if (!gameStarted) return;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) leftDown = true;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) rightDown = true;
            if (e.KeyCode == Keys.Space) spaceDown = true;

            // Prevent default arrow key behavior
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || 
                e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) leftDown = false;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) rightDown = false;
            if (e.KeyCode == Keys.Space) spaceDown = false;

            // Prevent default arrow key behavior
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || 
                e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
            }
        }

        // Override ProcessCmdKey to capture arrow keys before they are processed by the form
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Only process if game has started
            if (!gameStarted) return base.ProcessCmdKey(ref msg, keyData);

            // Process arrow keys and movement keys
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

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            // Don't process game logic if game hasn't started
            if (!gameStarted) return;

            // input - move player
            if (leftDown)
            {
                playerPictureBox.Left = Math.Max(8, playerPictureBox.Left - playerSpeed);
            }
            if (rightDown)
            {
                playerPictureBox.Left = Math.Min(gamePanel.Width - playerPictureBox.Width - 8, playerPictureBox.Left + playerSpeed);
            }

            // shooting - create bullet PictureBox
            if (spaceDown && bullets.Count < 6)
            {
                var bullet = new PictureBox
                {
                    Size = new Size(8, 12),
                    BackColor = Color.Orange,
                    Location = new Point(playerPictureBox.Left + playerPictureBox.Width / 2 - 4, playerPictureBox.Top - 12)
                };
                bullets.Add(bullet);
                gamePanel.Controls.Add(bullet);
                bullet.BringToFront();
            }

            // update bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Top -= bulletSpeed;
                if (bullets[i].Bottom < 0)
                {
                    gamePanel.Controls.Remove(bullets[i]);
                    bullets[i].Dispose();
                    bullets.RemoveAt(i);
                }
            }

            // spawn enemies - create enemy PictureBox
            spawnCounter++;
            if (spawnCounter > 40)
            {
                spawnCounter = 0;
                int w = 48, h = 48;
                int x = rng.Next(8, Math.Max(8, gamePanel.Width - w - 8));
                
                var enemy = new PictureBox
                {
                    Size = new Size(w, h),
                    BackColor = Color.Transparent,
                    Location = new Point(x, -h)
                };
                enemy.Paint += EnemyPictureBox_Paint;
                enemies.Add(enemy);
                gamePanel.Controls.Add(enemy);
                enemy.BringToFront();
            }

            // update enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Top += enemySpeed;
                if (enemies[i].Top > gamePanel.Height)
                {
                    gamePanel.Controls.Remove(enemies[i]);
                    enemies[i].Dispose();
                    enemies.RemoveAt(i);
                    lives--;
                    if (lives <= 0) EndGame();
                }
            }

            // collisions
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                bool removed = false;
                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (enemies[i].Bounds.IntersectsWith(bullets[j].Bounds))
                    {
                        gamePanel.Controls.Remove(enemies[i]);
                        enemies[i].Dispose();
                        enemies.RemoveAt(i);
                        
                        gamePanel.Controls.Remove(bullets[j]);
                        bullets[j].Dispose();
                        bullets.RemoveAt(j);
                        
                        score += 100;
                        removed = true;
                        break;
                    }
                }
                if (removed) continue;

                if (enemies.Count > i && enemies[i].Bounds.IntersectsWith(playerPictureBox.Bounds))
                {
                    gamePanel.Controls.Remove(enemies[i]);
                    enemies[i].Dispose();
                    enemies.RemoveAt(i);
                    lives--;
                    if (lives <= 0) EndGame();
                }
            }

            // Update labels
            lblScore.Text = $"Score: {score}";
            lblLives.Text = $"Lives: {lives}";
        }

        private void EndGame()
        {
            gameTimer.Stop();
            lblGameOver.Visible = true;
            lblInstructions.Visible = true;
            lblGameOver.BringToFront();
            lblInstructions.BringToFront();

            // Update button states
            btnStart.Enabled = false;
            btnPause.Enabled = false;
            btnResume.Enabled = false;
            btnRestart.Enabled = true;
        }

        private void ClearGame()
        {
            // Clear all bullets
            foreach (var bullet in bullets)
            {
                gamePanel.Controls.Remove(bullet);
                bullet.Dispose();
            }
            bullets.Clear();

            // Clear all enemies
            foreach (var enemy in enemies)
            {
                gamePanel.Controls.Remove(enemy);
                enemy.Dispose();
            }
            enemies.Clear();
        }

        private void ResetGame()
        {
            ClearGame();
            
            lives = 3;
            score = 0;
            playerPictureBox.Left = (gamePanel.Width - playerPictureBox.Width) / 2;
            playerPictureBox.Top = gamePanel.Height - playerPictureBox.Height - 60;
            spawnCounter = 0;
            lblGameOver.Visible = false;
            lblInstructions.Visible = false;
            lblScore.Text = "Score: 0";
            lblLives.Text = "Lives: 3";
        }

        // Button Event Handlers
        private void btnStartGame_Click(object? sender, EventArgs e)
        {
            // Hide welcome screen and start the game
            HideWelcomeScreen();
            btnStart_Click(sender, e);
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                ResetGame();
                gameTimer.Start();

                // Update button states
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnResume.Enabled = false;
                btnRestart.Enabled = true;

                // Give focus back to form so keyboard works
                this.Focus();
            }
        }

        private void btnPause_Click(object? sender, EventArgs e)
        {
            gameTimer.Stop();

            // Update button states
            btnPause.Enabled = false;
            btnResume.Enabled = true;

            // Give focus back to form
            this.Focus();
        }

        private void btnResume_Click(object? sender, EventArgs e)
        {
            gameTimer.Start();

            // Update button states
            btnPause.Enabled = true;
            btnResume.Enabled = false;

            // Give focus back to form
            this.Focus();
        }

        private void btnRestart_Click(object? sender, EventArgs e)
        {
            ResetGame();
            gameTimer.Start();
            gameStarted = true;

            // Update button states
            btnStart.Enabled = false;
            btnPause.Enabled = true;
            btnResume.Enabled = false;
            btnRestart.Enabled = true;

            // Give focus back to form
            this.Focus();
        }

        private void PlayerPictureBox_Paint(object? sender, PaintEventArgs e)
        {
            DrawPlayer(e.Graphics, new Rectangle(0, 0, playerPictureBox.Width, playerPictureBox.Height));
        }

        private void EnemyPictureBox_Paint(object? sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb != null)
            {
                DrawChicken(e.Graphics, new Rectangle(0, 0, pb.Width, pb.Height));
            }
        }

        private void DrawPlayer(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // body
            using (var body = new SolidBrush(Color.LightSteelBlue))
            using (var trim = new Pen(Color.DarkSlateBlue, 2))
            {
                var bodyR = new Rectangle(r.X, r.Y + 8, r.Width, r.Height - 8);
                g.FillEllipse(body, bodyR);
                g.DrawEllipse(trim, bodyR);

                // cockpit
                var cock = new Rectangle(r.X + r.Width / 2 - 8, r.Y + 12, 16, 12);
                g.FillEllipse(Brushes.WhiteSmoke, cock);
                g.DrawEllipse(Pens.Gray, cock);

                // fins
                Point[] leftFin = { new Point(r.X + 6, r.Y + r.Height - 6), new Point(r.X + 18, r.Y + r.Height - 16), new Point(r.X + 6, r.Y + r.Height - 22) };
                Point[] rightFin = { new Point(r.X + r.Width - 6, r.Y + r.Height - 6), new Point(r.X + r.Width - 18, r.Y + r.Height - 16), new Point(r.X + r.Width - 6, r.Y + r.Height - 22) };
                g.FillPolygon(Brushes.SteelBlue, leftFin);
                g.FillPolygon(Brushes.SteelBlue, rightFin);
            }
        }

        private void DrawChicken(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // body
            using (var b = new SolidBrush(Color.Gold))
            using (var p = new Pen(Color.OrangeRed, 2))
            {
                var body = new Rectangle(r.X + 6, r.Y + 10, r.Width - 12, r.Height - 18);
                g.FillEllipse(b, body);
                g.DrawEllipse(p, body);

                // eye
                var eye = new Rectangle(r.X + r.Width - 18, r.Y + 14, 6, 6);
                g.FillEllipse(Brushes.White, eye);
                g.FillEllipse(Brushes.Black, new Rectangle(eye.X + 2, eye.Y + 1, 2, 2));

                // beak
                Point[] beak = { new Point(r.X + 8, r.Y + r.Height - 20), new Point(r.X + 18, r.Y + r.Height - 16), new Point(r.X + 10, r.Y + r.Height - 12) };
                g.FillPolygon(Brushes.Orange, beak);

                // feet
                g.FillRectangle(Brushes.OrangeRed, r.X + 12, r.Y + r.Height - 6, 6, 6);
                g.FillRectangle(Brushes.OrangeRed, r.X + r.Width - 18, r.Y + r.Height - 6, 6, 6);
            }
        }
    }
}

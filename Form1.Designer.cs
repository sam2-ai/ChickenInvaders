namespace ChickenInvaders
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblLives;
        private System.Windows.Forms.Label lblGameOver;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.PictureBox playerPictureBox;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Panel welcomePanel;
        private System.Windows.Forms.Label lblWelcomeTitle;
        private System.Windows.Forms.Label lblWelcomeInstructions;
        private System.Windows.Forms.Button btnStartGame;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblScore = new Label();
            lblLives = new Label();
            lblGameOver = new Label();
            lblInstructions = new Label();
            gamePanel = new Panel();
            playerPictureBox = new PictureBox();
            gameTimer = new System.Windows.Forms.Timer(components);
            headerPanel = new Panel();
            btnStart = new Button();
            btnPause = new Button();
            btnResume = new Button();
            btnRestart = new Button();
            buttonPanel = new Panel();
            welcomePanel = new Panel();
            lblWelcomeTitle = new Label();
            lblWelcomeInstructions = new Label();
            btnStartGame = new Button();
            gamePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).BeginInit();
            headerPanel.SuspendLayout();
            welcomePanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(208, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chicken Invaders";
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.BackColor = Color.Transparent;
            lblScore.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblScore.ForeColor = Color.LightGreen;
            lblScore.Location = new Point(800, 15);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(69, 21);
            lblScore.TabIndex = 1;
            lblScore.Text = "Score: 0";
            // 
            // lblLives
            // 
            lblLives.AutoSize = true;
            lblLives.BackColor = Color.Transparent;
            lblLives.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLives.ForeColor = Color.OrangeRed;
            lblLives.Location = new Point(920, 15);
            lblLives.Name = "lblLives";
            lblLives.Size = new Size(65, 21);
            lblLives.TabIndex = 2;
            lblLives.Text = "Lives: 3";
            // 
            // lblGameOver
            // 
            lblGameOver.AutoSize = true;
            lblGameOver.BackColor = Color.FromArgb(220, 0, 0, 0);
            lblGameOver.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            lblGameOver.ForeColor = Color.White;
            lblGameOver.Location = new Point(350, 320);
            lblGameOver.Name = "lblGameOver";
            lblGameOver.Size = new Size(278, 65);
            lblGameOver.TabIndex = 3;
            lblGameOver.Text = "Game Over";
            lblGameOver.Visible = false;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.BackColor = Color.FromArgb(220, 0, 0, 0);
            lblInstructions.Font = new Font("Segoe UI", 10F);
            lblInstructions.ForeColor = Color.White;
            lblInstructions.Location = new Point(412, 400);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(167, 19);
            lblInstructions.TabIndex = 4;
            lblInstructions.Text = "Click Restart to play again";
            lblInstructions.Visible = false;
            // 
            // gamePanel
            // 
            gamePanel.BackColor = Color.FromArgb(24, 24, 30);
            gamePanel.Controls.Add(playerPictureBox);
            gamePanel.Dock = DockStyle.Fill;
            gamePanel.Location = new Point(0, 64);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(1024, 685);
            gamePanel.TabIndex = 5;
            // 
            // playerPictureBox
            // 
            playerPictureBox.BackColor = Color.Transparent;
            playerPictureBox.Location = new Point(488, 600);
            playerPictureBox.Name = "playerPictureBox";
            playerPictureBox.Size = new Size(48, 48);
            playerPictureBox.TabIndex = 0;
            playerPictureBox.TabStop = false;
            playerPictureBox.Paint += PlayerPictureBox_Paint;
            // 
            // gameTimer
            // 
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(40, 40, 48);
            headerPanel.Controls.Add(btnStart);
            headerPanel.Controls.Add(btnPause);
            headerPanel.Controls.Add(btnResume);
            headerPanel.Controls.Add(btnRestart);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblScore);
            headerPanel.Controls.Add(lblLives);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1024, 64);
            headerPanel.TabIndex = 6;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(50, 205, 50);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(240, 12);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(80, 36);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnPause
            // 
            btnPause.BackColor = Color.FromArgb(255, 165, 0);
            btnPause.Enabled = false;
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPause.ForeColor = Color.White;
            btnPause.Location = new Point(326, 12);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(80, 36);
            btnPause.TabIndex = 4;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // btnResume
            // 
            btnResume.BackColor = Color.FromArgb(30, 144, 255);
            btnResume.Enabled = false;
            btnResume.FlatStyle = FlatStyle.Flat;
            btnResume.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnResume.ForeColor = Color.White;
            btnResume.Location = new Point(413, 12);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(80, 36);
            btnResume.TabIndex = 5;
            btnResume.Text = "Resume";
            btnResume.UseVisualStyleBackColor = false;
            btnResume.Click += btnResume_Click;
            // 
            // btnRestart
            // 
            btnRestart.BackColor = Color.FromArgb(220, 20, 60);
            btnRestart.Enabled = false;
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRestart.ForeColor = Color.White;
            btnRestart.Location = new Point(499, 12);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(80, 36);
            btnRestart.TabIndex = 6;
            btnRestart.Text = "Restart";
            btnRestart.UseVisualStyleBackColor = false;
            btnRestart.Click += btnRestart_Click;
            // 
            // buttonPanel
            // 
            buttonPanel.Location = new Point(0, 0);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(200, 100);
            buttonPanel.TabIndex = 0;
            // 
            // welcomePanel
            // 
            welcomePanel.BackColor = Color.FromArgb(45, 45, 48);
            welcomePanel.BorderStyle = BorderStyle.FixedSingle;
            welcomePanel.Controls.Add(btnStartGame);
            welcomePanel.Controls.Add(lblWelcomeInstructions);
            welcomePanel.Controls.Add(lblWelcomeTitle);
            welcomePanel.Location = new Point(312, 250);
            welcomePanel.Name = "welcomePanel";
            welcomePanel.Size = new Size(400, 280);
            welcomePanel.TabIndex = 7;
            // 
            // lblWelcomeTitle
            // 
            lblWelcomeTitle.AutoSize = true;
            lblWelcomeTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblWelcomeTitle.ForeColor = Color.FromArgb(0, 204, 102);
            lblWelcomeTitle.Location = new Point(40, 30);
            lblWelcomeTitle.Name = "lblWelcomeTitle";
            lblWelcomeTitle.Size = new Size(326, 51);
            lblWelcomeTitle.TabIndex = 0;
            lblWelcomeTitle.Text = "Chicken Invaders";
            // 
            // lblWelcomeInstructions
            // 
            lblWelcomeInstructions.Font = new Font("Segoe UI", 11F);
            lblWelcomeInstructions.ForeColor = Color.White;
            lblWelcomeInstructions.Location = new Point(30, 100);
            lblWelcomeInstructions.Name = "lblWelcomeInstructions";
            lblWelcomeInstructions.Size = new Size(340, 90);
            lblWelcomeInstructions.TabIndex = 1;
            lblWelcomeInstructions.Text = "🎮 How to Play:\r\n\r\n← → Arrow Keys or A/D - Move Spaceship\r\nSPACE - Shoot\r\n\r\nPress START button to begin!";
            lblWelcomeInstructions.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.FromArgb(0, 204, 102);
            btnStartGame.FlatAppearance.BorderSize = 0;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnStartGame.ForeColor = Color.White;
            btnStartGame.Location = new Point(100, 210);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(200, 50);
            btnStartGame.TabIndex = 2;
            btnStartGame.Text = "START GAME";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 30);
            ClientSize = new Size(1024, 749);
            Controls.Add(welcomePanel);
            Controls.Add(lblInstructions);
            Controls.Add(lblGameOver);
            Controls.Add(gamePanel);
            Controls.Add(headerPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Chicken Invaders";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            Resize += Form1_Resize;
            gamePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).EndInit();
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            welcomePanel.ResumeLayout(false);
            welcomePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}

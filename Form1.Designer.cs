using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChickenInvaders
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Button btnViewRankings;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnPlayAgain;
        private System.Windows.Forms.Button btnBackToMenu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.btnViewRankings = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnPlayAgain = new System.Windows.Forms.Button();
            this.btnBackToMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 16;
            this.gameTimer.Tick += new System.EventHandler(this.GameLoop);
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = Color.FromArgb(30, 30, 50);
            this.txtUsername.BorderStyle = BorderStyle.FixedSingle;
            this.txtUsername.Font = new Font("Segoe UI", 16F);
            this.txtUsername.ForeColor = Color.White;
            this.txtUsername.Location = new Point(332, 355);
            this.txtUsername.MaxLength = 20;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(360, 36);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // btnStartGame
            // 
            this.btnStartGame.BackColor = Color.FromArgb(0, 200, 100);
            this.btnStartGame.Cursor = Cursors.Hand;
            this.btnStartGame.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 136);
            this.btnStartGame.FlatAppearance.BorderSize = 2;
            this.btnStartGame.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 150, 75);
            this.btnStartGame.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 230, 115);
            this.btnStartGame.FlatStyle = FlatStyle.Flat;
            this.btnStartGame.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnStartGame.ForeColor = Color.White;
            this.btnStartGame.Location = new Point(332, 510);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new Size(170, 45);
            this.btnStartGame.TabIndex = 1;
            this.btnStartGame.Text = "🚀 START GAME";
            this.btnStartGame.UseVisualStyleBackColor = false;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // btnViewRankings
            // 
            this.btnViewRankings.BackColor = Color.FromArgb(80, 80, 120);
            this.btnViewRankings.Cursor = Cursors.Hand;
            this.btnViewRankings.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 150);
            this.btnViewRankings.FlatAppearance.BorderSize = 2;
            this.btnViewRankings.FlatAppearance.MouseDownBackColor = Color.FromArgb(60, 60, 100);
            this.btnViewRankings.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 140);
            this.btnViewRankings.FlatStyle = FlatStyle.Flat;
            this.btnViewRankings.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnViewRankings.ForeColor = Color.White;
            this.btnViewRankings.Location = new Point(522, 510);
            this.btnViewRankings.Name = "btnViewRankings";
            this.btnViewRankings.Size = new Size(170, 45);
            this.btnViewRankings.TabIndex = 2;
            this.btnViewRankings.Text = "🏆 RANKINGS";
            this.btnViewRankings.UseVisualStyleBackColor = false;
            this.btnViewRankings.Click += new System.EventHandler(this.btnViewRankings_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = Color.FromArgb(255, 180, 50);
            this.btnPause.Cursor = Cursors.Hand;
            this.btnPause.FlatAppearance.BorderColor = Color.FromArgb(255, 200, 80);
            this.btnPause.FlatAppearance.BorderSize = 1;
            this.btnPause.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 140, 30);
            this.btnPause.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 200, 80);
            this.btnPause.FlatStyle = FlatStyle.Flat;
            this.btnPause.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnPause.ForeColor = Color.White;
            this.btnPause.Location = new Point(824, 15);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new Size(85, 35);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "⏸ PAUSE";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Visible = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.BackColor = Color.FromArgb(50, 150, 255);
            this.btnResume.Cursor = Cursors.Hand;
            this.btnResume.FlatAppearance.BorderColor = Color.FromArgb(80, 180, 255);
            this.btnResume.FlatAppearance.BorderSize = 1;
            this.btnResume.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 120, 200);
            this.btnResume.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 180, 255);
            this.btnResume.FlatStyle = FlatStyle.Flat;
            this.btnResume.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnResume.ForeColor = Color.White;
            this.btnResume.Location = new Point(824, 15);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new Size(85, 35);
            this.btnResume.TabIndex = 4;
            this.btnResume.Text = "▶ RESUME";
            this.btnResume.UseVisualStyleBackColor = false;
            this.btnResume.Visible = false;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = Color.FromArgb(220, 50, 80);
            this.btnRestart.Cursor = Cursors.Hand;
            this.btnRestart.FlatAppearance.BorderColor = Color.FromArgb(255, 80, 100);
            this.btnRestart.FlatAppearance.BorderSize = 1;
            this.btnRestart.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 30, 60);
            this.btnRestart.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 80, 100);
            this.btnRestart.FlatStyle = FlatStyle.Flat;
            this.btnRestart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnRestart.ForeColor = Color.White;
            this.btnRestart.Location = new Point(915, 15);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new Size(85, 35);
            this.btnRestart.TabIndex = 5;
            this.btnRestart.Text = "🔄 RESTART";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnPlayAgain
            // 
            this.btnPlayAgain.BackColor = Color.FromArgb(0, 200, 100);
            this.btnPlayAgain.Cursor = Cursors.Hand;
            this.btnPlayAgain.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 136);
            this.btnPlayAgain.FlatAppearance.BorderSize = 2;
            this.btnPlayAgain.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 150, 75);
            this.btnPlayAgain.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 230, 115);
            this.btnPlayAgain.FlatStyle = FlatStyle.Flat;
            this.btnPlayAgain.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnPlayAgain.ForeColor = Color.White;
            this.btnPlayAgain.Location = new Point(262, 545);
            this.btnPlayAgain.Name = "btnPlayAgain";
            this.btnPlayAgain.Size = new Size(200, 45);
            this.btnPlayAgain.TabIndex = 6;
            this.btnPlayAgain.Text = "🎮 PLAY AGAIN";
            this.btnPlayAgain.UseVisualStyleBackColor = false;
            this.btnPlayAgain.Visible = false;
            this.btnPlayAgain.Click += new System.EventHandler(this.btnPlayAgain_Click);
            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.BackColor = Color.FromArgb(80, 80, 120);
            this.btnBackToMenu.Cursor = Cursors.Hand;
            this.btnBackToMenu.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 150);
            this.btnBackToMenu.FlatAppearance.BorderSize = 2;
            this.btnBackToMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(60, 60, 100);
            this.btnBackToMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 140);
            this.btnBackToMenu.FlatStyle = FlatStyle.Flat;
            this.btnBackToMenu.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnBackToMenu.ForeColor = Color.White;
            this.btnBackToMenu.Location = new Point(562, 545);
            this.btnBackToMenu.Name = "btnBackToMenu";
            this.btnBackToMenu.Size = new Size(200, 45);
            this.btnBackToMenu.TabIndex = 7;
            this.btnBackToMenu.Text = "🏠 MAIN MENU";
            this.btnBackToMenu.UseVisualStyleBackColor = false;
            this.btnBackToMenu.Visible = false;
            this.btnBackToMenu.Click += new System.EventHandler(this.btnBackToMenu_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(10, 10, 25);
            this.ClientSize = new Size(1024, 768);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.btnViewRankings);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnPlayAgain);
            this.Controls.Add(this.btnBackToMenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "🐔 Chicken Invaders - Space Battle";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}

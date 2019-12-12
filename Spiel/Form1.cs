using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spiel
{
    public partial class Form1 : Form
    {
        bool goleft;
        bool goright;
        int speed = 8;
        int highscore = 0;
        int score = 0;
        int missed = 5;
        Random rndY = new Random();
        Random rndX = new Random();
        PictureBox splash = new PictureBox();
       
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            reset();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
        }

        private void gameTick(object sender, EventArgs e)
        {
            label1.Text = "Hgihscore: " + highscore;
            label2.Text = "Socre: " + score;
            label3.Text = "Leben: " + missed;

            if (goleft == true && chicken.Left > 0)
            {
                chicken.Left -= 12;
                chicken.Image = Properties.Resources.chicken_normal2;
            }

            if (goright == true && chicken.Left + chicken.Width < this.ClientSize.Width)
            {
                chicken.Left += 12;
                chicken.Image = Properties.Resources.chicken_normal;
            }

            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "Eggs")
                {
                    X.Top += speed;

                    if (X.Top + X.Height > this.ClientSize.Height)
                    {
                        splash.Image = Properties.Resources.splash;
                        splash.Location = X.Location;
                        splash.Height = 59;
                        splash.Width = 60;
                        splash.BackColor = System.Drawing.Color.Transparent;

                        this.Controls.Add(splash);

                        X.Top = rndY.Next(80, 300)*-1;
                        X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                        missed--;
                        label3.Text = "Leben: " + missed;
                        chicken.Image = Properties.Resources.chicken_hurt;
                    }

                    if (X.Bounds.IntersectsWith(chicken.Bounds))
                        {
                            X.Top = rndY.Next(80, 300) * -1;
                            X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                            score++;
                            label2.Text = "Socre: " + score;
                    }
                    
                    if (score >= 20)
                    {
                        speed = 16;
                    }

                    if (score > highscore)
                    {
                        highscore++;
                        label1.Text = "Hgihscore: " + highscore;
                    }

                    if (missed == 0) 
                    {
                        gameTimer.Stop();
                        MessageBox.Show("Game Over! Du hast zuviele Eier verloren" + "\r\n" + "Drücke OK für einene neuen Versuch");
                        reset();
                    }  
                }
            }
        }
        private void reset()
        {
            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "Eggs")
                {
                    X.Top = rndY.Next(80, 300) * -1;
                    X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                }
            }

            chicken.Left = this.ClientSize.Width / 2;
            chicken.Image = Properties.Resources.chicken_normal2;

            score = 0;
            missed = 5;
            speed = 8;

            goleft = false;
            goright = false;
            gameTimer.Start();
        }
    }
}


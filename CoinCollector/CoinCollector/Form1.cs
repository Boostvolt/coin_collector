using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoinCollector
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

            if (e.KeyCode == Keys.Escape)
            {
                if (gameTimer.Enabled)
                {
                    gameTimer.Enabled = false;
                    //label4.Text = "PAUSE";
                }
                else{
                    gameTimer.Enabled = true;
                    //label4.Text = "";
                }
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
            label1.Text = "Highscore: " + highscore;
            label2.Text = "Socre: " + score;
            label3.Text = "Leben: " + missed;

            if (goleft == true && figure.Left > 0)
            {
                figure.Left -= 12;
                figure.Image = Properties.Resources.figure_left;
            }

            if (goright == true && figure.Left + figure.Width < this.ClientSize.Width)
            {
                figure.Left += 12;
                figure.Image = Properties.Resources.figure_right;
            }

            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "Coins")
                {
                    X.Top += speed;

                    if (X.Top + X.Height > this.ClientSize.Height)
                    {
                        splash.Image = Properties.Resources.splash;
                        splash.Location = X.Location;
                        splash.Height = 75;
                        splash.Width = 75;
                        splash.BackColor = System.Drawing.Color.Transparent;

                        this.Controls.Add(splash);

                        X.Top = rndY.Next(80, 300) * -1;
                        X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                        missed--;
                        label3.Text = "Leben: " + missed;
                        figure.Image = Properties.Resources.figure_hurt;
                    }

                    if (X.Bounds.IntersectsWith(figure.Bounds))
                    {
                        X.Top = rndY.Next(80, 300) * -1;
                        X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                        score++;
                        label2.Text = "Socre: " + score;
                    }

                    //Level 1
                    if (score >= 20)
                    {
                        speed = 12;
                    }

                    //Level 2
                    if (score >= 40)
                    {
                        speed = 16;
                    }

                    //Level 3
                    if (score >= 60)
                    {
                        speed = 20;
                    }

                    //Level 4
                    if (score > highscore)
                    {
                        highscore++;
                        label1.Text = "Highscore: " + highscore;
                    }


                    if (missed == 0)
                    {
                        gameTimer.Stop();
                        MessageBox.Show("Game Over! Du hast zuviele Münzen verloren." + "\r\n" + "Drücke OK für einene neuen Versuch.");
                        reset();
                    }
                }
            }
        }
        private void reset()
        {
            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "Coins")
                {
                    X.Top = rndY.Next(80, 300) * -1;
                    X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                }
            }

            figure.Left = this.ClientSize.Width / 2;
            figure.Image = Properties.Resources.figure_left;

            score = 0;
            missed = 5;
            speed = 8;

            goleft = false;
            goright = false;
            gameTimer.Start();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
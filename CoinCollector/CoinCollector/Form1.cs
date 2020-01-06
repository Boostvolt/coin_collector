using System; 
using System.Windows.Forms;

namespace CoinCollector
{
    public partial class Form1 : Form
    {
        bool goleft; //check player can move left
        bool goright; //check player can move right
        int speed = 8; //default speed for the Coins dropping
        int highscore = 0; // default highscore value
        int score = 0; //default score value
        int life = 5; //default life value
        
        Random rndY = new Random(); //generate a random Y location
        Random rndX = new Random(); //genreate a random X location
        PictureBox splash = new PictureBox(); //create a new splash picture box

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            CenterToScreen();
            reset();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                //if left key is pressed goleft to true
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                //if right key is pressed goright to true
                goright = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                //if esc key is pressed pause game
                if (gameTimer.Enabled)
                {
                    gameTimer.Enabled = false;
                    label4.Text = "PAUSE";
                }
                else{
                    gameTimer.Enabled = true;
                    label4.Text = "";
                }
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                //if left key is up goleft to false
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                //if right key is up goright to false
                goright = false;
            }
        }
        private void gameTick(object sender, EventArgs e)
        {
            label1.Text = "Highscore: " + highscore;
            label2.Text = "Score: " + score;
            label3.Text = "Leben: " + life;

            if (goleft == true && figure.Left > 0)
            {
                //figure 12 pixel to the left
                figure.Left -= 12;
                //figure image change to the one moving left
                figure.Image = Properties.Resources.figure_left;
            }

            if (goright == true && figure.Left + figure.Width < this.ClientSize.Width)
            {
                //figure 12 pixel to the right
                figure.Left += 12;
                //figure image change to the one moving right
                figure.Image = Properties.Resources.figure_right;
            }

            //check the coins dropping from the top
            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "Coins")
                {
                    //move X towards the bottom
                    X.Top += speed;

                    if (X.Top + X.Height > this.ClientSize.Height)
                    {
                        System.Media.SoundPlayer hurt = new System.Media.SoundPlayer(@"c:\Users\jankott\OneDrive - Kantonsschule Frauenfeld\IMS\GitHub\coin_collector\CoinCollector\CoinCollector\Resources\sound_hurt.wav");
                        hurt.Play();

                        //if the coins hit the floor show splash image
                        splash.Image = Properties.Resources.splash;
                        splash.Location = X.Location;
                        splash.Height = 75;
                        splash.Width = 75;
                        splash.BackColor = System.Drawing.Color.Transparent;

                        this.Controls.Add(splash);

                        //position the coins random Y location
                        X.Top = rndY.Next(80, 300) * -1; 
                        //position the coins random X location
                        X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                        life--;                      

                        label3.Text = "Leben: " + life;
                        figure.Image = Properties.Resources.figure_hurt;
                    }
                    
                    //if figure collect coin
                    if (X.Bounds.IntersectsWith(figure.Bounds))
                    {
                        System.Media.SoundPlayer coin = new System.Media.SoundPlayer(@"c:\Users\jankott\OneDrive - Kantonsschule Frauenfeld\IMS\GitHub\coin_collector\CoinCollector\CoinCollector\Resources\sound_coin.wav");
                        coin.Play();

                        X.Top = rndY.Next(80, 300) * -1;
                        X.Left = rndX.Next(5, this.ClientSize.Width - X.Width);
                        score++;
                        label2.Text = "Score: " + score;
                    }

                    //level 1
                    if (score >= 20)
                    {
                        speed = 12;
                    }

                    //level 2
                    if (score >= 40)
                    {
                        speed = 16;
                    }

                    //level 3
                    if (score >= 60)
                    {
                        speed = 20;
                    }

                    //level 4
                    if (score > highscore)
                    {
                        highscore++;
                        label1.Text = "Highscore: " + highscore;
                    }

                    //game over
                    if (life == 0)
                    {
                        System.Media.SoundPlayer coin = new System.Media.SoundPlayer(@"c:\Users\jankott\OneDrive - Kantonsschule Frauenfeld\IMS\GitHub\coin_collector\CoinCollector\CoinCollector\Resources\sound_over.wav");
                        coin.Play();

                        gameTimer.Stop();
                        MessageBox.Show("Game Over! Du hast zuviele Münzen verloren." + "\r\n" + "Drücke OK für einen neuen Versuch.");
                        reset();
                    }
                }
            }
        }
        //reset everything
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
            life = 5;
            speed = 8;

            goleft = false;
            goright = false;
            gameTimer.Start();
        }
    }
}
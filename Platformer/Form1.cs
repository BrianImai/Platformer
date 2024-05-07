using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platformer
{
    public partial class Form1 : Form
    {

        bool moveLeft, moveRight, jump, restart, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;

        public Form1()
        {
            InitializeComponent();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            //Score Counter
            scoreCounter.Text = "Score: " + score;

            //Player Jump Speed
            player.Top += jumpSpeed;

            //Moving left
            if (moveLeft == true)
                player.Left -= playerSpeed;
            
            //Moving Right
            if (moveRight == true)
                player.Left += playerSpeed;
            
            //Stop jump if player not moving
            if (jump == true && force < 0)
                jump = false;

            //Setting Jump Speed and Force down
            if (jump == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            { 
                jumpSpeed = 10;
            }
             
            //Player goes Out of Bounds, Bottom/Left/Right
            if (player.Top > 720)
                RestartGame();
            if (player.Left < -10)
                RestartGame();
            if (player.Left > 1290)
                RestartGame();

            //Player can restart with keypress
            if (restart == true)
                RestartGame();
            
            //Collision with platform objects
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform" && player.Bounds.IntersectsWith(x.Bounds))
                    { 
                            force = 8;
                            player.Top = x.Top - player.Height;
                            x.BringToFront();
                    }

                    if ((string)x.Tag == "coin" && player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score++;
                    }                                       
                }
            }
        }
                
        //Key Detection
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                moveLeft = true;
            if (e.KeyCode == Keys.D)
                moveRight = true;
            if (e.KeyCode == Keys.Space && jump == false)
                jump = true;            
            if (e.KeyCode == Keys.Back)
                restart = true;
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                moveLeft = false;
            if (e.KeyCode == Keys.D)
                moveRight = false;
            if (jump == true)
                jump = false;
            if (e.KeyCode == Keys.Back)
                restart = false;
            if (e.KeyCode == Keys.Enter && isGameOver == true)
                RestartGame();
        }

        //Restart Game Function
        private void RestartGame()
        {
            jump = false;
            moveLeft = false;
            moveRight = false;
            isGameOver = false;
            score = 0;

            scoreCounter.Text = "Score: " + score;

            

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                    x.Visible = true;
            }

            //Reset position of player
            player.Left = 21;
            player.Top = 620;

            //restartBox.Visible = true;
            //Thread.Sleep(3000);
            //restartBox.Visible = false;

            //Restart Game
            gameTimer.Start();
        }
    }
}
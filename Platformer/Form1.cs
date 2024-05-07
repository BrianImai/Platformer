using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
            DrawingControl.SuspendDrawing(this);
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
            if (player.Top + player.Height > this.ClientSize.Height + 20)
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

            if (player.Bounds.IntersectsWith(objFlag.Bounds) && score == 25)
            {
                gameTimer.Stop();
                isGameOver = true;
                scoreCounter.Text = "Score: " + score + Environment.NewLine + "Congrats! You won!\nPress Enter to restart...";
            }
            else
            {
                scoreCounter.Text = "Score: " + score + Environment.NewLine + "Collect all the coins!";
            }

            DrawingControl.ResumeDrawing(this);
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
            
            //Restart Game
            gameTimer.Start();
        }

        //Stop Flickering Issue
        class DrawingControl
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

            private const int WM_SETREDRAW = 11;

            public static void SuspendDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }
        }

    }
}
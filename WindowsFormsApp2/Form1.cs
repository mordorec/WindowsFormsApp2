using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        int blocksX = 160;
        int blocksY = 85;
        int score = 0;
        int level = 3;
        int highestScore = 0;
        readonly List<PictureBox> pictureBoxes = new List<PictureBox>();
        List<PictureBox> chosenBoxes = new List<PictureBox>();
        Random rand = new Random();

        Color temp;

        int index = 0;
        int tries = 0;

        int timeLimit = 0;
        bool SelectingColours = false;

        string correctOrder = string.Empty;
        string playerOrder = string.Empty;

        public Form1()
        {
            InitializeComponent();
            SetUpBlocks();
            this.CenterToScreen();
            lblCurrentScore.Text = "Current Score: " + score.ToString();
            lblHighestScore.Text = "Highest Score: " + highestScore.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (SelectingColours)
            {
                timeLimit++;
                switch (timeLimit)
                {
                    case 10:
                        temp = chosenBoxes[index].BackColor;
                        chosenBoxes[index].BackColor = Color.White;
                        break;
                    case 20:
                        chosenBoxes[index].BackColor = temp;
                        break;
                    case 30:
                        chosenBoxes[index].BackColor = Color.White;
                        break;
                    case 40:
                        chosenBoxes[index].BackColor = temp;
                        break;
                    case 50:
                        if (index < chosenBoxes.Count - 1)
                        {
                            index++;
                            timeLimit = 0;
                        }
                        else
                        {
                            SelectingColours = false;
                        }
                        break;
                }
            }

            if (tries >= level)
            {
                if (correctOrder == playerOrder)
                {
                    tries = 0;
                    GameTimer.Stop();
                    MessageBox.Show("Well done, you got them correctly.", "Repeat it");
                    score++;
                    if (score > highestScore)
                    {
                        highestScore = score;
                    }
                }
                else
                {
                    tries = 0;
                    GameTimer.Stop();
                    MessageBox.Show("Your guesses did not match, try all over again.", "Repeat it");
                    level = 3;
                    ResetScore();
                }
                lblHighestScore.Text = "Highest Score: " + highestScore.ToString();
            }
            lblCurrentScore.Text = "Current Score: " + score.ToString();
            lblinfo.Text = "Click on " + level + " blocks in the same sequence.";
        }

        private void ButtonClickEvent(object sender, EventArgs e)
        {
            if (score > 0 && level < 16)
            {
                level++;
            }

            correctOrder = string.Empty;
            playerOrder = string.Empty;
            chosenBoxes.Clear();
            chosenBoxes = pictureBoxes.OrderBy(x => rand.Next()).Take(level).ToList();

            for (int i = 0; i < chosenBoxes.Count; i++)
            {
                correctOrder += chosenBoxes[i].Name + " ";
            }

            foreach (PictureBox x in pictureBoxes)
            {
                x.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }

            Debug.WriteLine(correctOrder);
            index = 0;
            timeLimit = 0;
            SelectingColours = true;
            GameTimer.Start();
        }
        private void ResetScore()
        {
            score = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void SetUpBlocks()
        {
            for (int i = 1; i < 17; i++)
            {
                PictureBox newPic = new PictureBox();
                newPic.Name = "pic_" + i;
                newPic.Height = 60;
                newPic.Width = 60;
                newPic.BackColor = Color.DarkGray;
                newPic.Left = blocksX;
                newPic.Top = blocksY;
                newPic.Click += ClickOnPictureBox;

                if (i == 4 || i == 8 || i == 12)
                {
                    blocksY += 65;
                    blocksX = 160;
                }
                else
                {
                    blocksX += 65;

                }
                this.Controls.Add(newPic);
                pictureBoxes.Add(newPic);
            }

        }

        private void ClickOnPictureBox(object sender, EventArgs e)
        {
            if (!SelectingColours && chosenBoxes.Count > 1)
            {
                PictureBox temp = sender as PictureBox;
                temp.BackColor = Color.Black;
                playerOrder += temp.Name + " ";
                Debug.WriteLine(playerOrder);
                tries++;
            }
            else
            {
                return;
            }
        }

        private void lblHighestScore_Click(object sender, EventArgs e)
        {

        }
    }
}

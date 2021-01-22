using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PA5_Draft
{
    public partial class MainForm : Form
    {
        private int Step;//speed of the snake moving 
        private readonly SnakeGame Game;
        private int NumberOfApples = 2000; //Number of apple for when the game start
        private SoundPlayer Applechew; //sounds for the game
        private SoundPlayer Wallhit;
        private SoundPlayer Snakehit;
        public MainForm()
        {
            InitializeComponent();
            //Beginapple = new ChooseNumAppletobegin();
            //Beginapple.ShowDialog();
            //NumberOfApples = Beginapple.SelectedNum;
            //Beginapple.Dispose();
            Step = 1;

                    Game = new SnakeGame(new System.Drawing.Point((Field.Width - 20) / 2, Field.Height / 2), 40, NumberOfApples, Field.Size);
                    Field.Image = new Bitmap(Field.Width, Field.Height);
                    Game.EatAndGrow += Game_EatAndGrow;
                    Game.HitWallAndLose += Game_HitWallAndLose;
                    Game.HitSnakeAndLose += Game_HitSnakeAndLose;
                    Applechew = new SoundPlayer(@"sounds\eatapple.wav");
                    Wallhit = new SoundPlayer(@"sounds\tap.wav");
                    Snakehit = new SoundPlayer(@"sounds\Ha.wav");
                
                
            
        }

        private void Game_HitWallAndLose()
        {
            Wallhit.Play();
            mainTimer.Stop();
            Field.Refresh();

          

        }
        private void Game_HitSnakeAndLose()
        {
            Snakehit.Play();
            mainTimer.Stop();
            Field.Refresh();
        }

        private void Game_EatAndGrow()
        {
            Applechew.Play();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            //if (Beginapple.ShowDialog() == DialogResult.OK)
            //{

            Game.Move(Step);
           Field.Invalidate();
           // }
        }

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            Random r = new Random();
            int one = r.Next(0, 255);
            int two = r.Next(0, 255);
            int three = r.Next(0, 255);
            Pen PenForObstacles = new Pen(Color.FromArgb(40,40,40),2);
            Pen PenForSnake = new Pen(Color.FromArgb(100, 100, 100), 2);
            Brush BackGroundBrush = new SolidBrush(Color.FromArgb(150, 250, 150));
            Brush AppleBrush = new SolidBrush(Color.FromArgb(one, two, three));



            using (Graphics g = Graphics.FromImage(Field.Image))
            {
                g.FillRectangle(BackGroundBrush,new Rectangle(0,0,Field.Width,Field.Height));
                foreach (Point Apple in Game.Apples)
                    g.FillEllipse(AppleBrush, new Rectangle(Apple.X - SnakeGame.AppleSize / 2, Apple.Y - SnakeGame.AppleSize / 2,
                        SnakeGame.AppleSize, SnakeGame.AppleSize));
                foreach (LineSeg Obstacle in Game.Obstacles)
                    g.DrawLine(PenForObstacles, new System.Drawing.Point(Obstacle.Start.X, Obstacle.Start.Y)
                        , new System.Drawing.Point(Obstacle.End.X, Obstacle.End.Y));
                foreach (LineSeg Body in Game.SnakeBody)
                    g.DrawLine(PenForSnake, new System.Drawing.Point((int)Body.Start.X, (int)Body.Start.Y)
                        , new System.Drawing.Point((int)Body.End.X, (int)Body.End.Y));
            }
        }



        private void Snakes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Game.Move(Step, Direction.UP);
                    break;
                case Keys.Down:
                    Game.Move(Step, Direction.DOWN);
                    break;
                case Keys.Left:
                    Game.Move(Step, Direction.LEFT);
                    break;
                case Keys.Right:
                    Game.Move(Step, Direction.RIGHT);
                    break;
            }
        }

        private void PauseResume(object sender, EventArgs e)
        {
            mainTimer.Enabled = !mainTimer.Enabled;
        }
    }
}

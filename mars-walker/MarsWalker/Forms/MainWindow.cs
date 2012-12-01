using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MarsRover.Classes.Objects;
using MarsRover.Classes.Objects.WalkerObjects;
using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;

namespace MarsRover.Forms
{
    public partial class MainWindow : Form
    {
        Walker MarsWalker;
        Camera Camera;

        Point mousePoint;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ViewPort.InitializeContexts();
            this.MarsWalker = new Walker();

            this.Camera = new Camera();
            this.Camera.Position.SetValues(0, -5, 5);
            this.Camera.Lock(MarsWalker);
            this.Camera.Angle.X = 0;
            this.Camera.Angle.Y = 90;

            DrawingService.Init(ViewPort.Width, ViewPort.Height);
            this.Redraw();
            this.ViewPort.SwapBuffers();
            this.FrameTimer.Start();
        }

        private void Redraw()
        {
            DrawingService.Clear();
            

            this.MarsWalker.Update();
            this.MarsWalker.Draw();

            this.Camera.Lock(MarsWalker);
            DrawingService.SetCamera(Camera);
            DrawingService.DrawLevel();

            DrawingService.Finish();
            this.ViewPort.SwapBuffers();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    {
                        this.MarsWalker.Drive(Walker.MoveDirection.Forward);
                        break;
                    }
                case Keys.S:
                    {
                        this.MarsWalker.Drive(Walker.MoveDirection.Backward);
                        break;
                    }
                case Keys.A:
                    {
                        this.MarsWalker.Turn(Walker.TurnDirection.CW);
                        break;
                    }
                case Keys.D:
                    {
                        this.MarsWalker.Turn(Walker.TurnDirection.CCW);
                        break;
                    }
                case Keys.Z:
                    {
                        this.MarsWalker.Angle.X -= 10;
                        break;
                    }
                case Keys.C:
                    {
                        this.MarsWalker.Angle.X += 10;
                        break;
                    }
                case Keys.X:
                    {
                        this.MarsWalker.Reset();
                        break;
                    }
                case Keys.K:
                    {
                        this.MarsWalker.SetAnimation(MarsWalker.Animations[Walker.Animation.StartScan]);
                        break;
                    }
                case Keys.P:
                    {
                        this.MarsWalker.SetAnimation(MarsWalker.Animations[Walker.Animation.StopScan]);
                        break;
                    }
                case Keys.Add:
                    {
                        ScaleCamera(0.1);
                        break;
                    }
                case Keys.Subtract:
                    {
                        ScaleCamera(-0.1);
                        break;
                    }
                case Keys.F:
                    {
                        if (MarsWalker.PresentState == Walker.States.Base)
                            this.MarsWalker.SetAnimation(MarsWalker.Animations[Walker.Animation.StartAnalysis]);
                        if (MarsWalker.PresentState == Walker.States.Analysis)
                            this.MarsWalker.SetAnimation(MarsWalker.Animations[Walker.Animation.StopAnalysis]);
                        break;
                    }
            }
        }
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            DrawingService.Resize(ViewPort.Width, ViewPort.Height);
            this.Redraw();
            this.ViewPort.SwapBuffers();
        }

        void MainWindow_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double enlarger = e.Delta > 0 ? 0.1 : -0.1;
            ScaleCamera(enlarger);
        }

        private void ScaleCamera(double enlarger)
        {
            Camera.Scale += enlarger;
        }


        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            Redraw();
        }

        private void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point pd = new Point(e.X - mousePoint.X, e.Y - mousePoint.Y);
                this.Camera.Angle.X += pd.Y;
                this.Camera.Angle.Y += pd.X;
            }
            mousePoint = new Point(e.X, e.Y);
        }
    }
}
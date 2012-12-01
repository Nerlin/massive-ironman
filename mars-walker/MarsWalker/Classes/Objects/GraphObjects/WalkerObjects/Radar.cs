using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;
using MarsRover.Interfaces;

using Tao;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace MarsRover.Classes.Objects.GraphObjects.WalkerObjects
{
    public class Radar: GraphObject, IAnimated
    {
        #region Fields & Properties
        public double Size { get; set; }
        public string SelectedAnimation
        {
            get { return selectedAnimation; }
            set { this.SetAnimation(value); }
        }
        /// <summary>
        /// Отвечает за активность радара.
        /// </summary>
        public bool Enabled { get; protected set; }

        protected double normalAngle;
        protected double direction;
        protected string frontTexture = TextureNames.Body;

        string selectedAnimation;

        List<string> animations;
        #endregion
        #region Constants
        protected const string ScanAnimation = "scan";
        protected const string BaseAnimation = "base";
        protected const string StopScaningAnimation = "stopScan";
        #endregion
        #region Flags
        protected bool turningRight;
        #endregion
        public Radar()
        {
            this.Size = 1;
            InitiailizeAnimations();
        }

        private void InitiailizeAnimations()
        {
            animations = new List<string>();
            animations.Add(BaseAnimation);
            animations.Add(ScanAnimation);
            animations.Add(StopScaningAnimation);
        }

        public void SetAnimation(string name)
        {
            if (animations.Contains(name))
            {
                selectedAnimation = name;
                PrepareAnimation(name);
            }
            else throw new Exception("Выбранная анимация \"" + name + "\" не поддерживается данным объектом.");
        }
        public void Reset()
        {
            selectedAnimation = BaseAnimation;
        }

        public void Update()
        {
            switch (selectedAnimation)
            {
                case BaseAnimation: { break; }
                case ScanAnimation: { PlayScan(); break; }
                case StopScaningAnimation: { PlayStopScan(); break; }
            }       
        }

        public void StartScan()
        {
            this.SetAnimation(ScanAnimation);
            this.Enabled = true;
        }
        public void StopScan()
        {
            this.SetAnimation(StopScaningAnimation);
        }

        public override void Draw()
        {
            Gl.glPushMatrix();
            {
                Normalize();
                Gl.glScaled(this.Size, this.Size, this.Size);
                DrawingService.PickTexture(frontTexture);
                DrawPanel(Side.Back);

                DrawingService.PickTexture(TextureNames.Body);
                DrawBody(Side.Front);
                DrawPanel(Side.Front);


                DrawBody(Side.Back);

                Gl.glPushMatrix();
                Gl.glRotated(90, 0, 0, 1);
                Gl.glTranslated(0.5, -0.5, 0);
                DrawPanel(Side.Front);
                Gl.glPopMatrix();

                Gl.glPushMatrix();
                Gl.glRotated(-90, 0, 0, 1);
                Gl.glTranslated(-0.5, -0.5, 0);
                DrawPanel(Side.Front);
                Gl.glPopMatrix();

                Gl.glPushMatrix();
                Gl.glRotated(90, 0, 1, 0);
                Gl.glTranslated(0.5, 0.5, 0.5);
                Gl.glColor3f(1, 0, 0);
                DrawingService.PickTexture(TextureNames.Metal);
                Glut.glutSolidCylinder(0.25, 0.25, 25, 25);
                Gl.glPopMatrix();
            }
            Gl.glPopMatrix();
        }
        private static void DrawPanel(Side side)
        {
            Gl.glPushMatrix();
            Point3d[] t = DrawingService.GetTextureCoordinates();

            if (side == Side.Back)
            {
                Gl.glRotated(180, 0, 1, 0);
                for (int i = 0; i <= t.Length - 1; i++)
                    t[i].Y = -t[i].Y;
            }

            Point3d[] p = new Point3d[4]
                {
                    new Point3d(-0.5, 0, 1),
                    new Point3d(-0.5, 1, 1),
                    new Point3d(-0.5, 1, -1),
                    new Point3d(-0.5, 0, -1)
                };

            
            Point3d n = new Point3d(-1, 0, 0);

            DrawingService.DrawPolygon(p, t, side, n);
            Gl.glPopMatrix();
        }
        private static void DrawBody(Side side)
        {
            Gl.glPushMatrix();
            if (side == Side.Back)
                Gl.glRotated(180, 0, 1, 0);

            Point3d[] p = new Point3d[4]
                {
                    new Point3d(-0.5, 0, 1),
                    new Point3d(0.5, 0, 1),
                    new Point3d(0.5, 1, 1),
                    new Point3d(-0.5, 1, 1)
                };

            Point3d[] t = DrawingService.GetTextureCoordinates();
            Point3d n = new Point3d(0, 0, 1);

            DrawingService.DrawPolygon(p, t, Side.Front, n);
            Gl.glPopMatrix();
        }

        protected virtual void PrepareAnimation(string name)
        {
            switch (name)
            {
                case BaseAnimation: { break; }
                case ScanAnimation: { normalAngle = this.Angle.Y; break; }
                case StopScaningAnimation: {
                    double diff = normalAngle - this.Angle.Y;
                    direction = diff / Math.Abs(diff); break;
                }
            }
        }

        protected virtual void PlayScan()
        {
            double enlarger = turningRight ? 1 : -1;
            this.Angle.Y += enlarger;
            bool flag = false;

            if (turningRight)
                flag = this.Angle.Y >= normalAngle + 45;
            else
                flag = this.Angle.Y <= normalAngle - 45;

            if (flag)
                turningRight = !turningRight;
        }
        protected virtual void PlayStopScan()
        {
            if (normalAngle != this.Angle.Y)
                this.Angle.Y += direction;
            else
            {
                this.SetAnimation(BaseAnimation);
                this.Enabled = false;
            }
        }
    }
}

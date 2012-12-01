using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao;
using Tao.OpenGl;
using Tao.FreeGlut;

using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;
using MarsRover.Classes.Objects.GraphObjects.WalkerObjects;
using MarsRover.Interfaces;



namespace MarsRover.Classes.Objects.WalkerObjects
{
    public class Walker : GraphObject, IMobile, IAnimated
    {
        #region Fields & Properties

        double bodyPosition;
        double speed;
        string selectedAnimation;
        int frame;

        /// <summary>
        /// Колеса марсохода.
        /// </summary>
        Wheel[] marsWheels = new Wheel[6];
        Wheel[] driveWheels = new Wheel[2];
        Radar mainRadar;
        Scaner scaner;

        public new Point3d Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }
        public new Angle3d Angle
        {
            get { return base.Angle; }
            set { base.Angle = value; }
        }
        /// <summary>
        /// Скорость марсохода.
        /// </summary>
        public double Speed
        {
            get { return speed; }
            set
            {
                if (Math.Abs(speed + value) <= Math.Abs(MaxSpeed))
                    speed = value;
            }
        }
        /// <summary>
        /// Максимальная скорость марсохода.
        /// </summary>
        public double MaxSpeed
        {
            get;
            set;
        }
        public string SelectedAnimation
        {
            get { return selectedAnimation; }
            set { this.SetAnimation(value); }
        }

        public enum States
        {
            Base,
            Analysis,
            Scaning,
            Busy,
        }
        public States PresentState
        {
            get
            {
                switch (SelectedAnimation)
                {
                    case BaseAnimation: return States.Base;
                    case ScaningAnimation: return States.Scaning;
                    case AnalysingAnimation: return States.Analysis;
                    default: return States.Busy;
                }
            }
        }

        #endregion
        #region Lists
        /// <summary>
        /// Ось колес марсохода.
        /// </summary>
        List<Fixture> fixtures;
        List<Fixture> fixturesWheels;
        List<Fixture> fixturesCorpse;
        List<Fixture> fixturesMain;
        List<Fixture> fixturesRadar;
        List<Fixture> fixturesCamera;
        List<string> animations;
        #endregion
        #region Constants

        public enum Animation
        {
            Init, Base, StartScan, StopScan, StartAnalysis, StopAnalysis
        }
        public Dictionary<Animation, string> Animations { get; protected set; }

        protected const string InitAnimation = "init";
        protected const string BaseAnimation = "base";
        protected const string StartScanAnimation = "radarUp";
        protected const string StopScanAnimation = "radarDown";
        protected const string ScaningAnimation = "scan";
        protected const string StartAnalysisAnimation = "cameraOn";
        protected const string StopAnalysisAnimation = "cameraOff";
        protected const string AnalysingAnimation = "analysing";

        #endregion
        #region Flags
        bool driveAvailable;
        bool scanAvailable;
        bool analysisAvailable;

        bool turnFlag;
        #endregion

        public Walker()
        {
            this.Position = new Point3d(0, 0, 0);
            Initialize();
        }
        public Walker(Point3d startPosition)
        {
            this.Position = startPosition;
            Initialize();
        }

        private void Initialize()
        {
            this.MaxSpeed = 1;

            InitializeLists();
            InitializeFixtures();
            InitializeAnimations();
        }

        private void InitializeFixtures()
        {
            for (int i = 0; i <= marsWheels.Length - 1; i++)
            {
                Fixture item = new Fixture();
                this.marsWheels[i] = new Wheel();
                double z = i <= 2 ? -1 : 1;

                item.Lock(marsWheels[i], new Point3d(0, 0, z * -0.5));
                fixturesWheels.Add(item);
            }

            driveWheels[0] = marsWheels[2];
            driveWheels[1] = marsWheels[5];

            for (int i = 0; i <= 5; i++)
            {
                Fixture item = new Fixture();
                fixturesCorpse.Add(item);
            }

            fixturesWheels[0].SetPosition(-5, -5, 3);
            fixturesWheels[1].SetPosition(0, -5, 3);
            fixturesWheels[2].SetPosition(6, -5, 3);

            fixturesWheels[3].SetPosition(-5, -5, -3);
            fixturesWheels[4].SetPosition(0, -5, -3);
            fixturesWheels[5].SetPosition(6, -5, -3);

            fixturesCorpse[0].SetPosition(-5, -3.8, 2.6);
            fixturesCorpse[1].SetPosition(0, -4, 2.6);
            fixturesCorpse[2].SetPosition(6, -3.8, 2.6);
            fixturesCorpse[3].SetPosition(-5, -3.8, -2.6);
            fixturesCorpse[4].SetPosition(0, -4, -2.6);
            fixturesCorpse[5].SetPosition(6, -3.8, -2.6);

            for (int i = 0; i <= 1; i++)
            {
                Fixture mainFixture = new Fixture();
                mainFixture.Size = 2;
                fixturesMain.Add(mainFixture);
            }

            fixturesMain[0].SetPosition(2, -3, 2);
            fixturesMain[1].SetPosition(2, -3, -2);

            Block block = new Block(fixturesCorpse[2], fixturesMain[0]);
            fixturesMain[0].Lock(block);

            block = new Block(fixturesCorpse[5], fixturesMain[1]);
            fixturesMain[1].Lock(block);

            for (int i = 0; i <= 1; i++)
            {
                double z = i == 0 ? -1 : 1;
                Fixture item = new Fixture();
                item.Size = 1.5;
                item.SetPosition(-2, -3.6, z * -2.6);
                fixturesCorpse.Add(item);
            }

            block = new Block(fixturesCorpse[6], fixturesMain[0]);
            fixturesMain[0].Lock(block);

            block = new Block(fixturesCorpse[7], fixturesMain[1]);
            fixturesMain[1].Lock(block);

            for (int i = 0; i <= 5; i++)
            {
                block = new Block(marsWheels[i], fixturesCorpse[i]);
                fixturesCorpse[i].Lock(block);
                //fixturesWheels[i].Lock(block);
            }

            for (int i = 0; i <= 1; i++)
            {
                Fixture fixt = fixturesCorpse[i + 6];
                Fixture fcon = fixturesCorpse[i * 3];
                block = new Block(fcon, fixt);
                fixt.Lock(block);

                fcon = fixturesCorpse[i * 3 + 1];
                block = new Block(fcon, fixt);
                fixt.Lock(block);
            }

            Fixture radarBasement = new Fixture();
            radarBasement.Size = 2;
            radarBasement.SetPosition(-1, -1.5, 0);
            fixturesRadar.Add(radarBasement);

            Fixture r2 = new Fixture();
            r2.SetPosition(-1, 3, 0);
            fixturesRadar.Add(r2);

            mainRadar = new Radar();
            r2.Lock(mainRadar);

            block = new Block(r2, radarBasement);
            radarBasement.Lock(block);

            Fixture cameraBasement = new Fixture();
            cameraBasement.Size = 1.5;
            cameraBasement.SetPosition(6.5, -2, 0);
            fixturesCamera.Add(cameraBasement);

            Fixture c2 = new Fixture();
            c2.SetPosition(8.5, 0, 0);
            fixturesCamera.Add(c2);

            Fixture c3 = new Fixture();
            c3.SetPosition(12.5, -1, 0);
            fixturesCamera.Add(c3);

            block = new Block(c2, cameraBasement);
            cameraBasement.Lock(block);

            block = new Block(c3, c2);
            c2.Lock(obj: block, type: PositionType.Absolute);

            scaner = new Scaner();
            scaner.Size = 0.75;
            c2.Lock(obj: c3, type: PositionType.Absolute);
            c3.Lock(scaner);     
            
            //cameraBasement.Lock(c2);
            //c2.Turn(new Point3d(0, 1, 0), 90);

            fixtures.AddRange(fixturesCamera);
            fixtures.AddRange(fixturesCorpse);
            fixtures.AddRange(fixturesMain);
            fixtures.AddRange(fixturesRadar);
            fixtures.AddRange(fixturesWheels);
        }
        private void InitializeLists()
        {
            this.Angle = new Angle3d();
            this.fixtures = new List<Fixture>();
            this.fixturesWheels = new List<Fixture>();
            this.fixturesCorpse = new List<Fixture>();
            this.fixturesMain = new List<Fixture>();
            this.fixturesRadar = new List<Fixture>();
            this.fixturesCamera = new List<Fixture>();
            this.animations = new List<string>();
            this.Animations = new Dictionary<Animation, string>();
        }
        private void InitializeAnimations()
        {
            animations.Add(InitAnimation);
            animations.Add(BaseAnimation);
            animations.Add(StartScanAnimation);
            animations.Add(StopScanAnimation);
            animations.Add(ScaningAnimation);
            animations.Add(StartAnalysisAnimation);
            animations.Add(StopAnalysisAnimation);
            animations.Add(AnalysingAnimation);

            this.SelectedAnimation = InitAnimation;

            this.Animations.Add(Animation.Init, InitAnimation);
            this.Animations.Add(Animation.Base, BaseAnimation);
            this.Animations.Add(Animation.StartScan, StartScanAnimation);
            this.Animations.Add(Animation.StopScan, StopScanAnimation);
            this.Animations.Add(Animation.StartAnalysis, StartAnalysisAnimation);
            this.Animations.Add(Animation.StopAnalysis, StopAnalysisAnimation);
        }

        public enum MoveDirection { Forward, Backward, SameSpeed };
        public void Drive(MoveDirection direction)
        {
            if (driveAvailable)
            {
                double enlarger = direction == MoveDirection.Forward ? MaxSpeed / 4 : direction == MoveDirection.Backward ? -MaxSpeed / 4 : 0;
                this.Speed -= enlarger;

                double addAngle = driveWheels[0].Angle.Y;
                if (addAngle < -1 || addAngle > 1)
                {
                    double enlargerAngle = addAngle / Math.Abs(addAngle);
                    this.Angle.Y += enlarger * Math.Sin(Angle3d.DegreeToRadian(driveWheels[0].Angle.Y)) * 3;
                }
            }
        }

        public enum TurnDirection { CW, CCW };
        public void Turn(TurnDirection turn)
        {
            if (driveAvailable)
            {
                int enlarger = turn == TurnDirection.CW ? 1 : -1;
                for (int i = 0; i <= 1; i++)
                {
                    if (Math.Abs(driveWheels[i].Angle.Y + enlarger) <= 30)
                        driveWheels[i].Angle.Y += enlarger;
                }
                if (speed != 0) 
                    this.Angle.Y += enlarger;

                turnFlag = true;
                frame = 0;
            }
        }

        /// <summary>
        /// Передвигает объект с заданной скоростью.
        /// </summary>
        public void Move()
        {
            Angle3d angle = new Angle3d(this.Angle.X, this.Angle.Y, this.Angle.Z);
            Point3d vector = DataService.Rotate(new Point3d(0, 0, this.Speed), angle);
            this.Position += vector;
        }
        public override void Move(Point3d point)
        {
            base.Move(point);
        }

        public override void Draw()
        {
            //DrawingService.GenerateTexture();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            {
                Gl.glTranslated(this.Position.X, this.Position.Y, this.Position.Z);
                Gl.glRotated(this.Angle.X, 1, 0, 0);
                Gl.glRotated(this.Angle.Y, 0, 1, 0);
                Gl.glRotated(this.Angle.Z, 0, 0, 1);
                
                #region Колеса
                Gl.glPushMatrix();
                {
                    Gl.glRotated(90, 0, 1, 0);
                    Gl.glScaled(0.5, 0.5, 0.5);
                    DrawFixtures();
                }
                Gl.glPopMatrix();
                #endregion

                Gl.glPushMatrix();
                Gl.glRotated(90, 0, 1, 0);
                Gl.glTranslated(3, -1, 1);
                Gl.glScaled(1, 0.5, 0.5);
                DrawingService.PickTexture(TextureNames.Side);
                DrawBody(Side.Front);
                Gl.glPopMatrix();

                Gl.glPushMatrix();
                Gl.glRotated(-90, 0, 1, 0);
                Gl.glTranslated(-3, -1, 1);
                Gl.glScaled(1, 0.5, 0.5);
                DrawingService.PickTexture(TextureNames.Side);
                DrawBody(Side.Back);
                Gl.glPopMatrix();

                /// Нижняя наклонная панель
                Gl.glPushMatrix();
                Gl.glRotated(90, 0, 1, 0);
                Gl.glTranslated(3, -1, 0);
                Gl.glScaled(1, 0.5, 0.5);

                //DrawInclinedPlane();
                DrawingService.PickTexture(TextureNames.Body);
                DrawRoof();
                DrawFrontRoof();
                DrawRoof(Side.Front);
                DrawingService.PickTexture(TextureNames.Back);
                DrawBackPanel();
                DrawingService.PickTexture(TextureNames.Body);
                DrawFrontPanel();
               
                DrawRoofRim();
                DrawFloor();
                Gl.glPopMatrix();

            }
            Gl.glPopMatrix();
        }

        private void DrawFixtures()
        {
            fixtures.ForEach(item => item.Draw());
        }
        private void DrawWheelAxis(int presentAxis)
        {
            Gl.glPushMatrix();
            this.marsWheels[presentAxis * 3 - 3].Draw();
            Gl.glTranslated(3, 0, 0);
            this.marsWheels[presentAxis * 3 - 2].Draw();
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            Gl.glTranslated(-3, 0, 0);
            this.marsWheels[presentAxis * 3 - 1].Draw();
            Gl.glPopMatrix();
        }
        private void DrawBody(Side side)
        {
            Gl.glPushMatrix();
            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();

            if (side == Side.Back)
            {
                Gl.glTranslated(5, 0, 0);
                for (int i = 0; i <= txtCrds.Length - 1; i++)
                    txtCrds[i].X = -txtCrds[i].X;
            }

            Point3d[] points = new Point3d[4] {
                new Point3d(0, 0.5, 0),
                new Point3d(-5, 0.5, 0),
                new Point3d(-5, -2, 0),
                new Point3d(0, -2, 0)
            };


            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(0, 0, 1));
            Gl.glPopMatrix();
        }
        private void DrawRoof(Side side)
        {
            Point3d[] points = new Point3d[3] {
                new Point3d(-5, 0.5, 2),
                new Point3d(-4, 0.5, 2),
                new Point3d(-5, 2, 2)
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            Point3d normal = new Point3d(0, 0, 1);
            if (side == Side.Back)
                normal = new Point3d(0, 0, -1);

            DrawingService.DrawPolygon(points, txtCrds, Side.Front, normal);

            Gl.glPushMatrix();
            points = new Point3d[3] {
                new Point3d(5, 0.5, 2),
                new Point3d(5, 2, 2),
                new Point3d(4, 0.5, 2)
                
            };
            Gl.glRotated(180, 0, 1, 0);
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(0, 0, 1));
            Gl.glPopMatrix();

            points = new Point3d[4] {
                new Point3d(-5, 0.5, 2),
                new Point3d(-5, 2, 2),
                new Point3d(-5, 2, -2),
                new Point3d(-5, 0.5, -2)
            };
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(-1, 0, 0));

            points = new Point3d[4] {
                new Point3d(-4, 0.5, 2),
                new Point3d(-4, 0.5, -2),
                new Point3d(-5, 2, -2),
                new Point3d(-5, 2, 2)
            };

            if (side == Side.Back)
                normal = new Point3d(0, -1, 0);
            else
                normal = new Point3d(1, 1, 0);
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, normal);
        }

        private void DrawFloor()
        {
            Gl.glPushMatrix();
            Gl.glRotated(180, 0, 1, 0);
            Point3d[] points = new Point3d[4] {
                new Point3d(5, -2, 2),
                new Point3d(0, -2, 2),
                new Point3d(0, -2, -2),
                new Point3d(5, -2, -2),
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(0, -1, 0));
            Gl.glPopMatrix();
        }    
        private void DrawFrontPanel()
        {
            Point3d[] points = new Point3d[4] {
                new Point3d(0, 0.5, 2),
                new Point3d(0, -2, 2),
                new Point3d(0, -2, -2),
                new Point3d(0, 0.5, -2),
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(1, 0, 0));
        }
        private void DrawBackPanel()
        {
            Point3d[] points = new Point3d[4] {
                new Point3d(-5, -2, 2),
                new Point3d(-5, 0.5, 2),
                new Point3d(-5, 0.5, -2),
                new Point3d(-5, -2, -2),
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(-1, 0, 0));
        }
        private void DrawRoof()
        {
            Point3d[] points = new Point3d[4] {
                new Point3d(-4, 0.5, 2),
                new Point3d(0, 0.5, 2),
                new Point3d(0, 0.5, -2),
                new Point3d(-4, 0.5, -2),
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(0, 1, 0));
        }
        private void DrawFrontRoof()
        {
            Gl.glPushMatrix();
            Gl.glScaled(0.3, 1, 1);
            Gl.glTranslated(-0.5, -4.5, 0);
            Gl.glRotated(-90, 0, 0, 1);
           

            DrawRoof(Side.Front);

            Point3d[] points = new Point3d[4] {
                new Point3d(-5, 0.5, 2),
                new Point3d(-4, 0.5, 2),
                new Point3d(-4, 0.5, -2),
                new Point3d(-5, 0.5, -2),
            };

            Point3d[] txtCrds = DrawingService.GetTextureCoordinates();
            //DrawingService.DrawPolygon(points, txtCrds, Side.Front, new Point3d(1, 0, 0));

            //DrawingService.PickTexture(TextureNames.BackGround);

            Gl.glPopMatrix();
        }
        private void DrawRoofRim()
        {
            Gl.glPushMatrix();
            Gl.glTranslated(-5, 2, 0);
            Gl.glRotated(90, 0, 1, 0);
            Gl.glRotated(60, 1, 0, 0);
            //Gl.glRotated(45, 1, 0, 0);
            Gl.glScaled(1, 0.8, 1);
            //Glut.glutSolidCylinder(2, 2, 32, 32);
            Gl.glPopMatrix();
        }

        public void SetAnimation(string name)
        {
            if (animations.Contains(name))
            {
                if (ValidateAnimation(name))
                {
                    selectedAnimation = name;
                    PrepareAnimation(name);
                }
            }
            else throw new Exception("Выбранная анимация \"" + name + "\" не поддерживается данным объектом.");
        }
        public void Reset()
        {
            this.Position.SetValues(0, 1, 0);
            this.SelectedAnimation = BaseAnimation;
        }

        public void Update()
        {
            switch (selectedAnimation)
            {
                case InitAnimation: { PlayInitAnimation(); break; }
                case BaseAnimation: { PlayBaseAnimation(); break; }
                case StartScanAnimation: { PlayStartScanAnimation(); break; }
                case StopScanAnimation: { PlayStopScanAnimation(); break; }
                case ScaningAnimation: { mainRadar.Update(); break; }
                case StartAnalysisAnimation: { PlayStartAnalysisAnimation(); break; }
                case StopAnalysisAnimation: { PlayStopAnalysisAnimation(); break; }
                case AnalysingAnimation: { scaner.Update(); break; }
            }
        }

        private bool ValidateAnimation(string name)
        {
            switch (name)
            {
                case BaseAnimation: { return true; }
                case InitAnimation: { return true; }
                case StartScanAnimation: { return selectedAnimation == BaseAnimation || selectedAnimation == InitAnimation; }
                case StopScanAnimation: { return selectedAnimation == ScaningAnimation; }
                case StartAnalysisAnimation: { return selectedAnimation == BaseAnimation || selectedAnimation == InitAnimation; }
                case StopAnalysisAnimation: { return selectedAnimation == AnalysingAnimation; }
                case ScaningAnimation: { return selectedAnimation == StartScanAnimation; }
                case AnalysingAnimation: { return selectedAnimation == StartAnalysisAnimation; }
                default: return false;                 
            }
        }
        private void PrepareAnimation(string name)
        {
            switch (selectedAnimation)
            {
                case InitAnimation: {
                    driveAvailable = false;                    
                    scanAvailable = false;

                    PrepareWheelsDown();
                    PrepareInitAnimation(); break;
                }
                case BaseAnimation: { break; }
                case StartScanAnimation: {
                    PrepareWheelsUp(); break;
                }
                case StopScanAnimation: {
                    PrepareWheelsDown();
                    mainRadar.StopScan(); break;
                }
                case ScaningAnimation: { break; }
                case StartAnalysisAnimation: {
                    PrepareWheelsUp(); break;
                }
                case StopAnalysisAnimation: {
                    PrepareWheelsDown();
                    scaner.StopScan(); break;
                }
                case AnalysingAnimation: { break; }
            }
        }

        private void PrepareWheelsDown()
        {
            bodyPosition = this.Position.Y + 1;
        }
        private void PrepareWheelsUp()
        {
            driveAvailable = false;
            bodyPosition = this.Position.Y - 1;
        }

        private void PrepareInitAnimation()
        {
            fixturesRadar[1].SetPosition(fixturesRadar[1].Position - new Point3d(0, 2.8, 0));
            fixturesCamera[1].Turn(new Point3d(0, 0, 1), -15, PositionType.Relative);

            PrepareWheels(new Point3d(0, 1.5, 0));
            this.Speed = 0;
        }
        private void PrepareWheels(Point3d start)
        {
            foreach (var w in fixturesWheels)
                w.SetPosition(w.Position + start);
        }

        private void PlayBaseAnimation()
        {
            UpdatePosition();
        }
        private void PlayInitAnimation()
        {
            PlayWheelsDown();
            if (driveAvailable)
                this.SelectedAnimation = BaseAnimation;
        }
        private void PlayWheelsUp()
        {   
            if (fixturesWheels[0].Position.Y < -3.5)
            {
                Point3d enlarger = new Point3d(0, 0.02, 0);
                foreach (var w in fixturesWheels)
                    w.SetPosition(w.Position + enlarger);

                if (this.Position.Y > bodyPosition)
                    this.Position.Y -= enlarger.Y / 1.5;
            }
        }
        private void PlayWheelsDown()
        {
            bool flag = this.Position.Y < bodyPosition;
            Point3d enlarger = new Point3d(0, -0.02, 0);
            foreach (var w in fixturesWheels)
                w.SetPosition(w.Position + enlarger);

            if (flag) 
                this.Position.Y -= enlarger.Y / 1.5;

            if (fixturesWheels[0].Position.Y <= -5 && flag)
                driveAvailable = true;
        }
        private void PlayRadarUp()
        {
            if (!scanAvailable)               
                if (fixturesRadar[1].Position.Y <= 3)
                    fixturesRadar[1].SetPosition(fixturesRadar[1].Position + new Point3d(0, 0.02, 0));
                else
                    scanAvailable = true;
        }
        private void PlayRadarDown()
        {
            if (fixturesRadar[1].Position.Y > 0.2)
                fixturesRadar[1].SetPosition(fixturesRadar[1].Position - new Point3d(0, 0.02, 0));
            else 
                scanAvailable = false;
        }
        private void PlayScanerUp()
        {
            if (!analysisAvailable)
            {
                Fixture cameraFixture = fixturesCamera[1];
                if (cameraFixture.Angle.Z != 0)
                    cameraFixture.Turn(new Point3d(0, 0, 1), 1, PositionType.Relative);
                else
                    analysisAvailable = true;
            }
        }
        private void PlayScanerDown()
        {
            if (analysisAvailable)
            {
                Fixture cameraFixture = fixturesCamera[1];
                if (cameraFixture.Angle.Z != -15)
                    cameraFixture.Turn(new Point3d(0, 0, 1), -1, PositionType.Relative);
                else
                    analysisAvailable = false;
            }
        }

        private void PlayStartScanAnimation()
        {
            PlayWheelsUp();
            PlayRadarUp();

            if (scanAvailable)
            {
                mainRadar.StartScan();
                selectedAnimation = ScaningAnimation;
            }
        }
        private void PlayStopScanAnimation()
        {
            if (mainRadar.Enabled)
                mainRadar.Update();
            else
            {
                PlayRadarDown();
                if (!driveAvailable)
                    PlayWheelsDown();

                if (!scanAvailable)
                    selectedAnimation = BaseAnimation;
            }
        }

        private void UpdatePosition()
        {
            for (int i = 0; i <= 5; i++)
                marsWheels[i].Angle.Z += this.Speed * 30;

            if (this.Speed != 0)
                Move();

            ResetWheels();
        }
        private void ResetWheels()
        {
            Angle3d angle = driveWheels[0].Angle;
            if (!turnFlag)
            {                
                if (angle.Y < -1 || angle.Y > 1)
                {
                    double angleEnlarger = angle.Y / Math.Abs(angle.Y);
                    for (int i = 0; i <= driveWheels.Length - 1; i++)
                        driveWheels[i].Angle.Y -= angleEnlarger * 3;
                }
            }
            else
            {
                if (frame++ == 5)
                {
                    frame = 0;
                    turnFlag = false;
                }
            }
        }

        private void PlayStartAnalysisAnimation()
        {
            PlayWheelsUp();
            PlayScanerUp();

            if (analysisAvailable)
            {
                scaner.StartScan();
                selectedAnimation = AnalysingAnimation;
            }
        }
        private void PlayStopAnalysisAnimation()
        {
            if (scaner.Enabled)
                scaner.Update();
            else
            {
                PlayScanerDown();

                if (!driveAvailable)
                    PlayWheelsDown();

                if (driveAvailable && !analysisAvailable)
                    selectedAnimation = BaseAnimation;
            }
        }
    }
}

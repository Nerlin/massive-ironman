using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao;
using Tao.FreeGlut;
using Tao.OpenGl;

using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;

namespace MarsRover.Classes.Objects.WalkerObjects
{
    public class Fixture : GraphObject
    {
        /// <summary>
        /// Закрепленные объекты.
        /// </summary>
        public List<GraphObject> FixtureObjects { get; private set; }

        public new Angle3d Angle {
            get { return base.Angle; }
            set
            {
                this.Turn(new Point3d(1, 0, 0), value.X);
                this.Turn(new Point3d(0, 1, 0), value.Y);
                this.Turn(new Point3d(0, 0, 1), value.Z);
            }
        }
        public new Point3d Position
        {
            get { return base.Position; }
            set { this.SetPosition(value); }
        }
       
        public double Size { get; set; }

        public Fixture(): base()
        {
            this.FixtureObjects = new List<GraphObject>();
            this.Size = 1;
        }
        public override void Draw()
        {
            Gl.glPushMatrix();
            Gl.glTranslated(this.Position.X, this.Position.Y, this.Position.Z);         
            Gl.glRotated(this.Angle.X, 1, 0, 0);
            Gl.glRotated(this.Angle.Y, 0, 1, 0);
            Gl.glRotated(this.Angle.Z, 0, 0, 1);

            DrawingService.PickTexture(TextureNames.Metal);
            DrawingService.GenerateTexture();
            Glut.glutSolidSphere(0.25 * this.Size, 48, 48);
            Gl.glPopMatrix();
            DrawingService.DeactivateGeneration();

            foreach (var obj in this.FixtureObjects)
            {
                //obj.Position += this.Position;
                //Point3d n = new Point3d(0, 0, 0);
                //bool flag = obj.Position.Equals(n);
                //if (flag)
                //    obj.Position += this.Position;

                obj.Draw();

                //if (flag)
                //    obj.Position -= this.Position;
                //obj.Position -= this.Position;
            }
        }
        public override void Turn(Point3d axis, double angle, PositionType type = PositionType.Absolute)
        {
            base.Turn(axis, angle, type);
            Angle3d angle3d = new Angle3d(axis.X * angle, axis.Y * angle, axis.Z * angle);
            foreach (var obj in this.FixtureObjects)
            {
                Point3d result = DataService.Rotate(obj.Position, angle3d);
                if (obj is Fixture)
                    ((Fixture)obj).SetPosition(result);
                else
                    obj.Position = result;
            }
        }
        public void SetPosition(params double[] values)
        {
            Point3d value = new Point3d(values);
            foreach (var obj in this.FixtureObjects)
            {
                Point3d offset = this.Position - obj.Position;
                obj.Position = new Point3d(value - offset);
            }
            this.Position.SetValues(values);            
        }
        public void SetPosition(Point3d value)
        {
            foreach (var obj in this.FixtureObjects)
            {
                Point3d offset = this.Position - obj.Position;
                obj.Position = new Point3d(value - offset);
            }

            base.Position = value;
        }
        /// <summary>
        /// Закрепляет новый объект на крепеже.
        /// </summary>
        /// <param name="obj">Закрепляемый объект.</param>
        /// <param name="lockPoint">Точка крепления.</param>
        public void Lock(GraphObject obj, Point3d lockPoint = null, PositionType type = PositionType.Relative)
        {
            if (lockPoint == null)
                lockPoint = obj.Position;

            if (type == PositionType.Relative)
                obj.Position = this.Position + lockPoint;
            
            this.FixtureObjects.Add(obj);
        }
        /// <summary>
        /// Открепляет объект от крепежа.
        /// </summary>
        /// <param name="obj"></param>
        public void Unlock(GraphObject obj)
        {
            this.FixtureObjects.Remove(obj);
        }
        
    }
}

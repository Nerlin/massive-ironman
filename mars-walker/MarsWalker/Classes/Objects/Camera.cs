using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarsRover.Classes.Core.Data;

namespace MarsRover.Classes.Objects
{
    public class Camera : GraphObject
    {      
        /// <summary>
        /// Дистанция до наблюдаемого объекта.
        /// </summary>
        public Point3d Distance
        {
            get;
            set;
        }
        /// <summary>
        /// FOV.
        /// </summary>
        public double Scale
        {
            get;
            set;
        }
        /// <summary>
        /// Наблюдаемый объект.
        /// </summary>
        public GraphObject Target
        {
            get;
            set;
        }

        public Camera()
        {
            //this.Distance = new Point3d(1, 1, 3);
            this.Target = null;
            this.Angle = new Angle3d();

            this.Scale = 1;
        }
        /// <summary>
        /// Закрепляет камеру за объектом.
        /// </summary>
        /// <param name="obj"></param>
        public void Lock(GraphObject obj)
        {
            //this.Target = obj;
            this.Position.SetValues(-obj.Position.X, -obj.Position.Y, -obj.Position.Z);
            //this.Angle = new Angle3d(obj.Angle.X, -obj.Angle.Y, obj.Angle.Z);
        }
        /// <summary>
        /// Открепляет камеру от объекта.
        /// </summary>
        public void Unlock()
        {
            this.Target = null;
            this.Angle = new Angle3d();
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}

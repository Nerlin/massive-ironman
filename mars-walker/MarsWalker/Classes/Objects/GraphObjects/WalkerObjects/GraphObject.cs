using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao;
using Tao.OpenGl;

using MarsRover.Interfaces;
using MarsRover.Classes.Core.Data;

namespace MarsRover.Classes.Objects
{
    /// <summary>
    /// Графический объект модели.
    /// </summary>
    public abstract class GraphObject 
    {
        /// <summary>
        /// Позиция объекта.
        /// </summary>
        public Point3d Position
        {
            get;
            set;
        }
        /// <summary>
        /// Угол поворота объекта.
        /// </summary>
        public Angle3d Angle
        {
            get;
            set;
        }

        public GraphObject()
        {
            this.Position = new Point3d();
            this.Angle = new Angle3d();
        }
        /// <summary>
        /// Рисует графический объект.
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Осуществляет поворот объекта вокруг оси на заданный угол.
        /// </summary>
        /// <param name="axis">Оси, вокруг которых осуществляется поворот.</param>
        /// <param name="angle">Угол, на который нужно повернуть объект.</param>
        /// <param name="type">Тип задания угла: абсолютно (перекрытие значений) или относительно (сложение значений).</param>
        public virtual void Turn(Point3d axis, Double angle, PositionType type = PositionType.Absolute)
        {
            Angle3d relative = new Angle3d();
            if (type == PositionType.Relative)
                relative = new Angle3d(this.Angle);

            if (axis.X > 0) this.Angle.X = angle + relative.X; 
            if (axis.Y > 0) this.Angle.Y = angle + relative.Y;
            if (axis.Z > 0) this.Angle.Z = angle + relative.Z; 
        }
        /// <summary>
        /// Передвигает объект в новую точку.
        /// </summary>
        /// <param name="point"></param>
        public virtual void Move(Point3d point)
        {
            this.Position = new Point3d(point.X, point.Y, point.Z);
        }
        /// <summary>
        /// Нормализация координат объекта.
        /// </summary>
        protected void Normalize()
        {
            Gl.glTranslated(this.Position.X, this.Position.Y, this.Position.Z);
            Gl.glRotated(this.Angle.X, 1, 0, 0);
            Gl.glRotated(this.Angle.Y, 0, 1, 0);
            Gl.glRotated(this.Angle.Z, 0, 0, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Classes.Core.Data
{
    public class Rectangle
    {
        public Point3d LeftTop { get; set; }
        public Point3d RightTop { get; set; }
        public Point3d LeftBottom { get; set; }
        public Point3d RightBottom { get; set; }

        public Point3d this[int i]
        {
            get {
                switch (i)
                {
                    case 0: return LeftTop;
                    case 1: return LeftBottom;
                    case 2: return RightBottom;
                    case 3: return RightTop;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i)
                {
                    case 0: { LeftTop = value; break; }
                    case 1: { LeftBottom = value; break; }
                    case 2: { RightBottom = value; break; }
                    case 3: { RightTop = value; break; }
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
        public Rectangle() { }
        public Rectangle(params Point3d[] values)
        {
            if (values.Length != 4)
                throw new Exception("Приняты неверные значения для прямоугольника.");

            for (int i = 0; i <= 3; i++)
                this[i] = values[i];
        }

        public Point3d[] GetValues()
        {
            Point3d[] result = new Point3d[4];
            for (int i = 0; i <= 3; i++)
                result[i] = this[i];
            return result;
        }
    }
}

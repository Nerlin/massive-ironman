using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Classes.Core.Data
{
    public class Point3d
    {
        public double X
        {
            get;
            set;
        }
        public double Y
        {
            get;
            set;
        }
        public double Z
        {
            get;
            set;
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0: { X = value; break; }
                    case 1: { Y = value; break; }
                    case 2: { Z = value; break; }
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Point3d()
        {

        }
        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Point3d(params double[] values)
        {
            this.SetValues(values);
        }
        public Point3d(Point3d basement)
        {
            this.X = basement.X;
            this.Y = basement.Y;
            this.Z = basement.Z;
        }

        public static Point3d operator +(Point3d point1, Point3d point2)
        {
            Point3d result = new Point3d();
            result.X = point1.X + point2.X;
            result.Y = point1.Y + point2.Y;
            result.Z = point1.Z + point2.Z;

            return result;
        }
        public static Point3d operator -(Point3d point1, Point3d point2)
        {
            Point3d result = new Point3d();
            result.X = point1.X - point2.X;
            result.Y = point1.Y - point2.Y;
            result.Z = point1.Z - point2.Z;

            return result;
        }
        public static Point3d operator *(Point3d point1, Point3d point2)
        {
            Point3d result = new Point3d();
            result.X = point1.X * point2.X;
            result.Y = point1.Y * point2.Y;
            result.Z = point1.Z * point2.Z;

            return result;
        }
        public static Point3d operator /(Point3d point1, Point3d point2)
        {
            Point3d result = new Point3d();
            result.X = point1.X / point2.X;
            result.Y = point1.Y / point2.Y;
            result.Z = point1.Z / point2.Z;

            return result;
        }

        public static Point3d operator -(Point3d point)
        {
            return new Point3d(-point.X, -point.Y, -point.Z);
        }

        public static bool Equals(Point3d point1, Point3d point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y && point1.Z == point2.Z;
        }

        public virtual void SetValues(params double[] values)
        {
            if (values.Length > 3)
                throw new IndexOutOfRangeException();
            else
                for (int i = 0; i <= 2; i++)
                    this[i] = values[i];
        }
        public override string ToString()
        {
            return string.Format("(X: {0} | Y: {1} | Z: {2})", X, Y, Z);
        }
    }
}

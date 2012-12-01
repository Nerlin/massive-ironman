using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Classes.Core.Data
{
    public class Angle3d
    {
        double x;
        public double X
        {
            get { return x; }
            set { x = value % 360; }
        }

        double y;
        public double Y
        {
            get { return y; }
            set { y = value % 360; }
        }

        double z;
        public double Z
        {
            get { return z; }
            set { z = value % 360; }
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static double RadianToDegree(double angle)
        {
            return (180 / Math.PI) * angle;
        }

        public static Angle3d operator +(Angle3d a1, Angle3d a2)
        {
            Angle3d angle = new Angle3d();
            angle.X = a1.X + a2.X;
            angle.Y = a1.Y + a2.Y;
            angle.Z = a1.Z + a2.Z;

            return angle;
        }

        public Angle3d() { }
        public Angle3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Angle3d(Angle3d basement)
        {
            this.X = basement.X;
            this.Y = basement.Y;
            this.Z = basement.Z;
        }
    }

}

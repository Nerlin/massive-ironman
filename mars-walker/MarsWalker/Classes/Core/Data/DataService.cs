using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao;
using Tao.OpenGl;

namespace MarsRover.Classes.Core.Data
{
    public static class DataService
    {
        public static Point3d Rotate(Point3d vector, Angle3d angle)
        {
            Matrix vectorColumn = new Matrix(3, 1);
            vectorColumn[0, 0] = vector.X;
            vectorColumn[1, 0] = vector.Y;
            vectorColumn[2, 0] = vector.Z;

            Matrix rotationMatrix = new Matrix(3, 3);
            
            if (angle.X != 0)
            {
                rotationMatrix[0, 0] = 1;
                rotationMatrix[1, 1] = rotationMatrix[2, 2] = Math.Cos(Angle3d.DegreeToRadian(angle.X));
                rotationMatrix[1, 2] = -Math.Sin(Angle3d.DegreeToRadian(angle.X));
                rotationMatrix[2, 1] = -rotationMatrix[1, 2];

                vectorColumn = rotationMatrix * vectorColumn;
                rotationMatrix.Clear();
            }

            if (angle.Y != 0)
            {
                rotationMatrix[0, 0] = rotationMatrix[2, 2] = Math.Cos(Angle3d.DegreeToRadian(angle.Y));
                rotationMatrix[0, 2] = Math.Sin(Angle3d.DegreeToRadian(angle.Y));
                rotationMatrix[1, 1] = 1;
                rotationMatrix[2, 0] = -rotationMatrix[0, 2];

                vectorColumn = rotationMatrix * vectorColumn;
                rotationMatrix.Clear();
            }

            if (angle.Z != 0)
            {
                rotationMatrix[0, 0] = rotationMatrix[1, 1] = Math.Cos(Angle3d.DegreeToRadian(angle.Z));
                rotationMatrix[2, 2] = 1;
                rotationMatrix[1, 0] = Math.Sin(Angle3d.DegreeToRadian(angle.Z));
                rotationMatrix[0, 1] = -rotationMatrix[1, 0];

                vectorColumn = rotationMatrix * vectorColumn;
            }

            Point3d result = new Point3d(vectorColumn[0, 0], vectorColumn[1, 0], vectorColumn[2, 0]);
            return result;
        }
    }
}

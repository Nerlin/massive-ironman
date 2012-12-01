using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Classes.Core.Data
{
    public class Matrix
    {
        double[,] matrix;
        public double this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Matrix(int rows, int columns)
        {
            Initialize(rows, columns);
        }
        public Matrix(int rows, int columns, double[] values)
        {
            Initialize(rows, columns);
            if (values.Length != rows * columns)
                throw new Exception("Длина массива значений задана неверно.");

            int n = 0;

            for (int i = 0; i <= rows - 1; i++)
                for (int j = 0; j <= columns - 1; j++)
                    matrix[i, j] = values[n++];
        }

        public void Clear()
        {
            for (int i = 0; i <= this.Rows - 1; i++)
                for (int j = 0; j <= this.Columns - 1; j++)
                    matrix[i, j] = 0;
        }
           
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
                throw new Exception("Форма матриц не согласована.");

            Matrix result = new Matrix(a.Rows, b.Columns);

            for (int i = 0; i <= result.Rows - 1; i++)
                for (int j = 0; j <= result.Columns - 1; j++)
                    for (int k = 0; k <= result.Rows - 1; k++)
                        result[i, j] += a[i, k] * b[k, j];
            return result;
        }
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows)
                throw new Exception("Форма матриц не согласована.");

            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i <= result.Rows - 1; i++)
                for (int j = 0; j <= result.Columns - 1; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }
        public static Matrix operator *(Matrix a, double k)
        {
            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i <= result.Rows - 1; i++)
                for (int j = 0; j <= result.Columns - 1; j++)
                    result[i, j] *= k;
            return result;
        }

        private void Initialize(int rows, int columns)
        {
            matrix = new double[rows, columns];
            this.Rows = rows;
            this.Columns = columns;
        }
    }
}

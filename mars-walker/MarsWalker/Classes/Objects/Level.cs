using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Text;

using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;

using Tao;
using Tao.OpenGl;

namespace MarsRover.Classes.Objects
{
    public class GameLevel
    {
        private Rectangle[,] blocks;
        private byte[,] textures;
        /// <summary>
        /// Ширина уровня.
        /// </summary>
        public int Width
        {
            get;
            set;
        }
        /// <summary>
        /// Длина уровня.
        /// </summary>
        public int Length
        {
            get;
            set;
        }
        /// <summary>
        /// Высота уровня.
        /// </summary>
        public int Height
        {
            get;
            set;
        }
        public int Left { 
            get { return -Width / 2; } }
        public int Right {
            get { return Width / 2; } }
        public int Near {
            get { return Length / 2; } }
        public int Far {
            get { return -Length / 2; } }

        private const double zerolevel = -2;


        public void Generate()
        {
            Random random = new Random();
            int lengthX = this.Width / 10, lengthY = this.Length / 10, x = 0, y = 0;


            blocks = new Rectangle[lengthX, lengthY];
            textures = new byte[lengthX, lengthY];

            for (int i = Left; i <= Right - 10; i += 10)
                for (int j = Far; j <= Near - 10; j += 10)
                {
                    byte t = (byte)random.Next(1, 3);
                    Point3d[] p = new Point3d[4] {
                        new Point3d(i, -2, j),
                        new Point3d(i, -2, j + 10),
                        new Point3d(i + 10, -2, j + 10),
                        new Point3d(i + 10, -2, j) };

                    Rectangle rect = new Rectangle(p);
                    blocks[x, y] = rect;
                    textures[x, y++] = t;

                    if (y > lengthY - 1)
                    {
                        x++;
                        y = 0;
                    }
                }
        }

        public void Draw()
        {
            for (int i = 0; i <= blocks.GetLength(0) - 1; i++)
                for (int j = 0; j <= blocks.GetLength(1) - 1; j++)
                {
                    Point3d[] p = blocks[i, j].GetValues();
                    DrawingService.PickTexture(string.Format("sand0{0}.jpg", textures[i, j]));
                    Point3d[] t = DrawingService.GetTextureCoordinates();
                    DrawingService.DrawPolygon(p, t, Side.Front, new Point3d(0, 1, 0));
                }

            DrawingService.PickTexture(TextureNames.BackGround);
            
            Gl.glBegin(Gl.GL_POLYGON);
            {
                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(2, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(2, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);

            }
            Gl.glEnd();

            Gl.glBegin(Gl.GL_POLYGON);
            {
                Gl.glTexCoord2d(0, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glTexCoord2d(2, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glTexCoord2d(2, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);

            }
            Gl.glEnd();

            Gl.glBegin(Gl.GL_POLYGON);
            {
                Gl.glTexCoord2d(0, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glTexCoord2d(2, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z - DrawingService.Distance);

                Gl.glTexCoord2d(2, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);
            }
            Gl.glEnd();

            Gl.glBegin(Gl.GL_POLYGON);
            {
                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(2, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(2, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, zerolevel, DrawingService.CameraPosition.Z + DrawingService.Distance);

                Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);
            }
            Gl.glEnd();

            Gl.glBegin(Gl.GL_POLYGON);
            {
                Gl.glTexCoord2d(0, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);
                Gl.glNormal3b(0, -1, 0);

                Gl.glTexCoord2d(2, 4);
                Gl.glVertex3d(DrawingService.CameraPosition.X + DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);
                Gl.glNormal3b(0, -1, 0);

                Gl.glTexCoord2d(2, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z + DrawingService.Distance);
                Gl.glNormal3b(0, -1, 0);

                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(DrawingService.CameraPosition.X - DrawingService.Distance, DrawingService.CameraPosition.Y + DrawingService.Distance, DrawingService.CameraPosition.Z - DrawingService.Distance);
                Gl.glNormal3b(0, -1, 0);
            }
            Gl.glEnd();
            Gl.glFlush();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;

using Tao;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;

using MarsRover.Classes.Objects;
using MarsRover.Classes.Core.Data;


namespace MarsRover.Classes.Core
{
    public enum Side { Front, Back }   
    /// <summary>
    /// Средство для отрисовки объектов.
    /// </summary>
    static class DrawingService
    {
        #region Fields & Properties
        private static GameLevel presentLevel;
        private static Thread thread;

        public static Point3d CameraPosition { get; private set; }

        public static GameLevel PresentLevel
        {
            get { return presentLevel; }
            private set {
                presentLevel = value;
            }
        }
        public static Boolean Ready {
            get; private set;
        }
        public static Dictionary<string, Texture> Textures { get; set; }

        public static int Width
        {
            get;
            set;
        }
        public static int Height
        {
            get;
            set;
        }

        static int distance;
        public static int Distance 
        {
            get { return distance - 100; }
            set {
                if (value > 95) 
                    distance = value; }
        }
        #endregion
        #region Constants
        #endregion

        public static void Init(int width, int height)
        {
            DrawingService.Resize(width, height);
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DEPTH | Glut.GLUT_DOUBLE);
           
            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor(0f, 0f, 0f, 1f);
                     
            /// TODO:
            /// Добавить включение света
            /// Добавить проверку глубины
            /// Включить (солнце)        
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            //Gl.glEnable(Gl.GL_FOG);
            //Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_CULL_FACE);
            //Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
            //Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[4] { 12, 10, 6, 1 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[3] { 1f, 1f, 1f });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[3] { 1f, 1f, 1f });
            //Gl.glLightf(Gl.GL_LIGHT0, Gl.GL_LINEAR_ATTENUATION, 0.01f);
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, new float[4] { 0, 10, 0, 0 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPOT_DIRECTION, new float[3] { 0, 0, 0 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, new float[3] { 0.7f, 0.7f, 0.7f });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, new float[3] { 0.7f, 0.7f, 0.7f });
            Gl.glEnable(Gl.GL_LIGHT1);

            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_LINEAR_ATTENUATION, 0.005f);

            //Gl.glLightModelf(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE);
            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, new float[3] { 0.7f, 0.7f, 0.7f });
            //Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[3] { 0.2f, 0.2f, 0.2f });

            //Gl.glFogi(Gl.GL_FOG_MODE, Gl.GL_LINEAR);
            //Gl.glFogfv(Gl.GL_FOG_COLOR, new float[4] { 0.6f, 0.5f, 0.5f, 1f });
            //Gl.glFogf(Gl.GL_FOG_DENSITY, 0.35f);
            //Gl.glHint(Gl.GL_FOG_HINT, Gl.GL_FASTEST);

            DrawingService.Width = width;
            DrawingService.Height = height;
            DrawingService.Ready = true;

            DrawingService.Clear();
            DrawingService.LoadLevel();

            DrawingService.Textures = new Dictionary<string, Texture>();
            InitTextures();
        }
        public static void LoadLevel()
        {
            /// TODO: Загрузка уровня.
            presentLevel = new GameLevel();
            presentLevel.Height = 150;
            presentLevel.Width = 450;
            presentLevel.Length = 450;
            presentLevel.Generate();

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            DrawingService.Distance = 280;
            Gl.glFrustum(-5, 5, -5, 5, 30, distance + 75);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glTranslated(0, 0, -45);

            thread = new Thread(presentLevel.Draw);
        }
        public static void SetCamera(Camera camera)
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glTranslated(0, 0, -45);

            Gl.glRotated(camera.Angle.X, 1, 0, 0);
            Gl.glRotated(camera.Angle.Y, 0, 1, 0);
            Gl.glRotated(camera.Angle.Z, 0, 0, 1);

            Gl.glScaled(camera.Scale, camera.Scale, camera.Scale);
            Gl.glTranslated(camera.Position.X, camera.Position.Y, camera.Position.Z);

            Gl.glFogf(Gl.GL_FOG_START, 80);
            Gl.glFogf(Gl.GL_FOG_END, 120);

            CameraPosition = -camera.Position;
        }
        public static void DrawLevel()
        {
            presentLevel.Draw();
        }

        public static void Clear()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }
        public static void Resize(int width, int height)
        {
            Gl.glViewport(0, 0, width, height);
        }
        public static void Finish()
        {
            Gl.glFlush();
        }

        public static void DrawCircle(double x, double y, double z)
        {
            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 360; i >= 1; i--)
            {
                double x1 = Math.Cos(Angle3d.DegreeToRadian(i));
                double y1 = Math.Sin(Angle3d.DegreeToRadian(i));
                Gl.glVertex3d(x + x1 / 2, y + y1 / 2, z);
            }
            Gl.glEnd();
        }
        public static void DrawPolygon(Point3d[] points)
        {
            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i <= points.Length - 1; i++)
                Gl.glVertex3d(points[i].X, points[i].Y, points[i].Z);
            Gl.glEnd();
        }
        public static void DrawPolygon(Point3d[] points, Point3d[] textureCoords = null, Side side = Side.Front,
            Point3d normal = null)
        {
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i <= points.Length - 1; i++)
            {
                if (normal != null)
                    Gl.glNormal3d(normal.X, normal.Y, normal.Z);

                if (textureCoords != null && textureCoords[i] != null)
                    Gl.glTexCoord3d(textureCoords[i].X, textureCoords[i].Y, textureCoords[i].Z);
                
                Gl.glVertex3d(points[i].X, points[i].Y, points[i].Z);
            }           
            Gl.glEnd();
            Gl.glPopMatrix();
        }
        public static void DrawPolygon(Point3d[] points, Point3d[] textureCoords, string textureName)
        {
            PickTexture(textureName);
            Gl.glBegin(Gl.GL_POLYGON);         
            for (int i = 0; i <= points.Length - 1; i++)
            {
                if (textureCoords[i] != null)
                    Gl.glTexCoord3d(textureCoords[i].X, textureCoords[i].Y, textureCoords[i].Z);

                Gl.glVertex3d(points[i].X, points[i].Y, points[i].Z);
            }
            DrawingService.UnloadTexture();
            Gl.glEnd();
        }
        public static void DrawRim(Point3d p1, Point3d p2, double size = 1, double radius = 1,
            string textureName = null)
        {
            for (int i = 360; i >= 1; i--)
            {
                if (textureName != null)
                    DrawingService.PickTexture(textureName);

                Gl.glBegin(Gl.GL_POLYGON);
                double x1 = Math.Cos(Angle3d.DegreeToRadian(i));
                double y1 = Math.Sin(Angle3d.DegreeToRadian(i));
                double x2 = Math.Cos(Angle3d.DegreeToRadian(i - 3));
                double y2 = Math.Sin(Angle3d.DegreeToRadian(i - 3));

                Gl.glNormal3d(x1, y1, y1);
                Gl.glTexCoord3d(x2, 0, 0);
                //Gl.glNormal3b(0, 1, 0);
                Gl.glVertex3d(x2 * size + p2.X, y2 * radius + p2.Y, y2 * radius + p2.Z);

                Gl.glNormal3d(x1, y1, y1);
                Gl.glTexCoord3d(x2, 1, 0);
                //Gl.glNormal3b(0, 1, 0);
                Gl.glVertex3d(x2 * size + p1.X, y2 * radius + p1.Y, y2 * radius + p1.Z);

                Gl.glNormal3d(x1, y1, y1);
                Gl.glTexCoord3d(x1, 1, 0);
                //Gl.glNormal3b(0, 1, 0);
                Gl.glVertex3d(x1 * size + p1.X, y1 * radius + p1.Y, y1 * radius + p1.Z);

                Gl.glNormal3d(x1, y1, y1);
                Gl.glTexCoord3d(x1, 0, 0);
                //Gl.glNormal3b(0, 1, 0);
                Gl.glVertex3d(x1 * size + p2.X, y1 * radius + p2.Y, y1 * radius + p2.Z);
                Gl.glEnd();
                DrawingService.UnloadTexture();
            }
            
        }

        public static void InitTextures()
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            LoadTexture(TextureNames.Bus);
            LoadTexture(TextureNames.Body);
            LoadTexture(TextureNames.Metal);
            LoadTexture(TextureNames.BackGround);
            LoadTexture(TextureNames.Sand1);
            LoadTexture(TextureNames.Sand2);
            LoadTexture(TextureNames.Side);
            LoadTexture(TextureNames.Back);
            LoadTexture(TextureNames.Sensor);
        }
        public static void LoadTexture(string textureName)
        {           
            Texture texture = new Texture();

            int id;
            Gl.glGenTextures(1, out id);

            texture.ID = id;
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture.ID);

            if (!File.Exists("Textures\\" + textureName))
                throw new Exception("Текстура не найдена.");

            Bitmap image = (Bitmap)Image.FromFile("Textures\\" + textureName);
            List<byte> textureImage = new List<byte>();

            for (int i = 0; i <= image.Width - 1; i++)
                for (int j = 0; j <= image.Height - 1; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    textureImage.AddRange(new List<byte> { pixel.R, pixel.G, pixel.B, pixel.A });
                }

            texture.Image = textureImage.ToArray();
            texture.Size = image.Size;

            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexEnvf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_BLEND);

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA,
                image.Width, image.Height, 1, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, texture.Image);

            Textures.Add(textureName, texture);
        }
        public static void GenerateTexture()
        {
            Gl.glEnable(Gl.GL_TEXTURE_GEN_S);
            Gl.glTexGend(Gl.GL_TEXTURE_GEN_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);

            Gl.glEnable(Gl.GL_TEXTURE_GEN_T);
            Gl.glTexGend(Gl.GL_TEXTURE_GEN_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);

            //Gl.glEnable(Gl.GL_TEXTURE_GEN_R);
            //Gl.glTexGend(Gl.GL_TEXTURE_GEN_R, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_OBJECT_PLANE);

            //Gl.glEnable(Gl.GL_TEXTURE_GEN_Q);
            //Gl.glTexGend(Gl.GL_TEXTURE_GEN_Q, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_OBJECT_PLANE);
        }
        public static void GenerateTexture(string textureName)
        {
            if (!Textures.ContainsKey(textureName))
                throw new Exception("Нет такой текстуры.");

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures[textureName].ID);
            Gl.glEnable(Gl.GL_TEXTURE_GEN_S);
            Gl.glTexGend(Gl.GL_TEXTURE_GEN_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_PLANE);

            Gl.glEnable(Gl.GL_TEXTURE_GEN_T);
            Gl.glTexGend(Gl.GL_TEXTURE_GEN_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_PLANE);

            Gl.glEnable(Gl.GL_TEXTURE_GEN_R);
            Gl.glTexGend(Gl.GL_TEXTURE_GEN_R, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_PLANE);

            //Gl.glEnable(Gl.GL_TEXTURE_GEN_Q);
            //Gl.glTexGend(Gl.GL_TEXTURE_GEN_Q, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_OBJECT_PLANE);
        }
        public static void DeactivateGeneration()
        {
            Gl.glDisable(Gl.GL_TEXTURE_GEN_S);
            Gl.glDisable(Gl.GL_TEXTURE_GEN_T);
            Gl.glDisable(Gl.GL_TEXTURE_GEN_R);
            //Gl.glDisable(Gl.GL_TEXTURE_GEN_Q);
        }

        public static void PickTexture(string textureName)
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures[textureName.ToString()].ID);
        }
        public static void UnloadTexture()
        {
            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        public static Point3d[] GetTextureCoordinates()
        {
            Point3d[] txtCrds = new Point3d[4] {
                new Point3d(0, 0, 0),
                new Point3d(1, 0, 0),
                new Point3d(1, 1, 0),
                new Point3d(0, 1, 0)
            };
            return txtCrds;
        }
    }
}

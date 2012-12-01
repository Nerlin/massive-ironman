using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao;
using Tao.OpenGl;

using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;

namespace MarsRover.Classes.Objects
{
    public class Wheel: GraphObject
    {
        public override void Draw()
        {
            Gl.glPushMatrix();
            Gl.glTranslated(this.Position.X, this.Position.Y, this.Position.Z);
            Gl.glRotated(this.Angle.X, 1, 0, 0);
            Gl.glRotated(this.Angle.Y, 0, 1, 0);
            Gl.glRotated(this.Angle.Z, 0, 0, 1);

            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -0.5);
            
            DrawSide(1);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, 0.5);
            DrawRim();
            Gl.glRotated(180, 0, -1, 0);
            DrawSide(2);        
            Gl.glPopMatrix();

            Gl.glPopMatrix();
        }

        private void DrawSide(int side)
        {
            Gl.glPushMatrix();
            Gl.glColor3f(1f, 0.33f, 0.66f);
            DrawRim(0.05, 2);

            //double distance = -0.0;
            //if (side == 2)
            //{
            //    //Gl.glRotated(180, 0, 1, 0);
            //    distance *= -1;
            //}
 
            //Gl.glTranslated(0, 0, 0);
            Gl.glScaled(0.5, 0.5, 0.5);
            Gl.glColor3f(1f, 1f, 1f);
            DrawDisk();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glScaled(0.5, 0.5, 0.5);
            Gl.glColor3f(0.5f, 0.33f, 0.66f);

            DrawingService.DrawCircle(0, 0, 0);
            Gl.glPopMatrix();
        }

        private void DrawRim(double depth = 1, double size = 1)
        {
            if (size == 0)
                size = 1;
         
            bool flag = depth == 1 && size == 1;        
            for (int i = 360; i >= 0; i--)
            {
                if (flag)
                    DrawingService.PickTexture(TextureNames.Bus);
                else
                    DrawingService.PickTexture(TextureNames.Body);

                Gl.glBegin(Gl.GL_POLYGON);
                double x1 = Math.Cos(Angle3d.DegreeToRadian(i));
                double y1 = Math.Sin(Angle3d.DegreeToRadian(i));
                double x2 = Math.Cos(Angle3d.DegreeToRadian(i - 2));
                double y2 = Math.Sin(Angle3d.DegreeToRadian(i - 2));
                if (flag) Gl.glTexCoord3d(x2, 0, 0);
                else
                    Gl.glTexCoord3d(x2 - 0.5, y2 - 1, 0);
                Gl.glNormal3d(x2, y2, 0);
                Gl.glVertex3d(x2, y2, 0);

                Gl.glNormal3d(0, 0, 1);
                if (flag) Gl.glTexCoord3d(x2, 1, 0);
                else
                    Gl.glTexCoord3d(x2 / size - 0.5, y2 / size - 1, 0);
                Gl.glVertex3d(x2 / size, y2 / size, -depth);

                Gl.glNormal3d(0, 0, 1);
                if (flag) Gl.glTexCoord3d(x1, 1, 0);
                else
                    Gl.glTexCoord3d(x1 / size - 0.5, y1 / size - 1, 0);
                Gl.glVertex3d(x1 / size, y1 / size, -depth);

                Gl.glNormal3d(0, 0, 1);
                if (flag) Gl.glTexCoord3d(x1, 0, 0);
                else
                    Gl.glTexCoord3d(x1 - 0.5, y1 - 1, 0);
                Gl.glVertex3d(x1, y1, 0);

                
                DrawingService.UnloadTexture();
                Gl.glEnd();
            }
        }
        private void DrawDisk()
        {
            for (int angle = 0; angle <= 360; angle += 72)
            {
                DrawingService.PickTexture(TextureNames.Metal);
                Gl.glBegin(Gl.GL_POLYGON);
                double x1 = Math.Cos(Angle3d.DegreeToRadian(angle));
                double y1 = Math.Sin(Angle3d.DegreeToRadian(angle));
                double x2 = Math.Cos(Angle3d.DegreeToRadian(angle - 35));
                double y2 = Math.Sin(Angle3d.DegreeToRadian(angle - 35));

                //Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(1, 1);
                Gl.glVertex3d(x1, y1, 0);

                //Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0.7, 0.7);
                Gl.glVertex3d(x1 * 0.2, y1 * 0.2, 0);

                //Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0.35, 0.35);
                Gl.glVertex3d(x2, y2, 0);

                //Gl.glNormal3b(0, 0, 1);
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(x2 * 0.2, y2 * 0.2, 0);
                
                DrawingService.UnloadTexture();
                Gl.glEnd();
            }
        }
    }

        
}

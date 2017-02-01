using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace WindowsFormsApplication2
{

    public struct Point3D
    {
        public Point3D(double xx, double yy, double zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public double x;
        public double y;
        public double z;
    }

    public partial class Form1 : Form
    {
        float AngleX = 0;
        float AngleY = 0;
        float AngleZ = 0;
        
        const float AngleDl = 5;

       PolygonMode mode = PolygonMode.Fill;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Viewport(0, 0, w, h); 

        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {

            // очистка буферов цвета и глубины
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // поворот изображения
            GL.LoadIdentity();
            GL.Rotate(AngleX, 1.0, 0.0, 0.0);
            GL.Rotate(AngleY, 0.0, 1.0, 0.0);
            GL.Rotate(AngleZ, 0.0, 0.0, 1.0);

            // оси координат
            GL.Begin(BeginMode.Lines);
            // ось x
            GL.Color3(1f, 1, 1); GL.Vertex2(-6, 0);
            GL.Color3(0f, 0, 1); GL.Vertex2(6, 0);
            // ось y
            GL.Color3(1f, 1, 1); GL.Vertex2(0, -6);
            GL.Color3(0f, 1, 0); GL.Vertex2(0, 6);
            // ось z
            GL.Color3(1f, 1, 1); GL.Vertex3(0, 0, -6);
            GL.Color3(1f, 0, 0); GL.Vertex3(0, 0, 6);
            GL.End();
            //...//


            GL.Color3(1f, 0, 0);

            // формирование изображения
            GL.PolygonMode(MaterialFace.FrontAndBack, mode);
            GL.Begin(BeginMode.Quads);
            //double r = 0.5;
            //int n = 10;
            //for (int i = 0; i <= n; ++i)
            //{
            //    double a = 2 * Math.PI/n * i;
            //    double x = r * Math.Cos(a);
            //    double y = r * Math.Sin(a);
            //    GL.Vertex3(x, y, -0.5);
            //    GL.Vertex3(x, y,  0.5);
            //}


            var vertexArr = new Point3D[8];

            var sPoint = new Point3D() { x = -0.5, y = -0.5, z = 0 };
            var ePoint = new Point3D() { x =  0.5, y =  0.5, z = 0 };
           
            var width = 0.1;
            var height = 0.1;
            
            var hypotenuse = Math.Sqrt(Math.Pow((ePoint.x - sPoint.x), 2) + Math.Pow((ePoint.y - sPoint.y), 2));
            
            var sinAlpha = (ePoint.y - sPoint.y) / hypotenuse;
            var cosAlpha = (ePoint.x - sPoint.x) / hypotenuse;
            
            var xfs = sPoint.x - (width / 2) * sinAlpha;
            var yfs = sPoint.y + (width / 2) * cosAlpha;
            
            var xls = sPoint.x + (width / 2) * sinAlpha;
            var yls = sPoint.y - (width / 2) * cosAlpha;
            
            var xfe = ePoint.x - (width / 2) * sinAlpha;
            var yfe = ePoint.y + (width / 2) * cosAlpha;
            
            var xle = ePoint.x + (width / 2) * sinAlpha;
            var yle = ePoint.y - (width / 2) * cosAlpha;

            //vertexArr[0] = new Point3D(xfs, yfs, 0);
            //vertexArr[1] = new Point3D(xfs, yfs, height);
            //vertexArr[2] = new Point3D(xls, yls, height);
            //vertexArr[3] = new Point3D(xls, yls, 0);
            //vertexArr[4] = new Point3D(xfe, yfe, 0);
            //vertexArr[5] = new Point3D(xfe, yfe, height);
            //vertexArr[6] = new Point3D(xle, yle, height);
            //vertexArr[7] = new Point3D(xle, yle, 0);

            GL.Vertex3(xfs, yfs, 0);
            GL.Vertex3(xfs, yfs, height);
            GL.Vertex3(xls, yls, height);
            GL.Vertex3(xls, yls, 0);
            GL.Vertex3(xfe, yfe, 0);
            GL.Vertex3(xfe, yfe, height);
            GL.Vertex3(xle, yle, height);
            GL.Vertex3(xle, yle, 0);

            GL.End();
            // завершение формирования изображения

            GL.Flush();
            GL.Finish();

            glControl1.SwapBuffers();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupViewport();
            GL.ClearColor(1f, 1f, 1f, 1f); // цвет фона
            GL.Enable(EnableCap.DepthTest);

            AngleX = 30;
            AngleY = 30;
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Insert:   AngleX += AngleDl; break;
                case Keys.Delete:   AngleX -= AngleDl; break;

                case Keys.Home:     AngleY += AngleDl; break;
                case Keys.End:      AngleY -= AngleDl; break;

                case Keys.Prior:    AngleZ += AngleDl; break;
                case Keys.Next:     AngleZ -= AngleDl; break;


                case Keys.F1: mode = PolygonMode.Fill; break;
                case Keys.F2: mode = PolygonMode.Line; break;
                case Keys.F3: mode = PolygonMode.Point; break;
            }

            glControl1.Invalidate(); 
        }
    }
}

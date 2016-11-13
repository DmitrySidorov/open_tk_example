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
            GL.Begin(BeginMode.QuadStrip);
            double r = 0.5;
            int n = 10;
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI/n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.Vertex3(x, y, -0.5);
                GL.Vertex3(x, y,  0.5);
            }
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

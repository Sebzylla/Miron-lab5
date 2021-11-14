using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Lab_5
{
    class Axes
    {
        private bool myVisibility;

        private const int AXIS_LENGTH = 75;

        public Axes()
        {
            myVisibility = true;
        }

        public void Draw()
        {
            if (myVisibility)
            {
                GL.LineWidth(1.0f);

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(AXIS_LENGTH, 0, 0);
                GL.Color3(Color.ForestGreen);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, AXIS_LENGTH, 0);
                GL.Color3(Color.RoyalBlue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, AXIS_LENGTH);
                GL.End();
            }
        }

        public void Show()
        {
            myVisibility = true;
        }

        public void Hide()
        {
            myVisibility = false;
        }

        public void ToggleVisibility()
        {
            myVisibility = !myVisibility;
        }
    }
}

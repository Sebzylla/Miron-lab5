using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Lab_5
{
    class Window3D : GameWindow
    {
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;
        private readonly Randomizer rando;
        private readonly Axes ax;
        private readonly Grid grid;
        private Camera3DIsometric cam;
        private bool displayMarker;
        private ulong updatesCounter;
        private ulong framesCounter;
        private MassiveObjects objy;
        private bool move;

        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            rando = new Randomizer();
            ax = new Axes();
            grid = new Grid();
            cam = new Camera3DIsometric();
            objy = new MassiveObjects(Color.Yellow);

            DisplayHelp();
            displayMarker = false;
            updatesCounter = 0;
            framesCounter = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.ClearColor(DEFAULT_BKG_COLOR);

            GL.Viewport(0, 0, this.Width, this.Height);

            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            cam.SetCamera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            updatesCounter++;

            if (displayMarker)
            {
                TimeStampIt("update", updatesCounter.ToString());
            }

            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if (currentKeyboard[Key.Escape])
            {
                Exit();
            }

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayHelp();
            }

            if (currentKeyboard[Key.R] && !previousKeyboard[Key.R])
            {
                GL.ClearColor(DEFAULT_BKG_COLOR);
                ax.Show();
                grid.Show();
            }

            if (currentKeyboard[Key.K] && !previousKeyboard[Key.K])
            {
                ax.ToggleVisibility();
            }

            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(rando.RandomColor());
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                grid.ToggleVisibility();
            }

            if (currentKeyboard[Key.O] && !previousKeyboard[Key.O])
            {
                objy.ToggleVisibility();
            }

            if (currentKeyboard[Key.W])
            {
                cam.MoveForward();
            }
            if (currentKeyboard[Key.S])
            {
                cam.MoveBackward();
            }
            if (currentKeyboard[Key.A])
            {
                cam.MoveLeft();
            }
            if (currentKeyboard[Key.D])
            {
                cam.MoveRight();
            }
            if (currentKeyboard[Key.Q])
            {
                cam.MoveUp();
            }
            if (currentKeyboard[Key.E])
            {
                cam.MoveDown();
            }

            if (currentKeyboard[Key.L] && !previousKeyboard[Key.L])
            {
                displayMarker = !displayMarker;
            }

            if(currentMouse[MouseButton.Left] && !previousMouse[MouseButton.Left])
            {
                objy.Draw();
                move = true;
            }

            //Seteaza camera in pozitia "departe"
            if(currentKeyboard[Key.Z] && !previousKeyboard[Key.Z])
            {
                cam = new Camera3DIsometric(200, 175, 25, 0, 25, 0, 0, 1, 0);
                cam.SetCamera();
            }

            //Seteaza camera in pozitia "aproape"
            if (currentKeyboard[Key.X] && !previousKeyboard[Key.X])
            {
                cam = new Camera3DIsometric(100, 85, 10, 0, 25, 0, 0, 1, 0);
                cam.SetCamera();
            }

            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            framesCounter++;

            if (displayMarker)
            {
                TimeStampIt("render", framesCounter.ToString());
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            grid.Draw();
            ax.Draw();
            objy.Draw();

            if (move == true)
            {
                GL.PushMatrix();
                for (int i = (int)updatesCounter; i <= (int)updatesCounter + 50; i++)
                {
                    GL.Translate(0, -1, 0);
                    objy.Draw();
                }
                GL.PopMatrix();
                move = false;
            }

            SwapBuffers();
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\n      MENIU");
            Console.WriteLine(" (H) - meniul");
            Console.WriteLine(" (ESC) - parasire aplicatie");
            Console.WriteLine(" (K) - schimbare vizibilitate sistem de axe");
            Console.WriteLine(" (R) - resteaza scena la valori implicite");
            Console.WriteLine(" (B) - schimbare culoare de fundal");
            Console.WriteLine(" (V) - schimbare vizibilitate linii");
            Console.WriteLine(" (W,A,S,D) - deplasare camera (izometric)");
        }

        private void TimeStampIt(String source, String counter)
        {
            String dt = DateTime.Now.ToString("hh:mm:ss.ffff");
            Console.WriteLine("     TSTAMP from <" + source + "> on iteration <" + counter + ">: " + dt);
        }
    }
}

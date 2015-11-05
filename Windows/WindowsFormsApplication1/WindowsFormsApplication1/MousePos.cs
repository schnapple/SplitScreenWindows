using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    internal class MousePos
    {
        int mpX;
        int mpY;
        public MousePos(){
            Point mousepoint = Control.MousePosition;
            mpX = mousepoint.X;
            mpY = mousepoint.Y;

        }
        public event MousePosHandler mpHand;
        public EventArgs e = null;
        public delegate void MousePosHandler(MousePos mp, EventArgs e);

        public void record()
        {
            while (true)
            {
                Point mousepoint = Control.MousePosition;
                if (mpX != mousepoint.X || mpY != mousepoint.Y) {
                    mpX = mousepoint.X;
                    mpY = mousepoint.Y;
                }
            }
        }
    }
}
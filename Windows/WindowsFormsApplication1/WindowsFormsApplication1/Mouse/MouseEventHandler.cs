using System;

namespace Mouse
{
    internal class MouseEventHandler
    {
        private Action<object, EventArgs> pictureBox1_MouseMove;

        public MouseEventHandler(Action<object, EventArgs> pictureBox1_MouseMove)
        {
            this.pictureBox1_MouseMove = pictureBox1_MouseMove;
        }
    }
}
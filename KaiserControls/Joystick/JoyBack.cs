using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kaiser {
    public class JoyBack : PictureBox {

        public CentreStick Joystick;
        public event EventHandler JoyReleased;

        public JoyBack() {
            this.DoubleBuffered = true;
            Size s = new Size(150, 150);
            this.MaximumSize = s;
            this.MinimumSize = s;
            AttachStick();
        }

        async Task AttachStick() {
            await Task.Delay(100);
            Joystick = new CentreStick(this);
        }

        protected override void OnPaint(PaintEventArgs pe) {
            pe.Graphics.DrawImage(Properties.Resources.back,
                0, 0);

            base.OnPaint(pe);
        }

        private Rectangle MakeRectangle(Point p1, Point p2) {
            int x = Math.Min(p1.X, p2.X);
            int y = Math.Min(p1.Y, p2.Y);
            int width = Math.Abs(p1.X - p2.X);
            int height = Math.Abs(p1.Y - p2.Y);
            return new Rectangle(x, y, width, height);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
        }

        public void CallReleased() {
            if(JoyReleased != null)
                JoyReleased(null, null);
        }
    }

}

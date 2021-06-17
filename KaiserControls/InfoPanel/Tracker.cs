using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Kaiser {
    public partial class Tracker : PictureBox {

        CustomTrackbar myTrackbar;
        bool active;
        private Point eOriginalPos;
        public int percentage = 0;

        public Tracker(CustomTrackbar parent) {
            myTrackbar = parent;
            this.Parent = myTrackbar;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(50, parent.Height);
            this.Image = Properties.Resources.tracker;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            UpdatePos();
        }

        //protected override CreateParams CreateParams {
        //    get {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle = cp.ExStyle | 0x2000000;
        //        return cp;
        //    }
        //}

        public void UpdatePos(int posX = 0) {
            if (posX == 0)
                posX = Location.X;

            Location = new Point(posX, (int)Math.Round((myTrackbar.Height / 2f) - (Height / 2f)));
        }

        protected override void OnMouseEnter(EventArgs e) {
            Cursor = Cursors.SizeWE;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            Cursor = Cursors.Default;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            active = true;
            eOriginalPos = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            active = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (active) {
                int maxDist = myTrackbar.Width - Width;
                int newLocX = Location.X;

                newLocX += e.Location.X - eOriginalPos.X;
                
                if (newLocX > maxDist)
                    newLocX = maxDist;
                else if (newLocX < 0)
                    newLocX = 0;

                UpdatePos(newLocX);
                percentage = (int)(Math.Round((newLocX + 1f) / (maxDist + 1f), 2) * 100);
                Console.WriteLine(percentage);
            }

            base.OnMouseMove(e);
        }

    }
}

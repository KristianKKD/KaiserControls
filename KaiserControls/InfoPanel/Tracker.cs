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

        public void UpdatePos(float percent = 0) {
            try {
                if (percent == 0)
                    percent = percentage / 100f;

                if (!myTrackbar.verticalMode)
                    Location = new Point((int)Math.Round(percent * (myTrackbar.Width - Width)), (int)Math.Round((myTrackbar.Height / 2f) - (Height / 2f)));
                else
                    Location = new Point((int)Math.Round((myTrackbar.Width / 2f) - (Width / 2f)), (int)Math.Round(percent * (myTrackbar.Height - Height)));
            }catch(Exception e) {
                Console.WriteLine("UPDATEPOS\n" + e.ToString());
            }
        }

        protected override void OnMouseEnter(EventArgs e) {
            if(!myTrackbar.verticalMode)
                Cursor = Cursors.SizeWE;
            else
                Cursor = Cursors.SizeNS;

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
                int maxDist, newLoc;

                if (!myTrackbar.isVertical) {
                    maxDist = myTrackbar.Width - Width;
                    newLoc = Location.X + (e.Location.X - eOriginalPos.X);
                } else {
                    maxDist = myTrackbar.Height - Height;
                    newLoc = Location.Y + (e.Location.Y - eOriginalPos.Y);
                }

                if (newLoc > maxDist)
                    newLoc = maxDist;
                else if (newLoc < 0)
                    newLoc = 0;

                percentage = (int)(Math.Round((newLoc + 1f) / (maxDist + 1f), 2) * 100);
                UpdatePos();
            }

            base.OnMouseMove(e);
        }

    }
}

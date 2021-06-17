using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kaiser {
    public partial class CustomTrackbar: PictureBox {

        Tracker track;
        public CustomTrackbar() {
            this.DoubleBuffered = true;
            this.Image = Properties.Resources.trackbar;
            this.SizeMode = PictureBoxSizeMode.StretchImage;

            track = new Tracker(this);
        }

        protected override void OnSizeChanged(EventArgs e) {
            int x = (int)Math.Round((track.percentage / 100f) * (Width - track.Width));
            track.UpdatePos(x);
            base.OnSizeChanged(e);
        }

        void MoveTracker() {
            
        }
    }
}

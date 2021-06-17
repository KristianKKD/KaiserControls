using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kaiser {
    public partial class CustomTrackbar: PictureBox {

        Tracker track;
        public bool verticalMode = false;

        [DefaultValue(false)]
        public bool isVertical {
            get {
                return this.verticalMode;
            }
            set {
                this.verticalMode = value;
            }
        }

        public CustomTrackbar() {
            this.DoubleBuffered = true;
            this.Image = Properties.Resources.trackbar;
            this.SizeMode = PictureBoxSizeMode.StretchImage;

            track = new Tracker(this);
        }

        protected override void OnSizeChanged(EventArgs e) {
            track.UpdatePos();
            base.OnSizeChanged(e);
        }

        void MoveTracker() {
            
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SizeablePanel {
    public class SizeablePanel : Panel {
        private const int cGripSize = 15;
        private bool resizing;

        private Point currentDragPos;
        private Point eOriginalPos;

        enum Direction {
            Up,
            Down,
            Left,
            Right,
            UpR,
            UpL,
            DownR,
            DownL,
        }

        Direction myDir;

        public SizeablePanel() {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.White;
        }

        private bool IsOnGrip(Point pos) {
            if (pos.X >= this.ClientSize.Width - cGripSize) {       //grabbed right side
                if (pos.Y >= this.ClientSize.Height - cGripSize) {  // bottom right
                    Cursor = Cursors.SizeNWSE;
                    myDir = Direction.DownR;
                } else if (pos.Y <= cGripSize) {                    // top right
                    Cursor = Cursors.SizeNESW;
                    myDir = Direction.UpR;
                } else { // right
                    Cursor = Cursors.SizeWE;
                    myDir = Direction.Right;
                }
            } else if (pos.X <= cGripSize) {                        //grabbed left side
                if (pos.Y >= this.ClientSize.Height - cGripSize) {  // bottom left
                    Cursor = Cursors.SizeNESW;
                    myDir = Direction.DownL;
                } else if (pos.Y <= cGripSize) {                    // top left
                    Cursor = Cursors.SizeNWSE;
                    myDir = Direction.UpL;
                } else {                                            // left
                    Cursor = Cursors.SizeWE;
                    myDir = Direction.Left;
                }
            } else if (pos.Y >= this.ClientSize.Height - cGripSize) {//grabbed down
                Cursor = Cursors.SizeNS;
                myDir = Direction.Down;
            } else if (pos.Y <= cGripSize) {                        //up
                Cursor = Cursors.SizeNS;
                myDir = Direction.Up;
            }

            return pos.X >= this.ClientSize.Width - cGripSize
                || pos.X <= cGripSize ||
                pos.Y >= this.ClientSize.Height - cGripSize
                || pos.Y <= cGripSize;
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            resizing = IsOnGrip(e.Location);
            currentDragPos = e.Location;
            eOriginalPos = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            resizing = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (resizing) {
                var location = Location;

                switch (myDir) {
                    case Direction.Up:
                        Size = new Size(Width, Height - (e.Y - eOriginalPos.Y));
                        location.Offset(0, e.Location.Y - eOriginalPos.Y);
                        break;
                    case Direction.Down:
                        Size = new Size(Width, Height + (e.Y - currentDragPos.Y));
                        break;
                    case Direction.Left:
                        Size = new Size(Width - (e.X - eOriginalPos.X), Height);
                        location.Offset(e.Location.X - eOriginalPos.X, 0);
                        break;
                    case Direction.Right:
                        Size = new Size(Width + (e.X - currentDragPos.X), Height);
                        break;
                    case Direction.UpL:
                        Size = new Size(Width - (e.X - eOriginalPos.X), Height - (e.Y - eOriginalPos.Y));
                        location.Offset(e.Location.X - eOriginalPos.X, e.Location.Y - eOriginalPos.Y);
                        break;
                    case Direction.UpR:
                        Size = new Size(Width + (e.X - currentDragPos.X), Height - (e.Y - eOriginalPos.Y));
                        location.Offset(0, e.Location.Y - eOriginalPos.Y);
                        break;
                    case Direction.DownR:
                        Size = new Size(Width + (e.X - currentDragPos.X), Height + (e.Y - currentDragPos.Y));
                        break;
                    case Direction.DownL:
                        Size = new Size(Width - (e.X - eOriginalPos.X), Height + (e.Y - currentDragPos.Y));
                        location.Offset(e.Location.X - eOriginalPos.X, 0);
                        break;
                }
                
                Location = location;
                currentDragPos = e.Location;
            } else if (!IsOnGrip(e.Location)) {
                Cursor = Cursors.Default;
            }
            
            base.OnMouseMove(e);
        }
    }
}

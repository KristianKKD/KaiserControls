using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kaiser {
    public class SizeablePanel : Panel {
        private const int cGripSize = 5;
        public bool resizing = false;
        bool dragging = false;

        private Point currentDragPos;
        private Point eOriginalPos;
        private Point dragPos;

        public enum Direction {
            Up,
            Down,
            Left,
            Right,
            UpR,
            UpL,
            DownR,
            DownL,
            None,
        }

        Direction myDir;

        public SizeablePanel() {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.Black;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private bool IsOnGrip(Point pos) {
            if (pos.X >= this.ClientSize.Width - cGripSize) {       //grabbed right side
                if (pos.Y >= this.ClientSize.Height - cGripSize) {  // bottom right
                    Cursor = Cursors.SizeNWSE;
                    myDir = Direction.DownR;
                } else if (pos.Y <= cGripSize) {                    // top right
                    Cursor = Cursors.SizeNESW;
                    myDir = Direction.UpR;
                } else {                                            // right
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
            if (e.Button == MouseButtons.Left) {
                dragPos = new Point(e.X, e.Y);
                dragging = true;

                resizing = IsOnGrip(e.Location);
                currentDragPos = e.Location;
                dragPos = e.Location;
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            resizing = false;
            dragging = false;
            dragPos = new Point(0, 0);
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (dragging && !resizing) {
                int xDragDist = e.X - dragPos.X;
                int yDragDist = e.Y - dragPos.Y;

                if (Location.X + xDragDist > 0 && Location.X + xDragDist < this.Parent.Width - Width)
                    Left += xDragDist;

                if (Location.Y + yDragDist > 0 && Location.Y + yDragDist < this.Parent.Height - Height)
                    Top += yDragDist;
            }

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
                    default:
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

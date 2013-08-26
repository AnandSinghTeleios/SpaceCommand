using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AIController;

namespace AIChallenge
{
    public class Sprite: IDrawableEntity
    {
        public int drawOrder { get; set; }
        private static readonly object _lock = new object();
        private Rect rectangle;
        private Rect collisionBounds;
        private int collisionOffsetX;
        private int collisionOffsetY;
        private int collisionOffsetW;
        private int collisionOffsetH;
        private RotateTransform rotater = new RotateTransform();
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsColliding { get; set; }
        public String name;

        private BitmapFrame _Get(string name, double x, double y)
        {
            lock (_lock)
            {
                BitmapFrame frame;
                using (var stream = new System.IO.FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    frame = decoder.Frames[0];                    
                }

                Width = frame.PixelWidth;
                Height = frame.PixelHeight;
                rectangle = new Rect(x, y, Width, Height);
                collisionBounds = new Rect(x, y, Width, Height);
                return frame;
            }
        }

        private bool alive = true;
        public bool Alive
        {
            set
            {
                alive = value;
            }

            get
            {
                return alive;
            }
        }

        public void SetCollisionOffset(int x, int y, int w, int h)
        {
            collisionOffsetX = x;
            collisionOffsetY = y;
            collisionOffsetW = w;
            collisionOffsetH = h;
        }

        public double GetAngle()
        {
            return rotater.Angle;
        }

        public void SetAngle(double a)
        {
            rotater.Angle = a;
        }

        public void UpdateAngle(double a)
        {
            rotater.Angle += a;
        }

        public double GetX()
        {
            return rectangle.X;
        }

        public double GetY()
        {
            return rectangle.Y;
        }

        public void SetX(double x)
        {
            rectangle.X = x;
        }

        public void SetY(double y)
        {
            rectangle.Y = y;
        }

        public void SetPosition(double x, double y)
        {
            rectangle.X = x;
            rectangle.Y = y;
        }

        public void UpdatePosition(double x, double y)
        {
            rectangle.X += x;
            rectangle.Y += y;
        }

        public Rect GetBounds()
        {
            collisionBounds.X = rectangle.X + collisionOffsetX;
            collisionBounds.Y = rectangle.Y + collisionOffsetY;
            collisionBounds.Width = rectangle.Width - collisionOffsetW;
            collisionBounds.Height = rectangle.Height - collisionOffsetH;
            return collisionBounds;
        }

        public bool CollidesWith(Sprite other)
        {
            collisionBounds.X = rectangle.X + collisionOffsetX;
            collisionBounds.Y = rectangle.Y + collisionOffsetY;
            collisionBounds.Width = rectangle.Width - collisionOffsetW;
            collisionBounds.Height = rectangle.Height - collisionOffsetH;

            if (collisionBounds.IntersectsWith(other.GetBounds()))
                return true;
            return false;
        }

        public BitmapFrame Image { get; set; }

        public Sprite(string filename, double x, double y)
        {
            MainWindow._instance.Dispatcher.BeginInvoke
            (
                new Action
                (
                    () => Image = _Get(filename, x, y)
                )
            );
        }

        #region IDrawable Members

        public int DrawOrder
        {
            get { return drawOrder; }
        }

        public virtual void Update()
        {
        }

        public void Draw()
        {
            Update();
            rotater.CenterX = rectangle.X + rectangle.Width / 2;
            rotater.CenterY = rectangle.Y + rectangle.Height / 2;
            if (!Alive)
                GameControl._drawingContext.PushOpacity(0.3);
            GameControl._drawingContext.PushTransform(rotater);
            GameControl._drawingContext.DrawImage(Image, rectangle);
            GameControl._drawingContext.Pop();
            if (!Alive)
                GameControl._drawingContext.Pop();
        }

        #endregion

        public String GetName()
        {
            return name;
        }
    }
}

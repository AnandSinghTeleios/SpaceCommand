using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Media;
using AIController;

namespace AIChallenge
{
	public class Background:
		IDrawable
	{
		private static readonly object _lock = new object();
		private static BitmapFrame _background;
		private static string _loadedName;

		internal static Background Instance;

		private static BitmapFrame _Get(string name)
		{
			lock(_lock)
			{
				if (_background == null || _loadedName != name)
				{
					using(var stream = new System.IO.FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
						_background = decoder.Frames[0];
					}

					_loadedName = name;
				}

				return _background;
			}
		}

        public Background(string mapName)
		{
			Instance = this;
            if (mapName == null)
                throw new ApplicationException("Background MapName was not received.");

            MainWindow._instance.Dispatcher.BeginInvoke
            (
                new Action
                (
                    () => Image = _Get(mapName)
                )
            );
		}

		public int DrawOrder
		{
			get
			{
				return -100;
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

		public BitmapFrame Image { get; private set; }

		public void Draw()
		{
			var rectangle = new Rect(0, 0, Image.PixelWidth, Image.PixelHeight);
            GameControl._drawingContext.DrawImage(Image, rectangle);
		}
	}
}

using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Pfz.Drawing.Wpf;
using System;
using AIController;

namespace AIChallenge
{
	public class Foreground:
		IDrawable
	{
		private static readonly object _lock = new object();
		private static WriteableBitmap _bitmap;
		private static string _loadedName;
		private static WriteableBitmap _Get(string name)
		{
			lock(_lock)
			{
				if (name != _loadedName)
				{
					ManagedBitmap managedBitmap;

                    using (var stream = new System.IO.FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        var frame = decoder.Frames[0];
                        managedBitmap = new ManagedBitmap(frame);
                    }
                    
					using(var lockedBitmap = managedBitmap.Lock())
					{
						int height = lockedBitmap.Height;
						int width = lockedBitmap.Width;
						for(int y=0; y<height; y++)
						{
							for(int x=0; x<width; x++)
							{
								var color = lockedBitmap[x, y];
								if (color.Red < 10 && color.Green < 10 && color.Blue < 10)
									lockedBitmap[x, y] = new Argb();
							}
						}
					}

					_bitmap = managedBitmap.WriteableBitmap;
					_loadedName = name;
				}

				return _bitmap;
			}
		}

		public int DrawOrder
		{
			get
			{
				return 100;
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

        public Foreground(string mapName)
        {
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

		public WriteableBitmap Image { get; private set; }

		public void Draw()
		{
			var rectangle = new Rect(0, 0, Image.PixelWidth, Image.PixelHeight);
			GameControl._drawingContext.DrawImage(Image, rectangle);
		}
	}
}

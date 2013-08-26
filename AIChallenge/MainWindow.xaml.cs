using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AIController;
using Pfz;
using Pfz.Extensions;
using Pfz.RemoteGaming;
using System.Windows.Media.Imaging;
using System.IO;

namespace AIChallenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow _instance;

        public List<IDrawable> AddedComponents;
        public List<IDrawable> RemovedComponents;

        public int WindowXOffset { get; set; }
        public int WindowYOffset { get; set; }
        public int WindowWidthOffset { get; set; }
        public int WindowHeightOffset { get; set; }

        public GameProperties GameProps { get; set; }

        public BitmapFrame DisabledImage { get; set; }
        private static readonly object _lock = new object();

        public HUD Hud { get; set; }
        public List<ShipCommander> CommanderList { get; set; }

        public MainWindow()
        {
            _instance = this;
            InitializeComponent();
            
            ResizeMode = ResizeMode.NoResize;

            WindowXOffset = 50;
            WindowYOffset = 50;
            WindowWidthOffset = 800 - 50;
            WindowHeightOffset = 600 - 60;

            AddedComponents = new List<IDrawable>();
            RemovedComponents = new List<IDrawable>();
            CommanderList = new List<ShipCommander>();

            GameProps = new GameProperties();
            GameProps.NumShips = 2;
            GameProps.GameTime = 5000;

            AddedComponents.Add(new AIChallenge.Background("Data\\Maps\\BigMap\\background.jpg"));
            
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Tick);
            dispatcherTimer.Interval = new TimeSpan(166666);
            dispatcherTimer.Start();

            String dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            ShipCommander commander = new ShipCommander(dir + @"\SpaceCommand\bin\Debug\SpaceCommand.dll", 100, 300);     
            ShipCommander commander2 = new ShipCommander(dir +@"\AIChallenge\DLLs\TestShip.dll", 400 - 33, 300 - 20);

            CommanderList.Add(commander);
            CommanderList.Add(commander2);

            Hud = new HUD();
            AddedComponents.Add(Hud);

            LoadAllImages();
        }

        private void LoadAllImages()
        {
            Dispatcher.BeginInvoke
            (
                new Action
                (
                    () => DisabledImage = LoadImage("Data\\Ships\\shipkill.png")
                )
            );
        }

        public BitmapFrame LoadImage(String image)
        {
            lock (_lock)
            {
                BitmapFrame frame;
                using (var stream = new System.IO.FileStream(image, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    frame = decoder.Frames[0];
                }

                return frame;
            }
        }
               
        private void Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke
            (
                new Action
                (
                    () =>
                    {
                        UpdateGame();
                    }
                )
            );
        }

        private Dictionary<IWpfControl, UserControl> _wpfControls = new Dictionary<IWpfControl, UserControl>();
        public void UpdateGame()
        {
            if (!GameProps.GameOver)
            {
                GameProps.ElapsedTime++;
                if (GameProps.ElapsedTime >= GameProps.GameTime)
                {
                    GameProps.ElapsedTime = 0;
                    GameProps.GameOver = true;
                    Hud.UpdateResults();
                }
            }

            if (AddedComponents.Count>0 || RemovedComponents.Count>0)
            {
                Dispatcher.Invoke
                (
                    new Action
                    (
                        () =>
                        {
                            if (RemovedComponents != null)
                            {
                                foreach (var component in RemovedComponents)
                                {
                                    var drawable = component as IDrawable;
                                    if (drawable != null)
                                        GameControl._drawables.Remove(drawable);

                                    var wpfControl = component as IWpfControl;
                                    if (wpfControl != null)
                                    {
                                        var userControl = _wpfControls.GetValueOrDefault(wpfControl);
                                        if (userControl != null)
                                        {
                                            _wpfControls.Remove(wpfControl);
                                            sameAreaPanel.Children.Remove(userControl);
                                        }
                                    }
                                }
                                RemovedComponents.Clear();
                            }

                            if (AddedComponents != null)
                            {
                                bool mustSort = false;
                                foreach (var component in AddedComponents)
                                {
                                    var drawable = component as IDrawable;
                                    if (drawable != null)
                                    {
                                        GameControl._drawables.Add(drawable);
                                        mustSort = true;
                                    }

                                    var wpfControl = component as IWpfControl;
                                    if (wpfControl != null && wpfControl.CanShow)
                                    {
                                        var userControl = new UserControl();
                                        userControl.Content = wpfControl;
                                        _wpfControls.Add(wpfControl, userControl);
                                        sameAreaPanel.Children.Add(userControl);
                                    }
                                }
                                AddedComponents.Clear();

                                if (mustSort)
                                    GameControl._drawables.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
                            }
                        }
                    )
                );
            }

            Dispatcher.BeginInvoke
            (
                new Action
                (
                    () =>
                    {
                        gameControl.InvalidateVisual();
                    }
                )
            );
        }
    }
}

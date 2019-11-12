using System;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DVD
{
    public partial class MainWindow : Window
    {
        public Point Location
        {
            get { return new Point(Left, Top); }
            set
            {
                //if (value == null) return;
                Location = value;
                Left = value.X;
                Top = value.Y;
            }
        }
        public Size Size
        {
            get { return new Size(Width, Height); }
            set
            {
                //if (value == null) return;
                Size = value;
                Width = value.Width;
                Height = value.Height;
            }
        }
        public Point MiddlePoint
        {
            get { return new Point(this.Width / 2, this.Height / 2); }
        }

        public Size ScreenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;

        static string path = Environment.CurrentDirectory;
        static Bitmap[] imgs = new Bitmap[]
        {
            Properties.Resources._1,
            Properties.Resources._2,
            Properties.Resources._3
        };

        enum MovingMode
        {
            Follow,
            Rebound,
            Stop
        }

        long t = 0;
        Vector direction = new Vector(5, 5);
        MovingMode mode = MovingMode.Rebound;
        bool dragging = false;
        
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();

        System.Windows.Forms.NotifyIcon icon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
        System.Windows.Forms.MenuItem setMode = new System.Windows.Forms.MenuItem();
        System.Windows.Forms.MenuItem onQuit = new System.Windows.Forms.MenuItem();
        System.Windows.Forms.MenuItem author = new System.Windows.Forms.MenuItem();

        public MainWindow()
        {
            InitializeComponent();

            Action switchMode = () =>
            {
                timer2.Stop();
                switch (mode)
                {
                    case MovingMode.Rebound:
                        mode = MovingMode.Follow;
                        setMode.Text = "跟随";
                        break;
                    case MovingMode.Follow:
                        mode = MovingMode.Stop;
                        setMode.Text = "停止";
                        break;
                    case MovingMode.Stop:
                        mode = MovingMode.Rebound;
                        setMode.Text = "反弹";
                        break;
                }
                Animation();
            };

            icon.DoubleClick += (object sender, EventArgs e) => switchMode();
            setMode.Text = "反弹";
            setMode.Click += (object sender, EventArgs e) => switchMode();
            author.Text = "作者";
            author.Click += (object sender, EventArgs e) => new author().Show();
            onQuit.Text = "退出";
            onQuit.Click += (object sender, EventArgs e) =>
            {
                icon.Visible = false;
                icon.Dispose();
                Environment.Exit(1);
            };

            menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { author, onQuit });
            icon.Icon = Properties.Resources.disc;
            icon.ContextMenu = menu;
            icon.Visible = true;
            ShowInTaskbar = false;

            //KeyDown += (object sender, KeyEventArgs e) => { if (e.Key == Key.Space) dragging = true; };
            //KeyUp += (object sender, KeyEventArgs e) => dragging = false;
            MouseDown += (object sender, MouseButtonEventArgs e) => DragMove();//{ if (dragging) DragMove(); };

            /*
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer1.Tick += (object sender, EventArgs e) => picChange();
            timer1.Start();
            */
            Animation();
        }

        public void Animation()
        {
            BitmapImage[] images = new BitmapImage[]
            {
                BitmapToBitmapImage(imgs[0]),
                BitmapToBitmapImage(imgs[1]),
                BitmapToBitmapImage(imgs[2])
            };
                
            Action picChange = () =>
            {
                pic.Source = images[t];
                t++;
                if (t == imgs.Length) t = 0;
            };
            Action picMove = () =>
            {
                switch (mode)
                {
                    case MovingMode.Follow:
                        //picFollow
                        Point pt = null;
                        Point p = (new Vector(LineSeg(10, MousePosition() - MiddlePoint, MiddlePoint)) / 100).ToPoint();
                        pt = PointToScreen(p);
                        Left = pt.X;
                        Top = pt.Y;
                        break;
                    case MovingMode.Rebound:
                        //picRebound
                        Left += direction.X;
                        Top += direction.Y;
                        if (Left <= 0 || Left + Width >= ScreenSize.Width)
                        {
                            direction.X *= -1;
                            picChange();
                        }
                        if (Top <= 0 || Top + Height >= ScreenSize.Height)
                        {
                            direction.Y *= -1;
                            picChange();
                        }
                        break;
                    case MovingMode.Stop:
                        timer2.Stop();
                        break;
                }
            };

            timer2.Interval = new TimeSpan(0, 0, 0, 0, 15);
            timer2.Tick += (object sender, EventArgs e) => picMove();
            timer2.Start();
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public Point LineSeg(double d, Point p1, Point p2)
        {
            Vector OA = new Vector(p1), OB = new Vector(p2);
            Vector AB = OB - OA;
            if (AB.Norm() == 0) return p1;
            double lambda = d / AB.Norm();
            return (OA + lambda * AB).ToPoint();
        }

        public Point MousePosition()
        {
            Point pos = System.Windows.Forms.Cursor.Position;
            return this.PointFromScreen(pos);
        }
    }
}
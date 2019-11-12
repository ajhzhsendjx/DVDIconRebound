using System.Windows;
using System.Windows.Media.Imaging;

namespace DVD
{
    /// <summary>
    /// author.xaml 的交互逻辑
    /// </summary>
    public partial class author : Window
    {
        public author()
        {
            InitializeComponent();
            pic.Source = BitmapToBitmapImage(Properties.Resources.discpic);
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
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
    }
}
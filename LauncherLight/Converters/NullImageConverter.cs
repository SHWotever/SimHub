using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LauncherLight.Converters
{

    public class NullImageConverter :MarkupExtension, IValueConverter
    {

        public string Hash(string temp)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(temp));
                return System.Convert.ToBase64String(hash).Replace("=","") ;
            }
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                
                    return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(fileName))).Replace("-", string.Empty);
                
            }
        }

        public NullImageConverter()
        {
            TargetWidth = 240;
            TargetHeight = 120;
        }

        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (!string.IsNullOrEmpty(value.ToString()))
            {

                string newPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Thumbnails", GetMD5HashFromFile(string.Format("{0},{1},{2}", value.ToString().ToLower(), TargetWidth, TargetHeight)) + ".png");

                if (!File.Exists(newPath))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(value.ToString());
                    bi.EndInit();
                    var result = CreateResizedImage(bi, TargetWidth, TargetHeight, 0);

                    Save(result, newPath);

                    return result;
                }
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(newPath.ToString());
                    //bi.DecodePixelHeight = 480;
                    //bi.DecodePixelWidth = 960;
                    bi.EndInit();
                    return bi;
                }
                //try
                //{
                //    BitmapImage bi = new BitmapImage();
                //    bi.BeginInit();
                //    bi.UriSource = new Uri(value.ToString());
                //    bi.DecodePixelHeight = 240;
                //    bi.DecodePixelWidth = 480;
                //    bi.CacheOption = BitmapCacheOption.OnLoad;
                //    bi.EndInit();
                //    return bi;//CreateResizedImage(bi, 240, 120, 0);
                //}
                //catch
                //{

                //}



                //return bi;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public static Size CalculateResizeToFit(Size imageSize, Size boxSize)
        {
            // TODO: Check for arguments (for null and <=0)
            var widthScale = boxSize.Width / (double)imageSize.Width;
            var heightScale = boxSize.Height / (double)imageSize.Height;
            var scale = Math.Min(widthScale, heightScale);
            return new Size(
                (int)Math.Round((imageSize.Width * scale)),
                (int)Math.Round((imageSize.Height * scale))
                );
        }


        private static BitmapFrame CreateResizedImage(ImageSource source, int width, int height, int margin)
        {

            var newSize = CalculateResizeToFit(new Size(source.Width, source.Height), new Size(width, height));

            width = (int)newSize.Width;
            height = (int)newSize.Height;

            var rect = new Rect(margin, margin, width - margin * 2, height - margin * 2);

            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);

            var resizedImage = new RenderTargetBitmap(
                width, height,         // Resized dimensions
                96, 96,                // Default DPI values
                PixelFormats.Default); // Default pixel format
            resizedImage.Render(drawingVisual);

            return BitmapFrame.Create(resizedImage);
        }

        public void Save(BitmapFrame frame, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file))) {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);
                encoder.Save(fileStream);
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


}

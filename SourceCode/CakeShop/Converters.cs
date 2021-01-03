using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CakeShop
{
    /// <summary>
    /// Chuyển đổi đường dẫn tuyệt đối sang tương đối
    /// </summary>
    class AbsolutePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relative = value.ToString();
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = $"{folder}{relative}";
            return absolutePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Chuyển đổi mảng byte sang bitmap Codebehind
    /// </summary>
    public class AlternativeByteArrayToImageConveter
    {
        public static BitmapImage Convert(byte[] imageArray)
        {
            using (var stream = new MemoryStream(imageArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }

    /// <summary>
    /// Chuyển đổi mảng byte sang bitmap UI
    /// </summary>
    public class ByteArrayToImageConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Byte[] array = value as Byte[];

            using (var stream = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
            // Nếu ảnh null
            //else
            //{
            //    var path = AppDomain.CurrentDomain.BaseDirectory;
            //    var image = new BitmapImage();

            //    image.BeginInit();
            //    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            //    image.CacheOption = BitmapCacheOption.OnLoad;
            //    image.UriSource = new Uri(path + @"Data\Images\cart.png");
            //    image.EndInit();
            //    image.Freeze();

            //    return image;

            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnknownImageToByteArrayConverter
    {
        public static byte[] ImageToByteArray(string sourcePath)
        {
            var result = new byte[] { };
            var image = new BitmapImage(
               new Uri(sourcePath, UriKind.Absolute)
                   );
            var encoder = CheckEncoder(sourcePath);
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                result = stream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static BitmapEncoder CheckEncoder(string filepath)
        {
            BitmapEncoder result = null;
            var img = System.Drawing.Image.FromFile(filepath);

            // Nếu là file chuẩn JEPG
            if (IsJEPGImage(img) == true)
            {
                result = new JpegBitmapEncoder();
            }
            // Nếu là file chuẩn PNG
            else if (IsPNGImage(img) == true)
            {
                result = new PngBitmapEncoder();
            }
            // Nếu là file chuẩn BMP
            else if (IsBMPImage(img) == true)
            {
                result = new BmpBitmapEncoder();
            }
            // Nếu là file chuẩn TIFFI
            else if (IsTIFFImage(img) == true)
            {
                result = new TiffBitmapEncoder();
            }
            // Nếu là file chuẩn GIF
            else if (IsGIFImage(img) == true)
            {
                result = new GifBitmapEncoder();
            }

            return result;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại JEPG hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsJEPGImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Jpeg);
            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại BMP hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsBMPImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Bmp);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại PNG hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsPNGImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Png);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại TIFFI hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsTIFFImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Tiff);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại GIF hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsGIFImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Gif);

            return check;
        }
    }

}

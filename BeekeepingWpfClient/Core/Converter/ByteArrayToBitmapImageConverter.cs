using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BeekeepingWpfClient.Core.Converter;

public class ByteArrayToBitmapImageConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return new BitmapImage();

        using var ms = new MemoryStream((value as byte[])!);
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = ms;
        image.EndInit();

        return image;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        ToByteArray((value as BitmapImage)!);

    public static byte[]? ToByteArray(BitmapImage? image)
    {
        if (image is null) return null;

        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using var ms = new MemoryStream();
        encoder.Save(ms);

        return ms.ToArray();
    }
}

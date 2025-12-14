using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.Views
{
    public class ImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string filename && !string.IsNullOrEmpty(filename))
            {
                try
                {
                    // Fontos: Az "avares://" után a te Assembly nevedet kell írni
                    var uri = new Uri($"avares://ChameleonGame/Assets/{filename}");
                    return new Bitmap(AssetLoader.Open(uri));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Helpers
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && (parameter is string))
            {
                var color = (string)parameter;
                switch (color)
                {
                    case "Work":
                        return Application.Current.Resources["GreenLightColor"];
                    case "Faulty":
                        return Application.Current.Resources["AlertColor"];
                    case "Orange":
                        return Application.Current.Resources["OrangeColor"];
                    //return Color.FromHex("c2bca8");
                    //case "text":
                    //    return Application.Current.Resources["ThemeBlue"];
                    //return Color.FromHex("96907e");
                    default:
                        return Color.Default;
                }
            }
            else
                return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

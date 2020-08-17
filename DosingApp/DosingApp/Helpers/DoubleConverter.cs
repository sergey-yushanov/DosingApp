using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Helpers
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
                return null;

            double dbl;
            if (double.TryParse(stringValue, out dbl))
            {
                if (dbl == 0)
                {
                    return null;
                }

                return dbl;
            }
            return null;
        }
    }
}

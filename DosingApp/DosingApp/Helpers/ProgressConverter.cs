using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Helpers
{
    public class ProgressConverter : IValueConverter
    {
        // Current / Total → 0..1
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int current && parameter is int total && total > 0)
                return (double)current / total;
            return 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

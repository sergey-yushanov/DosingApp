using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Controls
{
    public class PdfView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(string), typeof(PdfView));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}

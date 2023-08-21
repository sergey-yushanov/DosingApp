using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfJsPage : ContentPage
    {
        public PdfJsPage(string url)
        {
            InitializeComponent();

            PdfJsWebView.Uri = url;
        }
    }
}
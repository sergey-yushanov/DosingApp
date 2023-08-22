using DosingApp.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class PdfDocumentView : ContentPage
    {
        private readonly string filePath;

        public PdfDocumentView(string title, string filePath)
        {
            InitializeComponent();
            labelTitle.Text = title;
            this.filePath = filePath;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetBusyIndicator(true);

            PdfDocView.Uri = filePath;

            SetBusyIndicator(false);
        }

        private void SetBusyIndicator(bool isBusyIndicatorIsVisible) => BusyIndicator.IsRunning = BusyIndicator.IsVisible = isBusyIndicatorIsVisible;
    }
}
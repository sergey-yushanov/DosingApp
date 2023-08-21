﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleDriveViewerPage : ContentPage
    {
        public GoogleDriveViewerPage(string url)
        {
            InitializeComponent();

            GoogleDriveViewerWebView.Uri = url;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Controls
{
    public class GoogleDriveViewerWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(string), typeof(GoogleDriveViewerWebView), default(string));

        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }
    }
}

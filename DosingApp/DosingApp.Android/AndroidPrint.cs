using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DosingApp.Droid;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidPrint))]
namespace DosingApp.Droid
{
    public class AndroidPrint : IPrint
    {
        public bool Print(Stream file)
        {
            throw new NotImplementedException();
        }
    }
}
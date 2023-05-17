using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DosingApp.Droid;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidLogPath))]
namespace DosingApp.Droid
{
    public class AndroidLogPath : ILogPath
    {
        public string GetActualPath()
        {
            return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        }
    }
}
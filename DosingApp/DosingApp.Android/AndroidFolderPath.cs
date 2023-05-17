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
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidFolderPath))]
namespace DosingApp.Droid
{
    class AndroidFolderPath : IAccessFolder
    {
        [Obsolete]
        public string GetFolderPath(string folderName)
        {
            string downloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);
            //string downloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "ru.ghills.mixapp");
            string folderPath = Path.Combine(downloadsPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
    }
}
using Android.App;
using Android.Content;
using Android.Content.PM;
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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAppHandler))]
namespace DosingApp.Droid
{
    public class AndroidAppHandler : IAppHandler
    {
        public Task<bool> LaunchApp(string appPackageName, string filePath)
        {
            var result = false;

            try
            {
                var pm = Android.App.Application.Context.PackageManager;

                if (pm != null && IsAppInstalled(appPackageName))
                {
                    var intent = pm.GetLaunchIntentForPackage(appPackageName);
                    if (intent != null)
                    {
                        //intent.SetDataAndType(Uri(uri),  .setDataAndType(Uri. .Fr .fromFile(file), "text/*");
                        //Context context = Android.App.Application.Context;

                        //Android.Net.Uri uri = Android.Net.Uri.Parse(filePath);

                        //intent.SetData(uri);
                        //intent.SetType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        intent.SetFlags(ActivityFlags.NewTask);
                        Android.App.Application.Context.StartActivity(intent);
                        result = true;
                    }
                }
            }
            catch
            {
                // something went wrong...
            }

            return Task.FromResult(result);
        }

        private static bool IsAppInstalled(string packageName)
        {
            var installed = false;

            try
            {
                var pm = Android.App.Application.Context.PackageManager;

                if (pm != null)
                {
                    pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                    installed = true;
                }
            }
            catch
            {
                // something went wrong...
            }

            return installed;
        }
    }
}
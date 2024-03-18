using Android.Content;
using DosingApp.Droid;
using DosingApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidFilePath))]
namespace DosingApp.Droid
{
    public class AndroidFilePath : IActualPath
    {
        [System.Obsolete]
        public string GetActualPathFromUri(string uriString)
        {
            Context context = Android.App.Application.Context;
            Android.Net.Uri uri = Android.Net.Uri.Parse(uriString);

            return Helpers.FilesHelper.GetActualPathForFile(uri, context);
        }
    }
}
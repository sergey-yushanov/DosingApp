using DosingApp.Services;
using DosingApp.UWP;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(UwpUserDbPath))]
namespace DosingApp.UWP
{
    public class UwpUserDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}

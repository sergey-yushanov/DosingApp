using DosingApp.Services;
using DosingApp.UWP;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(UwpDbPath))]
namespace DosingApp.UWP
{
    public class UwpDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
            //return Path.Combine("C:\\Users\\yushanov\\source\\repos\\", filename);
        }
    }
}

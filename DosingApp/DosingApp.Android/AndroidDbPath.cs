using System.IO;
using DosingApp.Droid;
using DosingApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace DosingApp.Droid
{
    public class AndroidDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
        }
    }
}
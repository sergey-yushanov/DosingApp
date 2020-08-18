using DosingApp.Services;
using DosingApp.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(UwpFilePath))]
namespace DosingApp.UWP
{
    public class UwpFilePath : IActualPath
    {
        public string GetActualPathFromUri(string uriString)
        {
            return uriString;
        }
    }
}
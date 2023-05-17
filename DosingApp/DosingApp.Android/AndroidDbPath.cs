using System;
using System.IO;
using System.Threading.Tasks;
using DosingApp.Droid;
using DosingApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace DosingApp.Droid
{
    public class AndroidDbPath : IPath
    {
        [Obsolete]
        public string GetDatabasePath(string filename)
        {
            //return Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, filename);
            //return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);

            string dbPath = string.Empty;


            //string folderPath2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //string folderPath3 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            //string folderPath4 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            //string folderPath5 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //string folderPath6 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Resources);

            //var folder = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "MyAppDirectory");
            //System.IO.Directory.CreateDirectory(folder);

            var folderPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            var tempFolderPath = Path.Combine(folderPath, "db");
            
            //var tempFolderPath2 = Path.Combine(folderPath2, "db2");
            //var tempFolderPath3 = Path.Combine(folderPath3, "db3");
            //var tempFolderPath4 = Path.Combine(folderPath4, "db4");
            //var tempFolderPath5 = Path.Combine(folderPath5, "db5");
            //var tempFolderPath6 = Path.Combine(folderPath6, "db6");

            //CheckPath(tempFolderPath2, "db2.txt");
            //CheckPath(tempFolderPath3, "db3.txt");
            ////CheckPath(tempFolderPath4, "db4.txt");
            //CheckPath(tempFolderPath5, "db5.txt");
            //CheckPath(tempFolderPath6, "db6.txt");


            //if (!Directory.Exists(dir))
            //    Directory.CreateDirectory(dir);

            //if (PermissonManager.GetPermission(PermissonManager.PermissionsIdentifier.Storage))
            //{ 
            //string systemPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.RootDirectory.Path).Path;

            //string tempFolderPath = Path.Combine(systemPath, "mixapp");
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }
            dbPath = Path.Combine(tempFolderPath, filename);
            //}

            return dbPath;
        }


        private void CheckPath(string tempFolderPath, string filename)
        {
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
                using (StreamWriter writer = new StreamWriter(Path.Combine(tempFolderPath, filename), true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                }
                //File.CreateText(db);
            }
            Console.WriteLine(filename);
            string[] fileEntries = Directory.GetFiles(tempFolderPath);
            foreach (string fileName in fileEntries)
                Console.WriteLine(fileName);
        }
    }
}
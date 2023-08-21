using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Utils;
using DosingApp.ViewModels;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp
{
    public partial class App : Application
    {
        public const string DBFILENAME = "dosingapp.db";
        public const string USERDBFILENAME = "dosinguser.db";

        public const string SOURCEREPORTFILENAME = "Требование-накладная М-11 (шаблон).xlsx";
        public const string DESTREPORTFILENAME = "Требование-накладная М-11.xlsx";
        public const string PDFREPORTFILENAME = "Требование-накладная М-11.pdf";

        public static string FolderPath { get; set; }

        public static User ActiveUser { get; set; }

        public static LogUtils Logger { get; set; }

        public App()
        {
            InitializeComponent();

            Task.Run(async () => await GetPermissionsAsync()).Wait();

            FolderPath = DependencyService.Get<IAccessFolder>().GetFolderPath("MixApp");

            //string text = "yyy";
            //byte[] data = Encoding.ASCII.GetBytes(text);
            //File.WriteAllBytes(filePath, data);

            //string logPath = DependencyService.Get<ILogPath>().GetActualPath();
            
            Logger = new LogUtils(FolderPath);
            Logger.Log("Start App");

            //bool exists = GetContext().Database.IsSqlite();


            //GetContext().Database.EnsureDeleted();
            //var appliedMigration = GetContext().Database.GetAppliedMigrations();
            //var definedMigration = GetContext().Database.GetMigrations();

            var lastAppliedMigration = GetContext().Database.GetAppliedMigrations().LastOrDefault();
            var lastDefinedMigration = GetContext().Database.GetMigrations().LastOrDefault();
            Console.WriteLine($"Last applied migration id: {lastAppliedMigration}");
            Console.WriteLine(lastAppliedMigration == lastDefinedMigration
                ? "Database is up to date."
                : $"There are outstanding migrations. Last defined migration is: {lastDefinedMigration}");

            Logger.Log($"Last applied migration id: {lastAppliedMigration}");
            Logger.Log(lastAppliedMigration == lastDefinedMigration
                ? "Database is up to date."
                : $"There are outstanding migrations. Last defined migration is: {lastDefinedMigration}");


            GetContext().Database.Migrate();

            //GetContext().Database.EnsureDeleted();
            //GetContext().Database.EnsureCreated();

            //GetUserContext().Database.Migrate();
            //GetUserContext().Database.EnsureDeleted();
            GetUserContext().Database.EnsureCreated();

            CreateAdminUser();
            CreateWaterComponent();

            var mainViewModel = new MainViewModel();
            MainPage = new NavigationPage(new MainPage(mainViewModel));

            // show login page on app start
            MainPage.Navigation.PushPopupAsync(new LoginPage(new LoginViewModel() { MainViewModel = mainViewModel }));
        }

        public async Task GetPermissionsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await Application.Current.MainPage.DisplayAlert("Предупреждение", "Необходим доступ к документам", "Ok");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("Permissions granted");
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Console.WriteLine("Permissions denied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Permissions went wrong");
                Console.WriteLine(ex);
            }
        }

        public static string GetReportFilePath(bool cleanFile)
        {
            string destFilePath = Path.Combine(FolderPath, DESTREPORTFILENAME);

            if (cleanFile)
            {
                string sourceFilePath = Path.Combine(FolderPath, SOURCEREPORTFILENAME);
                File.Delete(destFilePath);
                File.Copy(sourceFilePath, destFilePath);
            }

            return destFilePath;
        }

        // Получение контекста БД при запуске приложения
        public static AppDbContext GetContext()
        {
            //var appDbContextFactory = new AppDbContextFactory();
            //string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            string dbPath = Path.Combine(FolderPath, DBFILENAME);
            //Console.WriteLine(dbPath);
            //return appDbContextFactory.CreateDbContext(new string[] { dbPath });
            //return new AppDbContext(dbPath);
            return new AppDbContext(dbPath);
            //return new AppDbContext();
        }

        // Получение контекста БД пользователей при запуске приложения
        public static UserDbContext GetUserContext()
        {
            var userDbContextFactory = new UserDbContextFactory();
            //string dbPath = DependencyService.Get<IPath>().GetDatabasePath(USERDBFILENAME);
            string dbPath = Path.Combine(FolderPath, USERDBFILENAME);
            return userDbContextFactory.CreateDbContext(new string[] { dbPath });
        }

        // Создаем админа с пустым паролем, если его нет в базе
        protected static void CreateAdminUser()
        {
            using (UserDbContext db = GetUserContext())
            {
                if (!db.Users.Any(u => u.Username == Admin.Username))
                {
                    db.Users.Add(Admin.GetUser());
                    db.SaveChanges();
                }
            }
        }

        public static bool IsActiveUserAdmin()
        {
            return Admin.IsAdminUsername(ActiveUser.Username);
        }

        // Создаем компонент по умолчанию - вода
        protected static void CreateWaterComponent()
        {
            using (AppDbContext db = GetContext())
            {
                if (!db.Components.Any(c => c.Name == Water.Name))
                {
                    Component component = Water.GetComponent();
                    db.Components.Add(component);
                    db.SaveChanges();
                }
            }
        }

        // Получение текущей активной установки
        public static Mixer GetUsedMixer()
        {
            using (AppDbContext db = GetContext())
            {
                return db.Mixers.FirstOrDefault(m => m.IsUsedMixer);
            }
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

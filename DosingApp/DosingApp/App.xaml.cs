using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp
{
    public partial class App : Application
    {
        public const string DBFILENAME = "dosingapp.db";

        public App()
        {
            InitializeComponent();
            GetContext().Database.EnsureCreated();

            //string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            //using (var db = new AppDbContext(dbPath))
            //{
            // Создаем бд, если она отсутствует
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            //}
            MainPage = new NavigationPage(new MainPage());
        }

        // Получение контекста БД при запуске приложения
        public static AppDbContext GetContext()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            return new AppDbContext(dbPath);
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

using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp
{
    public partial class App : Application
    {
        public const string DBFILENAME = "dosingapp.db";
        public const string USERDBFILENAME = "dosinguser.db";

        public static User ActiveUser { get; set; }

        public App()
        {
            InitializeComponent();

            //GetContext().Database.Migrate();
            //GetContext().Database.EnsureDeleted();
            GetContext().Database.EnsureCreated();

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

        // Получение контекста БД при запуске приложения
        public static AppDbContext GetContext()
        {
            var appDbContextFactory = new AppDbContextFactory();
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            return appDbContextFactory.CreateDbContext(new string[] { dbPath });
        }

        // Получение контекста БД пользователей при запуске приложения
        public static UserDbContext GetUserContext()
        {
            var userDbContextFactory = new UserDbContextFactory();
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(USERDBFILENAME);
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

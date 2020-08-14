using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
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

        public const string AdminName = "admin";

        private static User activeUser;
        public static User ActiveUser
        {
            get { return activeUser; }
            set { activeUser = value; }
        }

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

            MainPage = new NavigationPage(new LoginPage());
        }

        // Получение контекста БД при запуске приложения
        public static AppDbContext GetContext()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            return new AppDbContext(dbPath);
        }

        // Получение контекста БД пользователей при запуске приложения
        public static UserDbContext GetUserContext()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(USERDBFILENAME);
            return new UserDbContext(dbPath);
        }

        // Создаем админа с пустым паролем, если его нет в базе
        protected static void CreateAdminUser()
        {
            using (UserDbContext db = GetUserContext())
            {
                if (!db.Users.Any(u => u.Username == AdminName))
                {
                    User user = new User();
                    user.Username = AdminName;
                    user.DisplayName = AdminName;
                    user.PasswordSalt = CryptoService.GenerateSalt();
                    user.PasswordHash = CryptoService.ComputeHash(AdminName, user.PasswordSalt);
                    user.AccessMainMenu = true;
                    user.AccessMainParams = true;
                    user.AccessAdditionalParams = true;
                    user.AccessAdminParams = true;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }

        // Создаем компонент по умолчанию - вода
        protected static void CreateWaterComponent()
        {
            using (AppDbContext db = GetContext())
            {
                if (!db.Components.Any(c => c.Name == Water.Name))
                {
                    Component component = new Component();
                    component.Name = Water.Name;
                    component.Consistency = Water.Consistency;
                    component.Density = Water.Density;
                    db.Components.Add(component);
                    db.SaveChanges();
                }
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

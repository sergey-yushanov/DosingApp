using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        MainViewModel mainViewModel;
        private string username;
        private string password;

        public ICommand SignInCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public LoginViewModel()
        {
            App.ActiveUser = null;
            //Username = "admin";
            //Password = "admin";
            SignInCommand = new Command(SignIn);
        }
        #endregion Constructor

        #region Properties
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
            set { SetProperty(ref mainViewModel, value); }
        }

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public bool IsValidPassword
        {
            get
            {
                return (!String.IsNullOrEmpty(Password));
            }
        }
        #endregion Properties

        #region Commands
        private void SignIn()
        {
            using (UserDbContext db = App.GetUserContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Username == Username);

                if (user != null)
                {
                    var password = IsValidPassword ? Password : "";
                    
                    if (CryptoService.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                    {
                        App.ActiveUser = user;
                        Application.Current.MainPage.Navigation.PopPopupAsync();
                        MainViewModel.SetUserAccess();
                        MainViewModel.Name = App.ActiveUser.DisplayName != null ? App.ActiveUser.DisplayName : App.ActiveUser.Username;
                        Username = null;
                        Password = null;
                    }
                    else
                    {
                        Password = null;
                        Application.Current.MainPage.DisplayAlert("Авторизация", "Пароль указан неверно", "Ok");
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Авторизация", "Пользователь с именем " + Username + " не зарегистрирован", "Ok");
                    Username = null;
                    Password = null;
                }
            }
        }
        #endregion Commands
    }
}

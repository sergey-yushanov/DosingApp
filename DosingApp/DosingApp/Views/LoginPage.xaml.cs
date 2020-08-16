using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class LoginPage : PopupPage
    {
        public MainViewModel MainViewModel { get; private set; }
        public LoginPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            MainViewModel = mainViewModel;
        }

        private void SignInButton(object sender, EventArgs e)
        {
            using (UserDbContext db = App.GetUserContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Username == userEntry.Text);

                if (user != null)
                {
                    var password = String.IsNullOrEmpty(passwordEntry.Text) ? "" : passwordEntry.Text;
                    if (CryptoService.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                    {
                        App.ActiveUser = user;
                        //Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                        Application.Current.MainPage.Navigation.PopPopupAsync();
                        MainViewModel.SetUserAccess();
                        userEntry.Text = null;
                        passwordEntry.Text = null;

                    }
                    else
                    {
                        passwordEntry.Text = null;
                        DisplayAlert("Авторизация", "Пароль указан неверно", "Ok");
                    }
                }
                else
                {
                    DisplayAlert("Авторизация", "Пользователь с именем " + userEntry.Text + " не зарегистрирован", "Ok");
                    userEntry.Text = null;
                    passwordEntry.Text = null;
                }
            }
        }
    }
}
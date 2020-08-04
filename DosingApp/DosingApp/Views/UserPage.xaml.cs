using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var user = (User)BindingContext;
            userEntry.IsReadOnly = String.Equals(user.Username, App.AdminName);
            accessMainParamsSwitch.IsEnabled = App.ActiveUser.AccessMainParams && !String.Equals(user.Username, App.AdminName);
            accessAdditionalParamsSwitch.IsEnabled = App.ActiveUser.AccessAdditionalParams && !String.Equals(user.Username, App.AdminName);
            accessAdminParamsSwitch.IsEnabled = App.ActiveUser.AccessAdminParams && !String.Equals(user.Username, App.AdminName);
            base.OnAppearing();
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            var user = (User)BindingContext;

            if (String.IsNullOrEmpty(user.Username))
            {
                DisplayAlert("Ошибка", "Имя пользователя не указано", "Ok");
                return;
            }

            if (String.IsNullOrEmpty(passwordEntry.Text))
            {
                DisplayAlert("Ошибка", "Пароль пользователя не задан", "Ok");
                return;
            }

            if (!String.IsNullOrEmpty(user.Username) && !String.IsNullOrEmpty(passwordEntry.Text))
            {
                using (UserDbContext db = App.GetUserContext())
                {
                    if (user.UserId == 0)
                    {
                        user.PasswordSalt = CryptoService.GenerateSalt();
                        user.PasswordHash = CryptoService.ComputeHash(passwordEntry.Text, user.PasswordSalt);
                        db.Users.Add(user);
                    }
                    else
                    {
                        user.PasswordSalt = CryptoService.GenerateSalt();
                        user.PasswordHash = CryptoService.ComputeHash(passwordEntry.Text, user.PasswordSalt);
                        db.Users.Update(user);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var user = (User)BindingContext;
            using (UserDbContext db = App.GetUserContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            Back();
        }
    }
}
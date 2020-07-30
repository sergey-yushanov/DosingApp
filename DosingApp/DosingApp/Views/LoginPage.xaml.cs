using DosingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void SignInButton(object sender, EventArgs e)
        {
            User user = new User(userEntry.Text, passwordEntry.Text);
            if(user.CheckInformation())
            {
                Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            else
            {
                DisplayAlert("", "Неправильное имя пользователя или пароль", "Ok");
            }
        }
    }
}
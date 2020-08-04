using DosingApp.DataContext;
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
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            using (UserDbContext db = App.GetUserContext())
            {
                itemsList.ItemsSource = db.Users.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            User selectedUser = (User)e.SelectedItem;
            if (String.Equals(selectedUser.Username, App.AdminName) && !String.Equals(App.ActiveUser.Username, App.AdminName))
            {
                return;
            }
            UserPage userPage = new UserPage();
            userPage.BindingContext = selectedUser;
            await Navigation.PushAsync(userPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            User user = new User();
            UserPage userPage = new UserPage();
            userPage.BindingContext = user;
            await Navigation.PushAsync(userPage);
        }
    }
}
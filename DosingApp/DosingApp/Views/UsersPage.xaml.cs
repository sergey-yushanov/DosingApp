using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

        protected override void OnAppearing()
        {
            var usersViewModel = (UsersViewModel)BindingContext;
            usersViewModel.LoadUsers();
            base.OnAppearing();
        }
    }
}
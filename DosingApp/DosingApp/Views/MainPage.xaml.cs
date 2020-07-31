using DosingApp.DataContext;
using DosingApp.Services;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            using (UserDbContext db = App.GetUserContext())
            {
                userLabel.Text = App.ActiveUser.DisplayName;
            }
            base.OnAppearing();
        }
    }
}

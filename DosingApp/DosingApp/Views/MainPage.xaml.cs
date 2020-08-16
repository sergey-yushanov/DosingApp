using DosingApp.DataContext;
using DosingApp.Services;
using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /*protected override void OnAppearing()
        {
            var mainViewModel = (MainViewModel)BindingContext;
            mainViewModel.SetUserAccess();
            base.OnAppearing();
        }*/
    }
}

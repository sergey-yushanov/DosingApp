using DosingApp.DataContext;
using DosingApp.Services;
using DosingApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainViewModel ViewModel { get; private set; }
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
    }
}

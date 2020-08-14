using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class FacilityPage : TabbedPage
    {
        public FacilityViewModel ViewModel { get; private set; }
        public FacilityPage(FacilityViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var facilityViewModel = (FacilityViewModel)BindingContext;
            facilityViewModel.LoadFacilityTanks();
            base.OnAppearing();
        }
    }
}
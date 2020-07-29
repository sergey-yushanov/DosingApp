using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class FieldsPage : ContentPage
    {
        public FieldsPage()
        {
            InitializeComponent();
            BindingContext = new FieldsViewModel() { Navigation = this.Navigation };
        }

        protected override void OnAppearing()
        {
            FieldsViewModel fieldsViewModel = BindingContext as FieldsViewModel;
            fieldsViewModel.LoadFields();
            base.OnAppearing();
        }
    }
}
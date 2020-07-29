using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class FieldsPage : ContentPage
    {
        public FieldsPage()
        {
            InitializeComponent();
            //BindingContext = new FieldsViewModel();
        }

        protected override void OnAppearing()
        {
            FieldsViewModel fieldsViewModel = (FieldsViewModel)this.BindingContext;
            fieldsViewModel.LoadFields();
            base.OnAppearing();
        }
    }
}
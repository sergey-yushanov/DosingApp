using DosingApp.Models;
using DosingApp.ViewModels;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class FieldPage : ContentPage
    {
        public FieldViewModel FieldViewModel { get; private set; }
        public FieldPage(FieldViewModel fieldViewModel)
        {
            InitializeComponent();
            FieldViewModel = fieldViewModel;
            this.BindingContext = FieldViewModel;
        }
    }
}
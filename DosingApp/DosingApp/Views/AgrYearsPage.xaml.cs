using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class AgrYearsPage : ContentPage
    {
        public AgrYearsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
/*            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.AgrYears.ToList();
            }
            base.OnAppearing();*/
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AgrYear selectedAgrYear = (AgrYear)e.SelectedItem;
            AgrYearPage agrYearPage = new AgrYearPage();
            agrYearPage.BindingContext = selectedAgrYear;
            await Navigation.PushAsync(agrYearPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            AgrYear agrYear = new AgrYear();
            AgrYearPage agrYearPage = new AgrYearPage();
            agrYearPage.BindingContext = agrYear;
            await Navigation.PushAsync(agrYearPage);
        }
    }
}
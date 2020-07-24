using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class FacilitiesPage : ContentPage
    {
        public FacilitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Facilities.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Facility selectedFacility = (Facility)e.SelectedItem;
            FacilityPage facilityPage = new FacilityPage();
            facilityPage.BindingContext = selectedFacility;
            await Navigation.PushAsync(facilityPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Facility facility = new Facility();
            FacilityPage facilityPage = new FacilityPage();
            facilityPage.BindingContext = facility;
            await Navigation.PushAsync(facilityPage);
        }
    }
}
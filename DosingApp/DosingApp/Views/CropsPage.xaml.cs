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
    public partial class CropsPage : ContentPage
    {
        public CropsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Crops.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Crop selectedCrop = (Crop)e.SelectedItem;
            CropPage cropPage = new CropPage();
            cropPage.BindingContext = selectedCrop;
            await Navigation.PushAsync(cropPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Crop crop = new Crop();
            CropPage cropPage = new CropPage();
            cropPage.BindingContext = crop;
            await Navigation.PushAsync(cropPage);
        }
    }
}
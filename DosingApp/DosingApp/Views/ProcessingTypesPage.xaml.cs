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
    public partial class ProcessingTypesPage : ContentPage
    {
        public ProcessingTypesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.ProcessingTypes.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProcessingType selectedProcessingType = (ProcessingType)e.SelectedItem;
            ProcessingTypePage processingTypePage = new ProcessingTypePage();
            processingTypePage.BindingContext = selectedProcessingType;
            await Navigation.PushAsync(processingTypePage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            ProcessingType processingType = new ProcessingType();
            ProcessingTypePage processingTypePage = new ProcessingTypePage();
            processingTypePage.BindingContext = processingType;
            await Navigation.PushAsync(processingTypePage);
        }
    }
}
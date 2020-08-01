using DosingApp.DataContext;
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
    public partial class ApplicatorsPage : ContentPage
    {
        public ApplicatorsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Applicators.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Applicator selectedApplicator = (Applicator)e.SelectedItem;
            ApplicatorPage applicatorPage = new ApplicatorPage();
            applicatorPage.BindingContext = selectedApplicator;
            await Navigation.PushAsync(applicatorPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Applicator applicator = new Applicator();
            ApplicatorPage applicatorPage = new ApplicatorPage();
            applicatorPage.BindingContext = applicator;
            await Navigation.PushAsync(applicatorPage);
        }
    }
}
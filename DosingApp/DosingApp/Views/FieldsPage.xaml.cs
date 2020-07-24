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
    public partial class FieldsPage : ContentPage
    {
        public FieldsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Fields.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Field selectedField = (Field)e.SelectedItem;
            FieldPage fieldPage = new FieldPage();
            fieldPage.BindingContext = selectedField;
            await Navigation.PushAsync(fieldPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Field field = new Field();
            FieldPage fieldPage = new FieldPage();
            fieldPage.BindingContext = field;
            await Navigation.PushAsync(fieldPage);
        }
    }
}
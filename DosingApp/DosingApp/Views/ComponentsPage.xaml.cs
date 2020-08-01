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
    public partial class ComponentsPage : ContentPage
    {
        public ComponentsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Components.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Component selectedComponent = (Component)e.SelectedItem;
            ComponentPage componentPage = new ComponentPage();
            componentPage.BindingContext = selectedComponent;
            await Navigation.PushAsync(componentPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Component component = new Component();
            ComponentPage componentPage = new ComponentPage();
            componentPage.BindingContext = component;
            await Navigation.PushAsync(componentPage);
        }
    }
}
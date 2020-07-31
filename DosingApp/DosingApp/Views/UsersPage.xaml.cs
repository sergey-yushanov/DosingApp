using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            /*string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Transports.ToList();
            }
            base.OnAppearing();*/
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /*Transport selectedTransport = (Transport)e.SelectedItem;
            TransportPage transportPage = new TransportPage();
            transportPage.BindingContext = selectedTransport;
            await Navigation.PushAsync(transportPage);*/
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            /*Transport transport = new Transport();
            TransportPage transportPage = new TransportPage();
            transportPage.BindingContext = transport;
            await Navigation.PushAsync(transportPage);*/
        }
    }
}
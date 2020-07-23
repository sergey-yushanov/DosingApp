using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class TransportPage : ContentPage
    {
        string dbPath;

        public TransportPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            var transport = (Transport)BindingContext;
            if (!String.IsNullOrEmpty(transport.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (transport.Id == 0)
                        db.Transports.Add(transport);
                    else
                    {
                        db.Transports.Update(transport);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var transport = (Transport)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Transports.Remove(transport);
                db.SaveChanges();
            }
            Back();
        }

    }
}
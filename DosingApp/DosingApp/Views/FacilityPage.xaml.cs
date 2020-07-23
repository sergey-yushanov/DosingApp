using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class FacilityPage : ContentPage
    {
        string dbPath;

        public FacilityPage()
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
            var facility = (Facility)BindingContext;
            if (!String.IsNullOrEmpty(facility.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (facility.Id == 0)
                        db.Facilities.Add(facility);
                    else
                    {
                        db.Facilities.Update(facility);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var facility = (Facility)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Facilities.Remove(facility);
                db.SaveChanges();
            }
            Back();
        }

    }
}
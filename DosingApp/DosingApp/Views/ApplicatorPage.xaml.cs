using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class ApplicatorPage : ContentPage
    {
        string dbPath;

        public ApplicatorPage()
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
            var applicator = (Applicator)BindingContext;
            if (!String.IsNullOrEmpty(applicator.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (applicator.ApplicatorId == 0)
                        db.Applicators.Add(applicator);
                    else
                    {
                        db.Applicators.Update(applicator);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var applicator = (Applicator)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Applicators.Remove(applicator);
                db.SaveChanges();
            }
            Back();
        }

    }
}
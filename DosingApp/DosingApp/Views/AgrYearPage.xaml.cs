using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class AgrYearPage : ContentPage
    {
        string dbPath;

        public AgrYearPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        private void Back()
        {
            this.Navigation.PopAsync();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            var agrYear = (AgrYear)BindingContext;
            if (!String.IsNullOrEmpty(agrYear.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (agrYear.AgrYearId == 0)
                        db.AgrYears.Add(agrYear);
                    else
                    {
                        db.AgrYears.Update(agrYear);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var agrYear = (AgrYear)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.AgrYears.Remove(agrYear);
                db.SaveChanges();
            }
            Back();
        }
    }
}
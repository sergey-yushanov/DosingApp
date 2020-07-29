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
    public partial class CropPage : ContentPage
    {
        string dbPath;

        public CropPage()
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
            var crop = (Crop)BindingContext;
            if (!String.IsNullOrEmpty(crop.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (crop.CropId == 0)
                        db.Crops.Add(crop);
                    else
                    {
                        db.Crops.Update(crop);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var crop = (Crop)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Crops.Remove(crop);
                db.SaveChanges();
            }
            Back();
        }
    }
}
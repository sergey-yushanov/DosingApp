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
    public partial class ProcessingTypePage : ContentPage
    {
        string dbPath;

        public ProcessingTypePage()
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
            var processingType = (ProcessingType)BindingContext;
            if (!String.IsNullOrEmpty(processingType.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (processingType.Id == 0)
                        db.ProcessingTypes.Add(processingType);
                    else
                    {
                        db.ProcessingTypes.Update(processingType);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var processingType = (ProcessingType)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.ProcessingTypes.Remove(processingType);
                db.SaveChanges();
            }
            Back();
        }
    }
}
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
    public partial class FieldPage : ContentPage
    {
        string dbPath;

        public FieldPage()
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
            var field = (Field)BindingContext;
            if (!String.IsNullOrEmpty(field.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (field.FieldId == 0)
                        db.Fields.Add(field);
                    else
                    {
                        db.Fields.Update(field);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var field = (Field)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Fields.Remove(field);
                db.SaveChanges();
            }
            Back();
        }
    }
}
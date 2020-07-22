using DosingApp.Models;
using DosingApp.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FieldPage : ContentPage
    {
        string dbPath;

        public FieldPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        private void SaveField(object sender, EventArgs e)
        {
            var field = (Field)BindingContext;
            if (!String.IsNullOrEmpty(field.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (field.Id == 0)
                        db.Fields.Add(field);
                    else
                    {
                        db.Fields.Update(field);
                    }
                    db.SaveChanges();
                }
            }
            this.Navigation.PopAsync();
        }

        private void DeleteField(object sender, EventArgs e)
        {
            var field = (Field)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Fields.Remove(field);
                db.SaveChanges();
            }
            this.Navigation.PopAsync();
        }

    }
}
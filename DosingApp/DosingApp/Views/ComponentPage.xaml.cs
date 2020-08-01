using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class ComponentPage : ContentPage
    {
        string dbPath;

        public ComponentPage()
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
            var component = (Component)BindingContext;
            if (!String.IsNullOrEmpty(component.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (component.ComponentId == 0)
                        db.Components.Add(component);
                    else
                    {
                        db.Components.Update(component);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var component = (Component)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Components.Remove(component);
                db.SaveChanges();
            }
            Back();
        }

    }
}
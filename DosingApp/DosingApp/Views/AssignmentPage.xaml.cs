using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class AssignmentPage : ContentPage
    {
        public AssignmentPage()
        {
            InitializeComponent();
            
            using (AppDbContext db = App.GetContext())
            {
                recipesList.ItemsSource = db.Recipes.ToList() as List<Recipe>;
                agrYearsList.ItemsSource = db.AgrYears.ToList() as List<AgrYear>;
                fieldsList.ItemsSource = db.Fields.ToList() as List<Field>;
                facilitiesList.ItemsSource = db.Facilities.ToList() as List<Facility>;

                sourceFacilitiesList.ItemsSource = db.Facilities.ToList() as List<Facility>;
                sourceTransportsList.ItemsSource = db.Transports.ToList() as List<Transport>;
                sourceApplicatorsList.ItemsSource = db.Applicators.ToList() as List<Applicator>;

                destFacilitiesList.ItemsSource = db.Facilities.ToList() as List<Facility>;
                destTransportsList.ItemsSource = db.Transports.ToList() as List<Transport>;
                destApplicatorsList.ItemsSource = db.Applicators.ToList() as List<Applicator>;

                //sourceTypesList.ItemsSource.Add(TankType.Facility);
            }
        }

        protected override void OnAppearing()
        {
            var assignment = (Assignment)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                recipesList.SelectedIndex = (recipesList.ItemsSource as List<Recipe>).FindIndex(a => a.RecipeId == assignment.RecipeId);
                agrYearsList.SelectedIndex = (agrYearsList.ItemsSource as List<AgrYear>).FindIndex(a => a.AgrYearId == assignment.AgrYearId);
                fieldsList.SelectedIndex = (fieldsList.ItemsSource as List<Field>).FindIndex(a => a.FieldId == assignment.FieldId);

                sourceFacilitiesList.SelectedIndex = (sourceFacilitiesList.ItemsSource as List<Facility>).FindIndex(a => a.FacilityId == assignment.SourceFacilityId);
                sourceTransportsList.SelectedIndex = (sourceTransportsList.ItemsSource as List<Transport>).FindIndex(a => a.TransportId == assignment.SourceTransportId);
                sourceApplicatorsList.SelectedIndex = (sourceApplicatorsList.ItemsSource as List<Applicator>).FindIndex(a => a.ApplicatorId == assignment.SourceApplicatorId);

                destFacilitiesList.SelectedIndex = (destFacilitiesList.ItemsSource as List<Facility>).FindIndex(a => a.FacilityId == assignment.DestFacilityId);
                destTransportsList.SelectedIndex = (destTransportsList.ItemsSource as List<Transport>).FindIndex(a => a.TransportId == assignment.DestTransportId);
                destApplicatorsList.SelectedIndex = (destApplicatorsList.ItemsSource as List<Applicator>).FindIndex(a => a.ApplicatorId == assignment.DestApplicatorId);

                //SetSourceVisible();
                //SetDestVisible();
            }
            base.OnAppearing();
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            var assignment = (Assignment)BindingContext;
            if (!String.IsNullOrEmpty(assignment.Name))
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (assignment.AssignmentId == 0)
                    {
                        db.Entry(assignment).State = EntityState.Added;
                    }
                    else
                    {
                        db.Assignments.Attach(assignment);
                        db.Assignments.Update(assignment);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var assignment = (Assignment)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                db.Assignments.Attach(assignment);
                db.Assignments.Remove(assignment);
                db.SaveChanges();
            }
            Back();
        }

        private void SourceTypeChanged(object sender, EventArgs e)
        {
            var assignment = (Assignment)BindingContext;
            assignment.IsSourceFacility = sourceTypesList.SelectedIndex == 0;
            assignment.IsSourceTransport = sourceTypesList.SelectedIndex == 1;
            assignment.IsSourceApplicator = sourceTypesList.SelectedIndex == 2;
        }

        private void DestTypeChanged(object sender, EventArgs e)
        {
            var assignment = (Assignment)BindingContext;
            assignment.IsDestFacility = destTypesList.SelectedIndex == 0;
            assignment.IsDestTransport = destTypesList.SelectedIndex == 1;
            assignment.IsDestApplicator = destTypesList.SelectedIndex == 2;
        }

        /*        // добавление компонента
                private void AddComponent(object sender, EventArgs e)
                {
                    var assignment = (Assignment)BindingContext;
                    assignment.AssignmentComponents.Add(new AssignmentComponent { });
                    using (AppDbContext db = new AppDbContext(dbPath))
                    {
                        db.Assignments.Attach(assignment);
                        db.Assignments.Remove(assignment);
                        db.SaveChanges();
                    }
                    Back();
                }

                // удаление компонента
                private void RemoveComponent(object sender, EventArgs e)
                {
                    //Phone phone = phonesList.SelectedItem as Phone;
                    if (phone != null)
                    {
                        Phones.Remove(phone);
                        phonesList.SelectedItem = null;
                    }
                }*/
    }
}
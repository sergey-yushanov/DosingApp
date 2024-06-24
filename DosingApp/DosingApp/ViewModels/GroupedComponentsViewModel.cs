using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class GroupedComponentsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Manufacturer> manufacturers;
        private Manufacturer selectedManufacturer;

        public ICommand CreateManufacturerCommand { get; protected set; }
        public ICommand DeleteManufacturerCommand { get; protected set; }
        public ICommand SaveManufacturerCommand { get; protected set; }

        public ICommand EditManufacturerCommand { get; protected set; }

        public bool IsEditMode { get; protected set; }
        RecipeViewModel RecipeViewModel;
        RecipeComponentViewModel RecipeComponentViewModel;
        #endregion Attributes

        #region Constructor
        public GroupedComponentsViewModel(bool isEditMode, RecipeViewModel recipeViewModel, RecipeComponentViewModel recipeComponentViewModel)
        {
            IsEditMode = isEditMode;
            RecipeViewModel = recipeViewModel;
            RecipeComponentViewModel = recipeComponentViewModel;

            LoadManufacturers();
            CreateManufacturerCommand = new Command(CreateManufacturer);
            DeleteManufacturerCommand = new Command(DeleteManufacturer);
            SaveManufacturerCommand = new Command(SaveManufacturer);

            EditManufacturerCommand = new Command(EditManufacturer);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set { SetProperty(ref manufacturers, value); }
        }

        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                if (selectedManufacturer != value)
                {
                    if (!String.Equals(value.Name, Water.Name))
                    {
                        ComponentsViewModel tempComponentsViewModel = new ComponentsViewModel(value, IsEditMode, RecipeViewModel, RecipeComponentViewModel)
                        {
                            GroupedComponentsViewModel = this
                        };
                        selectedManufacturer = null;
                        OnPropertyChanged(nameof(SelectedManufacturer));
                        Application.Current.MainPage.Navigation.PushAsync(new ComponentsPage(tempComponentsViewModel));
                    }
                    else
                    {
                        if (IsEditMode)
                        {
                            selectedManufacturer = null;
                        }
                        else
                        { 
                            if (RecipeViewModel != null)
                            {
                                RecipeViewModel.Carrier = GetWaterComponent();
                            }
                            if (RecipeComponentViewModel != null)
                            {
                                RecipeComponentViewModel.Component = GetWaterComponent();
                            }
                            Back();
                        }
                    }
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Back2Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateManufacturer()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManufacturerPage(new ManufacturerViewModel(new Manufacturer()) { GroupedComponentsViewModel = this }));
        }

        private async void DeleteManufacturer(object manufacturerInstance)
        {
            ManufacturerViewModel manufacturerViewModel = manufacturerInstance as ManufacturerViewModel;
            if (manufacturerViewModel.Manufacturer != null && manufacturerViewModel.Manufacturer.ManufacturerId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    var componentIds = manufacturerViewModel.Components.Select(c => c.ComponentId).ToList();
                    var recipeComponents = db.RecipeComponents.Where(rc => componentIds.Contains((int)rc.ComponentId)).ToList();
                    
                    if (recipeComponents.Count > 0)
                    {
                        var recipeIds = recipeComponents.Select(rc => rc.RecipeId).Distinct().ToList();
                        var recipes = db.Recipes.Where(r => recipeIds.Contains(r.RecipeId)).ToList();
                        string recipesNames = string.Join("\n", recipes.Select(r => "- " + r.Name));
                        await Application.Current.MainPage.DisplayAlert("Предупреждение", "Невозможно удалить каталог пока компоненты из него используются в следующих рецептах:\n\n" + recipesNames, "Ok");
                        return;
                    }

                    var jobComponents = db.JobComponents.Where(jc => componentIds.Contains((int)jc.ComponentId)).ToList();
                    var jobIds = jobComponents.Select(jc => jc.JobId).Distinct().ToList();
                    var jobs = db.Jobs.Where(j => jobIds.Contains(j.JobId)).ToList();
                    db.JobComponents.RemoveRange(jobComponents);
                    db.Jobs.RemoveRange(jobs);
                    db.SaveChanges();
                }

                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Вы хотите удалить каталог вместе со всеми его компонентами?", "Да", "Нет"))
                {
                    using (AppDbContext db = App.GetContext())
                    {
                        db.Components.RemoveRange(manufacturerViewModel.Components);
                        db.Manufacturers.Remove(manufacturerViewModel.Manufacturer);
                        db.SaveChanges();
                    }
                }
            }
            Back();
        }

        private void SaveManufacturer(object manufacturerInstance)
        {
            ManufacturerViewModel manufacturerViewModel = manufacturerInstance as ManufacturerViewModel;
            if (manufacturerViewModel.Manufacturer != null)
            {
                if (!manufacturerViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название производителя", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (manufacturerViewModel.Manufacturer.ManufacturerId == 0)
                    {
                        db.Entry(manufacturerViewModel.Manufacturer).State = EntityState.Added;
                    }
                    else
                    {
                        db.Manufacturers.Update(manufacturerViewModel.Manufacturer);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void EditManufacturer(object manufacturerInstance)
        {
            Manufacturer manufacturer = manufacturerInstance as Manufacturer;
            if (manufacturer != null && manufacturer.ManufacturerId != 0)
            {
                ManufacturerViewModel manufacturerViewModel = new ManufacturerViewModel(manufacturer) { GroupedComponentsViewModel = this };
                Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
                Application.Current.MainPage.Navigation.PushAsync(new ManufacturerPage(manufacturerViewModel));
            }
        }
        #endregion Commands

        #region Methods
        public void LoadManufacturers()
        {
            using (AppDbContext db = App.GetContext())
            {
                Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.ToList());
            }
        }

        private Component GetWaterComponent()
        {
            using (AppDbContext db = App.GetContext())
            {
                return db.Components.FirstOrDefault(c => c.Name == Water.Name);
            }
        }
        #endregion Methods
    }
}

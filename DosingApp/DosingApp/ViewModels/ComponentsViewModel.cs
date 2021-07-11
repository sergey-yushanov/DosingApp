using CsvHelper;
using CsvHelper.Configuration;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.Files;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class ComponentsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Component> components;
        private Component selectedComponent;
        private string title;

        public Manufacturer Manufacturer { get; private set; }

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        public ICommand LoadFileCommand { get; protected set; }

        public bool IsEditMode { get; protected set; }
        RecipeViewModel RecipeViewModel;
        RecipeComponentViewModel RecipeComponentViewModel;
        #endregion Attributes

        #region Constructor
        public ComponentsViewModel(Manufacturer manufacturer, bool isEditMode, RecipeViewModel recipeViewModel, RecipeComponentViewModel recipeComponentViewModel)
        {
            Manufacturer = manufacturer;
            Title = "Каталог: " + Manufacturer.Name + "\nСписок компонентов";

            CreateCommand = new Command(CreateComponent);
            DeleteCommand = new Command(DeleteComponent);
            SaveCommand = new Command(SaveComponent);
            BackCommand = new Command(Back);

            LoadFileCommand = new Command(LoadFileAsync);

            IsEditMode = isEditMode;
            RecipeViewModel = recipeViewModel;
            RecipeComponentViewModel = recipeComponentViewModel;
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Component> Components
        {
            get { return components; }
            set { SetProperty(ref components, value); }
        }

        public Component SelectedComponent
        {
            get { return selectedComponent; }
            set
            {
                if (selectedComponent != value)
                {
                    if (IsEditMode)
                    {
                        ComponentViewModel tempComponent = new ComponentViewModel(value) { ComponentsViewModel = this };
                        selectedComponent = null;
                        OnPropertyChanged(nameof(SelectedComponent));
                        Application.Current.MainPage.Navigation.PushAsync(new ComponentPage(tempComponent));
                    }
                    else
                    {
                        if (RecipeViewModel != null)
                        {
                            RecipeViewModel.Carrier = value;
                        }
                        if (RecipeComponentViewModel != null)
                        {
                            RecipeComponentViewModel.Component = value;
                        }
                        Back2Pages();
                    }
                }
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Commands
        private void Back2Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void LoadFileAsync()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                string fileActualPath = DependencyService.Get<IActualPath>().GetActualPathFromUri(fileData.FilePath);
                fileActualPath = fileActualPath ?? fileData.FilePath;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";"
                };
                using (var reader = new StreamReader(fileActualPath))
                using (var csv = new CsvReader(reader, config))
                {
                    var fileComponents = csv.GetRecords<FileComponent>().ToList();

                    using (var db = App.GetContext())
                    {
                        fileComponents.ForEach(r => r.Density = r.Density.Replace(",", "."));  // change comma to point for string to double transform

                        var action = DisplayActions.Uncertain;
                        foreach (var fileComponent in fileComponents)
                        {                            
                            var existComponent = db.Components.FirstOrDefault(c => c.Name == fileComponent.Name && c.ManufacturerId == this.Manufacturer.ManufacturerId);
                            if (existComponent != null && String.Equals(action, DisplayActions.Uncertain))
                            {
                                action = await Application.Current.MainPage.DisplayActionSheet("Компонент с именем '" + fileComponent.Name + "' уже существует", null, null, DisplayActions.New, DisplayActions.Save, DisplayActions.SaveAll, DisplayActions.Skip, DisplayActions.SkipAll);
                            }

                            // skip all entire file
                            if (String.Equals(action, DisplayActions.SkipAll))
                            {
                                break;
                            }

                            // get new item values from file
                            double.TryParse(fileComponent.Density, NumberStyles.Any, CultureInfo.InvariantCulture, out double density);
                            string consistency = String.Equals(fileComponent.Consistency, ComponentConsistency.Dry) ? ComponentConsistency.Dry : ComponentConsistency.Liquid;
                            
                            // create new item
                            if (String.Equals(action, DisplayActions.New) ||
                                String.Equals(action, DisplayActions.Uncertain))
                            {
                                var newComponent = new Component()
                                {
                                    ManufacturerId = this.Manufacturer.ManufacturerId,
                                    Name = fileComponent.Name,
                                    Density = density,
                                    Consistency = consistency
                                };
                                db.Components.Add(newComponent);
                            }

                            // update item
                            if (String.Equals(action, DisplayActions.Save) ||
                                String.Equals(action, DisplayActions.SaveAll))
                            {
                                existComponent.Density = density;
                                existComponent.Consistency = consistency;
                                db.Components.Update(existComponent);
                            }
                            
                            // if we decided with only one item - next step ask again
                            if (String.Equals(action, DisplayActions.New) ||
                                String.Equals(action, DisplayActions.Save) ||
                                String.Equals(action, DisplayActions.Skip))
                            {
                                action = DisplayActions.Uncertain;
                            }
                        }
                        db.SaveChanges();
                    }
                    //LoadComponents();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ошибка работы с файлом: " + ex.ToString());
            }
        }

        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateComponent()
        {
            Component newComponent = new Component
            {
                Manufacturer = this.Manufacturer
            };
            Application.Current.MainPage.Navigation.PushAsync(new ComponentPage(new ComponentViewModel(newComponent) { ComponentsViewModel = this }));
        }

        private void DeleteComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null && componentViewModel.Component.ComponentId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Components.Remove(componentViewModel.Component);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null)
            {
                if (!componentViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название и форму компонента", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (componentViewModel.Component.ComponentId == 0)
                    {
                        db.Entry(componentViewModel.Component).State = EntityState.Added;
                    }
                    else
                    {
                        db.Components.Update(componentViewModel.Component);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadComponents()
        {
            using (AppDbContext db = App.GetContext())
            {
                var componentsDB = db.Components.Where(c => c.ManufacturerId == Manufacturer.ManufacturerId).ToList();
                Components = new ObservableCollection<Component>(componentsDB.OrderBy(c => c.Name));
            }
        }
        #endregion Methods

    }
}

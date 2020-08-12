using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DosingApp.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        #region Attributes
        RecipesViewModel recipesViewModel;
        public Recipe Recipe { get; private set; }
        private string title;

        private ObservableCollection<Crop> crops;
        private ObservableCollection<ProcessingType> processingTypes;
        private ObservableCollection<Component> carriers;
        #endregion Attributes

        #region Constructor
        public RecipeViewModel(Recipe recipe)
        {
            Recipe = recipe;
            LoadItems();
            InitSelectedItems();
        }
        #endregion Constructor

        #region Properties
        public RecipesViewModel RecipesViewModel
        {
            get { return recipesViewModel; }
            set { SetProperty(ref recipesViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (Recipe.RecipeId == 0) ? "Новый рецепт" : "Рецепт: " + Recipe.Name;
                return Recipe.Name; 
            }
            set
            {
                if (Recipe.Name != value)
                {
                    Recipe.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public Crop Crop
        {
            get { return Recipe.Crop; }
            set
            {
                if (Recipe.Crop != value)
                {
                    Recipe.Crop = value;
                    OnPropertyChanged(nameof(Crop));
                }
            }
        }

        public ObservableCollection<Crop> Crops
        {
            get { return crops; }
            set { SetProperty(ref crops, value); }
        }

        public ProcessingType ProcessingType
        {
            get { return Recipe.ProcessingType; }
            set
            {
                if (Recipe.ProcessingType != value)
                {
                    Recipe.ProcessingType = value;
                    OnPropertyChanged(nameof(ProcessingType));
                }
            }
        }

        public ObservableCollection<ProcessingType> ProcessingTypes
        {
            get { return processingTypes; }
            set { SetProperty(ref processingTypes, value); }
        }

        public Component Carrier
        {
            get { return Recipe.Carrier; }
            set
            {
                if (Recipe.Carrier != value)
                {
                    Recipe.Carrier = value;
                    OnPropertyChanged(nameof(Carrier));
                }
            }
        }

        public ObservableCollection<Component> Carriers
        {
            get { return carriers; }
            set { SetProperty(ref carriers, value); }
        }

        public string Note
        {
            get { return Recipe.Note; }
            set
            {
                if (Recipe.Note != value)
                {
                    Recipe.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public string CarrierReserve
        {
            get
            {
                if (Recipe.CarrierReserve == null)
                {
                    return "";
                }
                else
                {
                    return Recipe.CarrierReserve.ToString();
                }
            }
            set
            {
                try
                {
                    Recipe.CarrierReserve = float.Parse(value);
                    Recipe.CarrierReserve = PercentLimits(Recipe.CarrierReserve);
                }
                catch
                {
                    Recipe.CarrierReserve = null;
                }
                OnPropertyChanged(nameof(CarrierReserve));
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Methods
        public void LoadItems()
        {
            using (AppDbContext db = App.GetContext())
            {
                Crops = new ObservableCollection<Crop>(db.Crops.ToList());
                ProcessingTypes = new ObservableCollection<ProcessingType>(db.ProcessingTypes.ToList());
                Carriers = new ObservableCollection<Component>(db.Components.ToList());
            }
        }

        public void InitSelectedItems()
        {
            Crop = Crops.FirstOrDefault(c => c.CropId == Recipe.CropId);
            ProcessingType = ProcessingTypes.FirstOrDefault(pt => pt.ProcessingTypeId == Recipe.ProcessingTypeId);
            Carrier = Carriers.FirstOrDefault(c => c.ComponentId == Recipe.CarrierId);
        }

        private float? PercentLimits(float? percents)
        {
            return percents > 100.0 ? (float?)100.0 : percents < 0.0 ? (float?)0.0 : percents;
        }
        #endregion Methods
    }
}

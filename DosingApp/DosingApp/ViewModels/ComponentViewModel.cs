using DosingApp.Models;
using System;
using System.Collections.ObjectModel;

namespace DosingApp.ViewModels
{
    public class ComponentViewModel : BaseViewModel
    {
        #region Attributes
        ComponentsViewModel componentsViewModel;
        public Component Component { get; private set; }
        private string title;
        private bool isLiquid;
        #endregion Attributes

        #region Constructor
        public ComponentViewModel(Component component)
        {
            Component = component;
        }
        #endregion Constructor

        #region Properties
        public ComponentsViewModel ComponentsViewModel
        {
            get { return componentsViewModel; }
            set { SetProperty(ref componentsViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Производитель: " + ComponentsViewModel.Manufacturer.Name + ((Component.ComponentId == 0) ? "\nНовый компонент" : "\nКомпонент: " + Component.Name);
                return Component.Name; 
            }
            set
            {
                if (Component.Name != value)
                {
                    Component.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Consistency
        {
            get 
            {
                IsLiquid = Component.Consistency != null && String.Equals(Component.Consistency, ComponentConsistency.Liquid);
                return Component.Consistency; 
            }
            set
            {
                if (Component.Consistency != value)
                {
                    Component.Consistency = value;
                    OnPropertyChanged(nameof(Consistency));
                }
            }
        }

        public ObservableCollection<string> ConsistencyList
        {
            get { return new ObservableCollection<string>() { ComponentConsistency.Liquid, ComponentConsistency.Dry }; }
        }

        public double? Density
        {
            get { return Component.Density; }
            set
            {
                if (Component.Density != value)
                {
                    Component.Density = value;
                    OnPropertyChanged(nameof(Density));
                }
            }
        }

        public string Packing
        {
            get { return Component.Packing; }
            set
            {
                if (Component.Packing != value)
                {
                    Component.Packing = value;
                    OnPropertyChanged(nameof(Packing));

                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }

        public bool IsLiquid
        {
            get { return isLiquid; }
            set { SetProperty(ref isLiquid, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

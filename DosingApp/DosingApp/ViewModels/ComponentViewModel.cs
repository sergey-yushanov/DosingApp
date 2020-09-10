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
                Title = "Каталог: " + ComponentsViewModel.Manufacturer.Name + ((Component.ComponentId == 0) ? "\nНовый компонент" : "\nКомпонент: " + Component.Name);
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
                return Component.Consistency; 
            }
            set
            {
                if (Component.Consistency != value)
                {
                    if (value != null && String.Equals(value, ComponentConsistency.Dry))
                    {
                        Density = 1.0;
                    }
                    Component.Consistency = value;
                    OnPropertyChanged(nameof(Consistency));
                }
            }
        }

        public ObservableCollection<string> ConsistencyList
        {
            get { return new ObservableCollection<string>(ComponentConsistency.GetList()); }
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
            get { return (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Consistency)); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

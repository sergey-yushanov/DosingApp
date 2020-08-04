using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class ComponentViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Component> dataServiceComponents;
        #endregion Services

        #region Attributes
        ComponentsViewModel componentsViewModel;
        public Component Component { get; private set; }
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
            get { return Component.Name; }
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
            get { return Component.Consistency; }
            set
            {
                if (Component.Consistency != value)
                {
                    Component.Consistency = value;
                    OnPropertyChanged(nameof(Consistency));
                }
            }
        }

        public float Density
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
        #endregion Properties
    }
}

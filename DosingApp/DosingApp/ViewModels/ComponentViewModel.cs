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
                Title = (Component.ComponentId == 0) ? "Новый компонент" : "Компонент: " + Component.Name;
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

        public string Density
        {
            get
            {
                if (Component.Density == null)
                {
                    return "";
                }
                else
                {
                    return Component.Density.ToString();
                }
            }
            set
            {
                try
                {
                    Component.Density = float.Parse(value);
                }
                catch
                {
                    Component.Density = null;
                }
                OnPropertyChanged(nameof(Density));
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

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

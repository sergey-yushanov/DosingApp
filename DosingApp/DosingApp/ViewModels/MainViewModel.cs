using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DosingApp.ViewModels
{
    public class MenuGrouping<K, MenuItemViewModel> : ObservableCollection<MenuItemViewModel>
    {
        public K Name { get; private set; }
        public MenuGrouping(K name, IEnumerable<MenuItemViewModel> items)
        {
            Name = name;
            foreach (MenuItemViewModel item in items)
                Items.Add(item);
        }
    }

    public class MainViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        public User User { get; private set; }
        public ObservableCollection<MenuGrouping<string, MenuItemViewModel>> MenuGroups { get; private set; }

        private ObservableCollection<MenuItemViewModel> mainMenu;
        //private ObservableCollection<MenuItemViewModel> mainParams;
        //private ObservableCollection<MenuItemViewModel> additionalParams;
        //private ObservableCollection<MenuItemViewModel> adminParams;
        private bool accessMainParams;
        private bool accessAdditionalParams;
        private bool accessAdminParams;
        #endregion Attributes

        #region Properties
        public ObservableCollection<MenuItemViewModel> MainMenu
        {
            get { return this.mainMenu; }
            set { SetProperty(ref this.mainMenu, value); }
        }

/*        public ObservableCollection<MenuItemViewModel> MainParams
        {
            get { return this.mainParams; }
            set { SetProperty(ref this.mainParams, value); }
        }

        public ObservableCollection<MenuItemViewModel> AdditionalParams
        {
            get { return this.additionalParams; }
            set { SetProperty(ref this.additionalParams, value); }
        }

        public ObservableCollection<MenuItemViewModel> AdminParams
        {
            get { return this.adminParams; }
            set { SetProperty(ref this.adminParams, value); }
        }*/

        public bool AccessMainParams
        {
            get { return accessMainParams; }
            set { SetProperty(ref accessMainParams, value); }
        }

        public bool AccessAdditionalParams
        {
            get { return accessAdditionalParams; }
            set { SetProperty(ref accessAdditionalParams, value); }
        }

        public bool AccessAdminParams
        {
            get { return accessAdminParams; }
            set { SetProperty(ref accessAdminParams, value); }
        }
        #endregion Properties

        #region Constructor
        public MainViewModel()
        {
            User = App.ActiveUser;
            AccessMainParams = App.ActiveUser.AccessMainParams || App.ActiveUser.AccessAdminParams;
            AccessAdditionalParams = App.ActiveUser.AccessAdditionalParams || App.ActiveUser.AccessAdminParams;
            AccessAdminParams = App.ActiveUser.AccessAdminParams;
            this.LoadMenu();
        }
        #endregion Constructor

        #region Methods
        private void LoadMenu()
        {
            this.MainMenu = new ObservableCollection<MenuItemViewModel>();
            this.MainMenu.Clear();
            this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Mixtures, Group = MenuItemGroup.MainMenu, Title = "Сделать смесь" } );
            this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Reports, Group = MenuItemGroup.MainMenu, Title = "Отчеты" });

            if (AccessMainParams)
            { 
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Assignments, Group = MenuItemGroup.MainParams, Title = "Задания" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Recipes, Group = MenuItemGroup.MainParams, Title = "Рецепты" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Components, Group = MenuItemGroup.MainParams, Title = "Компоненты" });
            }

            if (AccessAdditionalParams)
            {
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Applicators, Group = MenuItemGroup.AdditionalParams, Title = "Аппликаторы" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Facilities, Group = MenuItemGroup.AdditionalParams, Title = "Объекты" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Transports, Group = MenuItemGroup.AdditionalParams, Title = "Транспорты" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Crops, Group = MenuItemGroup.AdditionalParams, Title = "Культуры" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.ProcessingTypes, Group = MenuItemGroup.AdditionalParams, Title = "Виды обработки" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.AgrYears, Group = MenuItemGroup.AdditionalParams, Title = "Сельхоз. годы" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Fields, Group = MenuItemGroup.AdditionalParams, Title = "Поля" });
            }

            if (AccessAdminParams)
            {
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Users, Group = MenuItemGroup.AdminParams, Title = "Пользователи" });
            }

            var groups = MainMenu.GroupBy(m => m.Group).Select(g => new MenuGrouping<string, MenuItemViewModel>(g.Key, g));
            MenuGroups = new ObservableCollection<MenuGrouping<string, MenuItemViewModel>>(groups);
        }
        #endregion Methods
    }
}

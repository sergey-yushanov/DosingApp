using DosingApp.Models;
using System.Collections.ObjectModel;

namespace DosingApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<MenuItemViewModel> menu;
        #endregion Attributes

        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu
        {
            get { return this.menu; }
            set { SetProperty(ref this.menu, value); }
        }
        #endregion Properties

        #region Constructor
        public MainViewModel()
        {
            this.LoadMenu();

            //this.InitDb();
        }
        #endregion Constructor

        #region Methods
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Clear();
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Mixtures, Access = MenuItemAccess.MainMenu, IsSeparatorVisible = false, Title = "Сделать смесь" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Reports, Access = MenuItemAccess.MainMenu, IsSeparatorVisible = true, Title = "Отчеты" });
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Assignments, Access = MenuItemAccess.MainParams, IsSeparatorVisible = false, Title = "Задания" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Recipes, Access = MenuItemAccess.MainParams, IsSeparatorVisible = false, Title = "Рецепты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Components, Access = MenuItemAccess.MainParams, IsSeparatorVisible = true, Title = "Компоненты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Applicators, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Аппликаторы" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Facilities, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Объекты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Transports, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Транспорты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Crops, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Культуры" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.ProcessingTypes, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Виды обработки" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.AgrYears, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = false, Title = "Сельхоз. годы" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Fields, Access = MenuItemAccess.AdditionalParams, IsSeparatorVisible = true, Title = "Поля" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Users, Access = MenuItemAccess.Admin, IsSeparatorVisible = false, Title = "Инженерное меню" });
        }
        #endregion Methods
    }
}

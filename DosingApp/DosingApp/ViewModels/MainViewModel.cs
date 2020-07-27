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
            set { SetValue(ref this.menu, value); }
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
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Mixtures, Title = "Сделать смесь" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Reports, Title = "Отчеты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Assignments, Title = "Задания" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Recipes, Title = "Рецепты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Components, Title = "Компоненты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Applicators, Title = "Аппликаторы" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Facilities, Title = "Объекты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Transports, Title = "Транспорты" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Crops, Title = "Культуры" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.ProcessingTypes, Title = "Виды обработки" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.AgrYears, Title = "Сельхоз. годы" } );
            this.Menu.Add(new MenuItemViewModel { Id = MenuItemType.Fields, Title = "Поля" } );
        }
        #endregion Methods
    }
}

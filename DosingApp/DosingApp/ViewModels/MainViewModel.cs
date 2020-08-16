using DosingApp.DataContext;
using DosingApp.Helpers;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Attributes
        public User user;
        private ObservableCollection<MenuGrouping<string, MenuItemViewModel>> menuGroups;
        private ObservableCollection<MenuItemViewModel> mainMenu;
        private bool accessMainParams;
        private bool accessAdditionalParams;
        private bool accessAdminParams;

        //public ICommand BackCommand { get; protected set; }
        //public ICommand LoginCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MainViewModel()
        {
            LoadMenu();

            //BackCommand = new Command(Back);
            //LoginCommand = new Command(Login);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<MenuGrouping<string, MenuItemViewModel>> MenuGroups
        {
            get { return this.menuGroups; }
            set { SetProperty(ref menuGroups, value); }
        }

        public ObservableCollection<MenuItemViewModel> MainMenu
        {
            get { return this.mainMenu; }
            set { SetProperty(ref this.mainMenu, value); }
        }

        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

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

        #region Commands
        //private void Back()
        //{
            //Application.Current.MainPage.Navigation.PopAsync();
        //}

        //private void Login()
        //{ 
            //Application.Current.MainPage.Navigation.PushPopupAsync(new LoginPage());
        //}
        #endregion Commands

        #region Methods
        private void LoadMenu()
        {
            this.MainMenu = new ObservableCollection<MenuItemViewModel>();
            this.MainMenu.Clear();
            this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Jobs, Group = MenuItemGroup.MainMenu, Title = "Сделать смесь" } );
            this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Reports, Group = MenuItemGroup.MainMenu, Title = "Отчетность" });

            if (AccessMainParams)
            { 
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Assignments, Group = MenuItemGroup.MainParams, Title = "Задания на смешивание" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Recipes, Group = MenuItemGroup.MainParams, Title = "Рецепты" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Components, Group = MenuItemGroup.MainParams, Title = "Компоненты смеси" });
            }

            if (AccessAdditionalParams)
            {
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Applicators, Group = MenuItemGroup.AdditionalParams, Title = "Аппликаторы" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Facilities, Group = MenuItemGroup.AdditionalParams, Title = "Стационарные объекты" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Transports, Group = MenuItemGroup.AdditionalParams, Title = "Транспорт" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Crops, Group = MenuItemGroup.AdditionalParams, Title = "Культуры" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.ProcessingTypes, Group = MenuItemGroup.AdditionalParams, Title = "Вид обработки" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.AgrYears, Group = MenuItemGroup.AdditionalParams, Title = "Сельскохозяйственный год" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Fields, Group = MenuItemGroup.AdditionalParams, Title = "Поля для обработки" });
            }

            this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Login, Group = MenuItemGroup.LoginMenu, Title = "Логин", MainViewModel = this });

            if (AccessAdminParams)
            {
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Users, Group = MenuItemGroup.AdminParams, Title = "Пользователи" });
                this.MainMenu.Add(new MenuItemViewModel { Id = MenuItemType.Mixers, Group = MenuItemGroup.AdminParams, Title = "Установки" });
            }

            var groups = MainMenu.GroupBy(m => m.Group).Select(g => new MenuGrouping<string, MenuItemViewModel>(g.Key, g));
            MenuGroups = new ObservableCollection<MenuGrouping<string, MenuItemViewModel>>(groups);
        }

        public void SetUserAccess()
        {
            if (App.ActiveUser != null)
            {
                User = App.ActiveUser;
                AccessMainParams = App.ActiveUser.AccessMainParams || App.ActiveUser.AccessAdminParams;
                AccessAdditionalParams = App.ActiveUser.AccessAdditionalParams || App.ActiveUser.AccessAdminParams;
                AccessAdminParams = App.ActiveUser.AccessAdminParams;
            }
            LoadMenu();
        }
        #endregion Methods
    }
}

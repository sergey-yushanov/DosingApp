using DosingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<Models.HomeMenuItem> menuItems;

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<Models.HomeMenuItem>
            {
                new Models.HomeMenuItem { Id = MenuItemType.Mixtures, Title="Сделать смесь" },
                new Models.HomeMenuItem { Id = MenuItemType.Reports, Title="Отчеты" },
                new Models.HomeMenuItem { Id = MenuItemType.Assignments, Title="Задания" },
                new Models.HomeMenuItem { Id = MenuItemType.Recipes, Title="Рецепты" },
                new Models.HomeMenuItem { Id = MenuItemType.Components, Title="Компоненты" },
                new Models.HomeMenuItem { Id = MenuItemType.Applicators, Title="Аппликаторы" },
                new Models.HomeMenuItem { Id = MenuItemType.Facilities, Title="Объекты" },
                new Models.HomeMenuItem { Id = MenuItemType.Transports, Title="Транспорты" },
                new Models.HomeMenuItem { Id = MenuItemType.Crops, Title="Культуры" },
                new Models.HomeMenuItem { Id = MenuItemType.ProcessingTypes, Title="Виды обработки" },
                new Models.HomeMenuItem { Id = MenuItemType.AgrYears, Title="Сельхоз. годы" },
                new Models.HomeMenuItem { Id = MenuItemType.Fields, Title="Поля" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((Models.HomeMenuItem)e.SelectedItem).Id;
                //await RootPage.NavigateFromMenu(id);
            };
        }
    }
}
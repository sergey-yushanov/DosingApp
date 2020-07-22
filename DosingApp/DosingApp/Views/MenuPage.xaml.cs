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
        List<HomeMenuItem> menuItems;

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Mixtures, Title="Сделать смесь" },
                new HomeMenuItem {Id = MenuItemType.Reports, Title="Отчеты" },
                new HomeMenuItem {Id = MenuItemType.Assignments, Title="Задания" },
                new HomeMenuItem {Id = MenuItemType.Recipes, Title="Рецепты" },
                new HomeMenuItem {Id = MenuItemType.Components, Title="Компоненты" },
                new HomeMenuItem {Id = MenuItemType.Applicators, Title="Аппликаторы" },
                new HomeMenuItem {Id = MenuItemType.Objects, Title="Объекты" },
                new HomeMenuItem {Id = MenuItemType.Transports, Title="Транспорты" },
                new HomeMenuItem {Id = MenuItemType.Crops, Title="Культуры" },
                new HomeMenuItem {Id = MenuItemType.ProcessingTypes, Title="Виды обработки" },
                new HomeMenuItem {Id = MenuItemType.AgrYears, Title="Сельхоз. годы" },
                new HomeMenuItem {Id = MenuItemType.Fields, Title="Поля" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}
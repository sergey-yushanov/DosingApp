using DosingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DosingApp.Views
{
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add((int)MenuItemType.Mixtures, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Components:
                        MenuPages.Add(id, new NavigationPage(new ComponentsPage()));
                        break;
                    case (int)MenuItemType.Applicators:
                        MenuPages.Add(id, new NavigationPage(new ApplicatorsPage()));
                        break;
                    case (int)MenuItemType.Facilities:
                        MenuPages.Add(id, new NavigationPage(new FacilitiesPage()));
                        break;
                    case (int)MenuItemType.Transports:
                        MenuPages.Add(id, new NavigationPage(new TransportsPage()));
                        break;
                    case (int)MenuItemType.Crops:
                        MenuPages.Add(id, new NavigationPage(new CropsPage()));
                        break;
                    case (int)MenuItemType.ProcessingTypes:
                        MenuPages.Add(id, new NavigationPage(new ProcessingTypesPage()));
                        break;
                    case (int)MenuItemType.AgrYears:
                        MenuPages.Add(id, new NavigationPage(new AgrYearsPage()));
                        break;
                    case (int)MenuItemType.Fields:
                        MenuPages.Add(id, new NavigationPage(new FieldsPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}

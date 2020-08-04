using DosingApp.Models;
using DosingApp.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class MenuItemViewModel
    {
        #region Attributes
        public MenuItemType Id { get; set; }
        public string Group { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        #endregion Attributes

        #region Commands
        public ICommand SelectMenuItemCommand
        {
            get
            {
                return new Command(SelectMenuItemExecute);
            }
        }
        #endregion Commands

        #region Methods
        private void SelectMenuItemExecute()
        {
            switch (this.Id)
            {
                case MenuItemType.Assignments:
                    Application.Current.MainPage.Navigation.PushAsync(new AssignmentsPage());
                    break;
                case MenuItemType.Recipes:
                    Application.Current.MainPage.Navigation.PushAsync(new RecipesPage());
                    break;
                case MenuItemType.Components:
                    Application.Current.MainPage.Navigation.PushAsync(new ComponentsPage());
                    break;
                case MenuItemType.Applicators:
                    Application.Current.MainPage.Navigation.PushAsync(new ApplicatorsPage());
                    break;
                case MenuItemType.Facilities:
                    Application.Current.MainPage.Navigation.PushAsync(new FacilitiesPage());
                    break;
                case MenuItemType.Transports:
                    Application.Current.MainPage.Navigation.PushAsync(new TransportsPage());
                    break;
                case MenuItemType.Crops:
                    Application.Current.MainPage.Navigation.PushAsync(new CropsPage());
                    break;
                case MenuItemType.ProcessingTypes:
                    Application.Current.MainPage.Navigation.PushAsync(new ProcessingTypesPage());
                    break;
                case MenuItemType.AgrYears:
                    Application.Current.MainPage.Navigation.PushAsync(new AgrYearsPage());
                    break;
                case MenuItemType.Fields:
                    Application.Current.MainPage.Navigation.PushAsync(new FieldsPage());
                    break;
                case MenuItemType.Users:
                    Application.Current.MainPage.Navigation.PushAsync(new UsersPage());
                    break;
            }
        }
        #endregion Methods
    }
}

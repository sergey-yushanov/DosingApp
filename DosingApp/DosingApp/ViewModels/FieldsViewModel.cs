using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FieldsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Field> fields;
        private Field selectedField;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public FieldsViewModel()
        {
            CreateCommand = new Command(CreateField);
            DeleteCommand = new Command(DeleteField);
            SaveCommand = new Command(SaveField);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Field> Fields
        {
            get { return fields; }
            set { SetProperty(ref fields, value); }
        }

        public Field SelectedField
        {
            get { return selectedField; }
            set
            {
                if (selectedField != value)
                {
                    FieldViewModel tempField = new FieldViewModel(value) { FieldsViewModel = this };
                    selectedField = null;
                    OnPropertyChanged(nameof(SelectedField));
                    Application.Current.MainPage.Navigation.PushAsync(new FieldPage(tempField));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateField()
        {
            Application.Current.MainPage.Navigation.PushAsync(new FieldPage(new FieldViewModel(new Field()) { FieldsViewModel = this }));
        }

        private void DeleteField(object fieldInstance)
        {
            FieldViewModel fieldViewModel = fieldInstance as FieldViewModel;
            if (fieldViewModel.Field != null && fieldViewModel.Field.FieldId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Fields.Remove(fieldViewModel.Field);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveField(object fieldInstance)
        {
            FieldViewModel fieldViewModel = fieldInstance as FieldViewModel;
            if (fieldViewModel.Field != null && fieldViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (fieldViewModel.Field.FieldId == 0)
                    {
                        db.Entry(fieldViewModel.Field).State = EntityState.Added;
                    }
                    else
                    {
                        db.Fields.Update(fieldViewModel.Field);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFields()
        {
            using (AppDbContext db = App.GetContext())
            {
                Fields = new ObservableCollection<Field>(db.Fields.ToList());
            }
        }
        #endregion Methods

    }
}

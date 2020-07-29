using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FieldsViewModel : BaseViewModel
    {
        #region Services
        private readonly DBDataAccess<Field> dataServiceFields;
        #endregion Services

        #region Attributes
        public ObservableCollection<Field> Fields { get; set; }
        public ICommand CreateCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand SaveCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        FieldViewModel selectedField;
        #endregion Attributes

        #region Constructor
        public FieldsViewModel()
        {
            CreateCommand = new Command(CreateField);
            DeleteCommand = new Command(DeleteField);
            SaveCommand = new Command(SaveField);
            BackCommand = new Command(Back);

            dataServiceFields = new DBDataAccess<Field>();
            //this.CreateFields();
            this.LoadFields();
        }
        #endregion Constructor

        #region Properties
        public FieldViewModel SelectedField
        {
            get { return selectedField; }
            set
            {
                if (selectedField != value)
                {
                    FieldViewModel tempField = value;
                    selectedField = null;
                    OnPropertyChanged(nameof(SelectedField));
                    Navigation.PushAsync(new FieldPage(tempField));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Navigation.PopAsync();
        }

        private void CreateField()
        {
            Navigation.PushAsync(new FieldPage(new FieldViewModel() { FieldsViewModel = this }));
        }

        private void DeleteField(object fieldInstance)
        {
            FieldViewModel fieldViewModel = fieldInstance as FieldViewModel;
            if (fieldViewModel != null)
            {
                dataServiceFields.Delete(fieldViewModel.Field);
            }
            Back();
        }

        private void SaveField(object fieldInstance)
        {
            FieldViewModel fieldViewModel = fieldInstance as FieldViewModel;
            if (fieldViewModel != null && fieldViewModel.IsValid)
            {
                if (fieldViewModel.Field.FieldId == 0)
                    dataServiceFields.Create(fieldViewModel.Field);
                else
                    dataServiceFields.Update(fieldViewModel.Field);               
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFields()
        {
            var fieldsDB = dataServiceFields.Get().ToList() as List<Field>;
            Fields = new ObservableCollection<Field>(fieldsDB);
        }

        private void CreateFields()
        {
            var fields = new List<Field>()
            {
                new Field { Name = "Field 1", Code = "f1" },
                new Field { Name = "Field 2", Code = "f2" },
                new Field { Name = "Field 3", Code = "f3" }
            };
            dataServiceFields.SaveList(fields);
        }
        #endregion Methods

    }
}

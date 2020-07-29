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
        private ObservableCollection<Field> fields;
        private Field selectedField;
        
        private string name;
        private string code;

        public ICommand CreateCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand SaveCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        #endregion Attributes

        #region Properties
        public ObservableCollection<Field> Fields
        {
            get { return this.fields; }
            set { SetValue(ref this.fields, value); }
        }

        public Field SelectedField
        {
            get { return this.selectedField; }
            set
            { 
                SetValue(ref this.selectedField, value);
                this.Name = selectedField.Name;
                this.Code = selectedField.Code;
                Application.Current.MainPage.Navigation.PushAsync(new FieldPage());
            }
        }

        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        public string Code
        {
            get { return this.code; }
            set { SetValue(ref this.code, value); }
        }
        #endregion Properties

        #region Constructor
        public FieldsViewModel()
        {
            this.dataServiceFields = new DBDataAccess<Field>();

            CreateCommand = new Command(CreateField);
            DeleteCommand = new Command(DeleteField);
            SaveCommand = new Command(SaveField);
            BackCommand = new Command(Back);

            //this.CreateFields();
            this.LoadFields();
        }
        #endregion Constructor

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateField()
        {
            this.Name = string.Empty;
            this.Code = string.Empty;
            Application.Current.MainPage.Navigation.PushAsync(new FieldPage());
        }

        private void DeleteField()
        {
            if (selectedField != null)
            {
                this.dataServiceFields.Delete(selectedField);
            }
            Back();
        }

        private void SaveField()
        {
            if (IsValid(this.Name))
            {
                var newField = new Field()
                {
                    Name = this.Name,
                    Code = this.Code
                };

                if (this.dataServiceFields.Create(newField))
                {
                    this.Name = string.Empty;
                    this.Code = string.Empty;
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFields()
        {
            var fieldsDB = this.dataServiceFields.Get().ToList() as List<Field>;
            this.Fields = new ObservableCollection<Field>(fieldsDB);
        }

        private void CreateFields()
        {
            var fields = new List<Field>()
            {
                new Field { Name = "Field 1", Code = "f1" },
                new Field { Name = "Field 2", Code = "f2" },
                new Field { Name = "Field 3", Code = "f3" }
            };

            this.dataServiceFields.SaveList(fields);
        }
        #endregion Methods

    }
}

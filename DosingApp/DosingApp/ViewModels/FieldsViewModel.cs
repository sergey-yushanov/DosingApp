using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class FieldsViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Field> dataServiceFields;
        public readonly AppDbContext db;
        #endregion Services

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
            db = App.GetContext();
            LoadFields();
            //CreateFields();

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
                db.Fields.Attach(fieldViewModel.Field);
                db.Fields.Remove(fieldViewModel.Field);
                db.SaveChanges();
                LoadFields();
                Back();
            }
        }

        private void SaveField(object fieldInstance)
        {
            FieldViewModel fieldViewModel = fieldInstance as FieldViewModel;
            if (fieldViewModel.Field != null && fieldViewModel.IsValid)
            {
                if (fieldViewModel.Field.FieldId == 0)
                {
                    db.Entry(fieldViewModel.Field).State = EntityState.Added;
                }
                else
                {
                    db.Fields.Attach(fieldViewModel.Field);
                    db.Fields.Update(fieldViewModel.Field);
                }
                db.SaveChanges();
            }
            LoadFields();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFields()
        {
            Fields = new ObservableCollection<Field>(db.Fields.ToList());
        }

        private void CreateFields()
        {
            var fields = new List<Field>()
            {
                new Field { Name = "Field 1", Code = "f1" },
                new Field { Name = "Field 2", Code = "f2" },
                new Field { Name = "Field 3", Code = "f3" }
            };

            db.Fields.AddRange(fields);
            db.SaveChanges();
        }
        #endregion Methods

    }
}

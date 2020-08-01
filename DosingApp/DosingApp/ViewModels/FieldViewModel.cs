using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class FieldViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Field> dataServiceFields;
        #endregion Services

        #region Attributes
        FieldsViewModel fieldsViewModel;
        public Field Field { get; private set; }
        #endregion Attributes

        #region Constructor
        public FieldViewModel(Field field)
        {
            Field = field;
        }
        #endregion Constructor

        #region Properties
        public FieldsViewModel FieldsViewModel
        {
            get { return fieldsViewModel; }
            set { SetProperty(ref fieldsViewModel, value); }
        }

        public string Name
        {
            get { return Field.Name; }
            set
            {
                if (Field.Name != value)
                {
                    Field.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Code
        {
            get { return Field.Code; }
            set
            {
                if (Field.Code != value)
                {
                    Field.Code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }
        #endregion Properties
    }
}

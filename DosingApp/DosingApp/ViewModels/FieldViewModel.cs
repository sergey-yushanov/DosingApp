using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class FieldViewModel : BaseViewModel
    {
        #region Attributes
        FieldsViewModel fieldsViewModel;
        public Field Field { get; private set; }
        private string title;
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
            get
            {
                Title = (Field.FieldId == 0) ? "Новое поле" : "Поле: " + Field.Name;
                return Field.Name;
            }
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

        public string Size
        {
            get
            {
                if (Field.Size == null)
                {
                    return "";
                }
                else
                {
                    return Field.Size.ToString();
                }
            }
            set
            {
                try
                {
                    Field.Size = float.Parse(value);
                }
                catch
                {
                    Field.Size = null;
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

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

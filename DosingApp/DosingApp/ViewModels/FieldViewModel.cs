using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FieldViewModel : BaseViewModel
    {
        FieldsViewModel fieldsViewModel; 
        
        public Field Field { get; private set; }

        public FieldViewModel()
        {
            Field = new Field();
        }

        public FieldsViewModel FieldsViewModel
        {
            get { return fieldsViewModel; }
            set
            {
                if (fieldsViewModel != value)
                {
                    fieldsViewModel = value;
                    OnPropertyChanged(nameof(FieldsViewModel));
                }
            }
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
    }
}

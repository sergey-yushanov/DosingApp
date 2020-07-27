using DosingApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FieldsViewModel : BaseViewModel
    {
        public ObservableCollection<FieldViewModel> Fields { get; set; }

        public ICommand CreateFieldCommand { protected set; get; }
        public ICommand DeleteFieldCommand { protected set; get; }
        public ICommand SaveFieldCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        
        FieldViewModel selectedField;

        public INavigation Navigation { get; set; }

        public FieldsViewModel()
        {
            Fields = new ObservableCollection<FieldViewModel>();
            CreateFieldCommand = new Command(CreateField);
            DeleteFieldCommand = new Command(DeleteField);
            SaveFieldCommand = new Command(SaveField);
            BackCommand = new Command(Back);
        }

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
            FieldViewModel field = fieldInstance as FieldViewModel;
            if (field != null)
            {
                Fields.Remove(field);
            }
            Back();
        }

        private void SaveField(object fieldInstance)
        {
            FieldViewModel field = fieldInstance as FieldViewModel;
            if (field != null && field.IsValid)
            {
                Fields.Add(field);
            }
            Back();
        }


    }
}

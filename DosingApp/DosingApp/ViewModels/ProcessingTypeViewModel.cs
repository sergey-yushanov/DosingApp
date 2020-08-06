using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class ProcessingTypeViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<ProcessingType> dataServiceProcessingTypes;
        #endregion Services

        #region Attributes
        ProcessingTypesViewModel processingTypesViewModel;
        public ProcessingType ProcessingType { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public ProcessingTypeViewModel(ProcessingType processingType)
        {
            ProcessingType = processingType;
        }
        #endregion Constructor

        #region Properties
        public ProcessingTypesViewModel ProcessingTypesViewModel
        {
            get { return processingTypesViewModel; }
            set { SetProperty(ref processingTypesViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (ProcessingType.ProcessingTypeId == 0) ? "Новый вид обработки" : "Вид обработки: " + ProcessingType.Name;
                return ProcessingType.Name; 
            }
            set
            {
                if (ProcessingType.Name != value)
                {
                    ProcessingType.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Code
        {
            get { return ProcessingType.Code; }
            set
            {
                if (ProcessingType.Code != value)
                {
                    ProcessingType.Code = value;
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

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

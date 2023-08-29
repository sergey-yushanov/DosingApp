using DosingApp.Models;
using System;

namespace DosingApp.ViewModels
{
    public class MixerViewModel : BaseViewModel
    {
        #region Attributes
        MixersViewModel mixersViewModel;
        public Mixer Mixer { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public MixerViewModel(Mixer mixer)
        {
            Mixer = mixer;
        }
        #endregion Constructor

        #region Properties
        public MixersViewModel MixersViewModel
        {
            get { return mixersViewModel; }
            set { SetProperty(ref mixersViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = (Mixer.MixerId == 0) ? "Новая система" : "Система: " + Mixer.Name;
                return Mixer.Name;
            }
            set
            {
                if (Mixer.Name != value)
                {
                    Mixer.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int? Collector
        {
            get { return Mixer.Collector; }
            set
            {
                if (Mixer.Collector != value)
                {
                    Mixer.Collector = value;
                    OnPropertyChanged(nameof(Collector));
                }
            }
        }

        //public int? Single
        //{
        //    get { return Mixer.Single; }
        //    set
        //    {
        //        if (Mixer.Single != value)
        //        {
        //            Mixer.Single = value;
        //            OnPropertyChanged(nameof(Single));
        //        }
        //    }
        //}

        public int? Volume
        {
            get { return Mixer.Volume; }
            set
            {
                if (Mixer.Volume != value)
                {
                    Mixer.Volume = value;
                    OnPropertyChanged(nameof(Volume));
                }
            }
        }

        public string Url
        {
            get { return Mixer.Url; }
            set
            {
                if (Mixer.Url != value)
                {
                    Mixer.Url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }

        //public int? ThreeWay
        //{
        //    get { return Mixer.ThreeWay; }
        //    set
        //    {
        //        if (Mixer.ThreeWay != value)
        //        {
        //            Mixer.ThreeWay = value;
        //            OnPropertyChanged(nameof(ThreeWay));
        //        }
        //    }
        //}

        public bool IsUsedMixer
        {
            get { return Mixer.IsUsedMixer; }
            set
            {
                if (Mixer.IsUsedMixer != value)
                {
                    Mixer.IsUsedMixer = value;
                    OnPropertyChanged(nameof(IsUsedMixer));
                }
            }
        }

        public bool IsValid
        {
            get { return (!String.IsNullOrEmpty(Name)); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

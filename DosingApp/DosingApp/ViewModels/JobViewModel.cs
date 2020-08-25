using Acr.UserDialogs;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class JobViewModel : BaseViewModel
    {
        #region Attributes
        JobsViewModel jobsViewModel;
        public Job Job { get; private set; }

        private ObservableCollection<Recipe> recipes;
        private ObservableCollection<Facility> facilities;
        private ObservableCollection<Transport> transports;
        private ObservableCollection<Applicator> applicators;
        private ObservableCollection<AgrYear> agrYears;
        private ObservableCollection<Field> fields;

        private ObservableCollection<FacilityTank> sourceFacilityTanks;
        private ObservableCollection<FacilityTank> destFacilityTanks;
        private ObservableCollection<TransportTank> sourceTransportTanks;
        private ObservableCollection<TransportTank> destTransportTanks;
        private ObservableCollection<ApplicatorTank> sourceApplicatorTanks;
        private ObservableCollection<ApplicatorTank> destApplicatorTanks;

        private bool isAccessToChangeMemory;
        #endregion Attributes

        #region Constructor
        public JobViewModel(Job job)
        {
            Job = job;
            InitJob();
            CalculateVolume();
            IsAccessToChangeMemory = false;
        }
        #endregion Constructor

        #region Properties
        public JobsViewModel JobsViewModel
        {
            get { return jobsViewModel; }
            set { SetProperty(ref jobsViewModel, value); }
        }

        public string Name
        {
            get { return Job.Name; }
        }

        public bool IsEnableToChange
        {
            get { return App.ActiveUser.AccessJobParams; }
        }

        public bool IsDisableToChange
        {
            get { return !App.ActiveUser.AccessJobParams; }
        }

        public bool IsAccessToChange
        {
            get { return Job.IsAccessToChange; }
            set
            {
/*                if (value && !IsAccessToChangeMemory)
                {
                    AccessToChange();
                }*/
                 
                if (value && IsAccessToChangeMemory)
                {
                    if (Job.IsAccessToChange != value)
                    {
                        Job.IsAccessToChange = value;
                    }
                }
                OnPropertyChanged(nameof(IsAccessToChange));
            }
        }

        public bool IsAccessToChangeMemory
        {
            get { return isAccessToChangeMemory; }
            set
            {
                SetProperty(ref isAccessToChangeMemory, value);
                if (value)
                {
                    IsAccessToChange = true;
                }
            }
        }

        public string Note
        {
            get { return Job.Note; }
            set
            {
                if (Job.Note != value)
                {
                    Job.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public Recipe Recipe
        {
            get { return Job.Recipe; }
            set
            {
                if (Job.Recipe != value)
                {
                    Job.Recipe = value;
                    OnPropertyChanged(nameof(Recipe));
                }
            }
        }

        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set { SetProperty(ref recipes, value); }
        }


        public string SourceType
        {
            get 
            {
                IsSourceFacility = String.Equals(Job.SourceType, SourceDestType.Facility);
                IsSourceTransport = String.Equals(Job.SourceType, SourceDestType.Transport);
                IsSourceApplicator = String.Equals(Job.SourceType, SourceDestType.Applicator);
                return Job.SourceType; 
            }
            set 
            {
                if (Job.SourceType != value)
                {
                    Job.SourceType = value;
                    OnPropertyChanged(nameof(SourceType));
                }
            }
        }

        public string DestType
        {
            get
            {
                IsDestFacility = String.Equals(Job.DestType, SourceDestType.Facility);
                IsDestTransport = String.Equals(Job.DestType, SourceDestType.Transport);
                IsDestApplicator = String.Equals(Job.DestType, SourceDestType.Applicator);
                return Job.DestType;
            }
            set
            {
                if (Job.DestType != value)
                {
                    Job.DestType = value;
                    OnPropertyChanged(nameof(DestType));
                }
            }
        }

        public ObservableCollection<string> SourceDestTypes
        {
            get { return new ObservableCollection<string>(SourceDestType.GetList()); }
        }

        public bool IsSourceFacility
        {
            get { return Job.IsSourceFacility; }
            set
            {
                if (Job.IsSourceFacility != value)
                {
                    Job.IsSourceFacility = value;
                    OnPropertyChanged(nameof(IsSourceFacility));
                }
            }
        }

        public Facility SourceFacility
        {
            get
            {
                SourceFacilityTanks = GetFacilityTanks(Job.SourceFacility);
                return Job.SourceFacility;
            }
            set
            {
                if (Job.SourceFacility != value)
                {
                    Job.SourceFacility = value;
                    OnPropertyChanged(nameof(SourceFacility));
                }
            }
        }

        public FacilityTank SourceFacilityTank
        {
            get { return Job.SourceFacilityTank; }
            set
            {
                if (Job.SourceFacilityTank != value)
                {
                    Job.SourceFacilityTank = value;
                    OnPropertyChanged(nameof(SourceFacilityTank));
                }
            }
        }

        public ObservableCollection<FacilityTank> SourceFacilityTanks
        {
            get { return sourceFacilityTanks; }
            set { SetProperty(ref sourceFacilityTanks, value); }
        }

        public bool IsDestFacility
        {
            get { return Job.IsDestFacility; }
            set
            {
                if (Job.IsDestFacility != value)
                {
                    Job.IsDestFacility = value;
                    OnPropertyChanged(nameof(IsDestFacility));
                }
            }
        }

        public Facility DestFacility
        {
            get
            {
                DestFacilityTanks = GetFacilityTanks(Job.DestFacility);
                return Job.DestFacility;
            }
            set
            {
                if (Job.DestFacility != value)
                {
                    Job.DestFacility = value;
                    OnPropertyChanged(nameof(DestFacility));
                }
            }
        }

        public FacilityTank DestFacilityTank
        {
            get { return Job.DestFacilityTank; }
            set
            {
                if (Job.DestFacilityTank != value)
                {
                    Job.DestFacilityTank = value;
                    OnPropertyChanged(nameof(DestFacilityTank));
                }
            }
        }

        public ObservableCollection<FacilityTank> DestFacilityTanks
        {
            get { return destFacilityTanks; }
            set { SetProperty(ref destFacilityTanks, value); }
        }

        public ObservableCollection<Facility> Facilities
        {
            get { return facilities; }
            set { SetProperty(ref facilities, value); }
        }

        public bool IsSourceTransport
        {
            get { return Job.IsSourceTransport; }
            set
            {
                if (Job.IsSourceTransport != value)
                {
                    Job.IsSourceTransport = value;
                    OnPropertyChanged(nameof(IsSourceTransport));
                }
            }
        }

        public Transport SourceTransport
        {
            get
            {
                SourceTransportTanks = GetTransportTanks(Job.SourceTransport);
                return Job.SourceTransport; 
            }
            set
            {
                if (Job.SourceTransport != value)
                {
                    Job.SourceTransport = value;
                    OnPropertyChanged(nameof(SourceTransport));
                }
            }
        }

        public TransportTank SourceTransportTank
        {
            get { return Job.SourceTransportTank; }
            set
            {
                if (Job.SourceTransportTank != value)
                {
                    Job.SourceTransportTank = value;
                    OnPropertyChanged(nameof(SourceTransportTank));
                }
            }
        }

        public ObservableCollection<TransportTank> SourceTransportTanks
        {
            get { return sourceTransportTanks; }
            set { SetProperty(ref sourceTransportTanks, value); }
        }

        public bool IsDestTransport
        {
            get { return Job.IsDestTransport; }
            set
            {
                if (Job.IsDestTransport != value)
                {
                    Job.IsDestTransport = value;
                    OnPropertyChanged(nameof(IsDestTransport));
                }
            }
        }

        public Transport DestTransport
        {
            get
            {
                DestTransportTanks = GetTransportTanks(Job.DestTransport);
                return Job.DestTransport; 
            }
            set
            {
                if (Job.DestTransport != value)
                {
                    Job.DestTransport = value;
                    OnPropertyChanged(nameof(DestTransport));
                }
            }
        }

        public TransportTank DestTransportTank
        {
            get { return Job.DestTransportTank; }
            set
            {
                if (Job.DestTransportTank != value)
                {
                    Job.DestTransportTank = value;
                    OnPropertyChanged(nameof(DestTransportTank));
                }
            }
        }

        public ObservableCollection<TransportTank> DestTransportTanks
        {
            get { return destTransportTanks; }
            set { SetProperty(ref destTransportTanks, value); }
        }

        public ObservableCollection<Transport> Transports
        {
            get { return transports; }
            set { SetProperty(ref transports, value); }
        }

        public bool IsSourceApplicator
        {
            get { return Job.IsSourceApplicator; }
            set
            {
                if (Job.IsSourceApplicator != value)
                {
                    Job.IsSourceApplicator = value;
                    OnPropertyChanged(nameof(IsSourceApplicator));
                }
            }
        }

        public Applicator SourceApplicator
        {
            get 
            {
                SourceApplicatorTanks = GetApplicatorTanks(Job.SourceApplicator);
                return Job.SourceApplicator; 
            }
            set
            {
                if (Job.SourceApplicator != value)
                {
                    Job.SourceApplicator = value;
                    OnPropertyChanged(nameof(SourceApplicator));
                }
            }
        }

        public ApplicatorTank SourceApplicatorTank
        {
            get { return Job.SourceApplicatorTank; }
            set
            {
                if (Job.SourceApplicatorTank != value)
                {
                    Job.SourceApplicatorTank = value;
                    OnPropertyChanged(nameof(SourceApplicatorTank));
                }
            }
        }

        public ObservableCollection<ApplicatorTank> SourceApplicatorTanks
        {
            get { return sourceApplicatorTanks; }
            set { SetProperty(ref sourceApplicatorTanks, value); }
        }

        public bool IsDestApplicator
        {
            get { return Job.IsDestApplicator; }
            set
            {
                if (Job.IsDestApplicator != value)
                {
                    Job.IsDestApplicator = value;
                    OnPropertyChanged(nameof(IsDestApplicator));
                }
            }
        }

        public Applicator DestApplicator
        {
            get 
            {
                DestApplicatorTanks = GetApplicatorTanks(Job.DestApplicator);
                return Job.DestApplicator; 
            }
            set
            {
                if (Job.DestApplicator != value)
                {
                    Job.DestApplicator = value;
                    OnPropertyChanged(nameof(DestApplicator));
                }
            }
        }

        public ApplicatorTank DestApplicatorTank
        {
            get { return Job.DestApplicatorTank; }
            set
            {
                if (Job.DestApplicatorTank != value)
                {
                    Job.DestApplicatorTank = value;
                    OnPropertyChanged(nameof(DestApplicatorTank));
                }
            }
        }

        public ObservableCollection<ApplicatorTank> DestApplicatorTanks
        {
            get { return destApplicatorTanks; }
            set { SetProperty(ref destApplicatorTanks, value); }
        }

        public ObservableCollection<Applicator> Applicators
        {
            get { return applicators; }
            set { SetProperty(ref applicators, value); }
        }

        public AgrYear AgrYear
        {
            get { return Job.AgrYear; }
            set
            {
                if (Job.AgrYear != value)
                {
                    Job.AgrYear = value;
                    OnPropertyChanged(nameof(AgrYear));
                }
            }
        }

        public ObservableCollection<AgrYear> AgrYears
        {
            get { return agrYears; }
            set { SetProperty(ref agrYears, value); }
        }

        public Field Field
        {
            get { return Job.Field; }
            set
            {
                if (Job.Field != value)
                {
                    Job.Field = value;
                    OnPropertyChanged(nameof(Field));
                }
            }
        }

        public ObservableCollection<Field> Fields
        {
            get { return fields; }
            set { SetProperty(ref fields, value); }
        }

        public double? PartyVolume
        {
            get { return Job.PartyVolume; }
            set
            {
                if (Job.PartyVolume != value)
                {
                    Job.PartyVolume = value;
                    OnPropertyChanged(nameof(PartyVolume));
                }
            }
        }

        public double? AssignmentSize
        {
            get { return Job.AssignmentSize; }
            set
            {
                if (Job.AssignmentSize != value)
                {
                    Job.AssignmentSize = value;
                    OnPropertyChanged(nameof(AssignmentSize));
                }
            }
        }
        public double? VolumeRate
        {
            get { return Job.VolumeRate; }
            set
            {
                if (Job.VolumeRate != value)
                {
                    Job.VolumeRate = value;
                    OnPropertyChanged(nameof(VolumeRate));
                }
            }
        }

        public string AssignmentType
        {
            get { return Job.AssignmentType; }
            set
            {
                if (Job.AssignmentType != value)
                {
                    Job.AssignmentType = value;
                    OnPropertyChanged(nameof(AssignmentType));
                }
            }
        }

        public ObservableCollection<string> AssignmentTypeList
        {
            get { return new ObservableCollection<string>(CAssignmentType.GetList()); }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }
        #endregion Properties

        #region Commands
        //private async void AccessToChange(object sender, ToggledEventArgs e)
        //{
            //if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Вы действительно хотите внести изменения в задание?", "Да", "Нет"))
            //{
                //IsAccessToChange = true;
            //}
        //}
        #endregion Commands

        #region Methods
        public void InitJob()
        {
            Job.Note = Job.Assignment.Note;

            Job.AssignmentId = Job.Assignment.AssignmentId;

            Job.RecipeId = Job.Assignment.RecipeId;
            
            Job.SourceType = Job.Assignment.SourceType;
            Job.DestType = Job.Assignment.DestType;

            Job.IsSourceFacility = Job.Assignment.IsSourceFacility;
            Job.SourceFacilityId = Job.Assignment.SourceFacilityId;
            Job.SourceFacilityTankId = Job.Assignment.SourceFacilityTankId;

            Job.IsSourceTransport = Job.Assignment.IsSourceTransport;
            Job.SourceTransportId = Job.Assignment.SourceTransportId;
            Job.SourceTransportTankId = Job.Assignment.SourceTransportTankId;

            Job.IsSourceApplicator = Job.Assignment.IsSourceApplicator;
            Job.SourceApplicatorId = Job.Assignment.SourceApplicatorId;
            Job.SourceApplicatorTankId = Job.Assignment.SourceApplicatorTankId;

            Job.IsDestFacility = Job.Assignment.IsDestFacility;
            Job.DestFacilityId = Job.Assignment.DestFacilityId;
            Job.DestFacilityTankId = Job.Assignment.DestFacilityTankId;

            Job.IsDestTransport = Job.Assignment.IsDestTransport;
            Job.DestTransportId = Job.Assignment.DestTransportId;
            Job.DestTransportTankId = Job.Assignment.DestTransportTankId;

            Job.IsDestApplicator = Job.Assignment.IsDestApplicator;
            Job.DestApplicatorId = Job.Assignment.DestApplicatorId;
            Job.DestApplicatorTankId = Job.Assignment.DestApplicatorTankId;

            Job.AgrYearId = Job.Assignment.AgrYearId;
            Job.FieldId = Job.Assignment.FieldId;

            Job.VolumeRate = Job.Assignment.VolumeRate;

            Job.AssignmentType = Job.Assignment.Type;
            Job.AssignmentSize = Job.Assignment.Size;
            //Job.Unit = Job.Assignment.Unit;


            LoadItems();
            InitSelectedItems();

            switch (DestType)
            {
                case SourceDestType.Facility:
                    Job.PartyVolume = DestFacilityTank?.Volume;
                    break;
                case SourceDestType.Transport:
                    Job.PartyVolume = DestTransportTank?.Volume;
                    break;
                case SourceDestType.Applicator:
                    Job.PartyVolume = DestApplicatorTank?.Volume;
                    break;
            }
        }

        public void CalculateVolume()
        {
            return;
        }

        public void LoadItems()
        {
            using (AppDbContext db = App.GetContext())
            {
                Recipes = new ObservableCollection<Recipe>(db.Recipes.ToList());
                Facilities = new ObservableCollection<Facility>(db.Facilities.ToList());
                Transports = new ObservableCollection<Transport>(db.Transports.ToList());
                Applicators = new ObservableCollection<Applicator>(db.Applicators.ToList());
                AgrYears = new ObservableCollection<AgrYear>(db.AgrYears.ToList());
                Fields = new ObservableCollection<Field>(db.Fields.ToList());
            }
        }

        public void InitSelectedItems()
        {
            Recipe = Recipes.FirstOrDefault(r => r.RecipeId == Job.RecipeId);
            AgrYear = AgrYears.FirstOrDefault(ay => ay.AgrYearId == Job.AgrYearId);
            Field = Fields.FirstOrDefault(f => f.FieldId == Job.FieldId);

            SourceFacility = Facilities.FirstOrDefault(f => f.FacilityId == Job.SourceFacilityId);
            DestFacility = Facilities.FirstOrDefault(f => f.FacilityId == Job.DestFacilityId);
            SourceTransport = Transports.FirstOrDefault(t => t.TransportId == Job.SourceTransportId);
            DestTransport = Transports.FirstOrDefault(t => t.TransportId == Job.DestTransportId);
            SourceApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Job.SourceApplicatorId);
            DestApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Job.DestApplicatorId);

            SourceFacilityTanks = GetFacilityTanks(SourceFacility);
            DestFacilityTanks = GetFacilityTanks(DestFacility);
            SourceTransportTanks = GetTransportTanks(SourceTransport);
            DestTransportTanks = GetTransportTanks(DestTransport);
            SourceApplicatorTanks = GetApplicatorTanks(SourceApplicator);
            DestApplicatorTanks = GetApplicatorTanks(DestApplicator);

            SourceFacilityTank = SourceFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Job.SourceFacilityTankId);
            DestFacilityTank = DestFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Job.DestFacilityTankId);
            SourceTransportTank = SourceTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Job.SourceTransportTankId);
            DestTransportTank = DestTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Job.DestTransportTankId);
            SourceApplicatorTank = SourceApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Job.SourceApplicatorTankId);
            DestApplicatorTank = DestApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Job.DestApplicatorTankId);
        }

        public ObservableCollection<FacilityTank> GetFacilityTanks(Facility facility)
        {
            if (facility == null)
            {
                return null;
            }
            else
            {
                using (AppDbContext db = App.GetContext())
                {
                    var facilityTanksDB = db.FacilityTanks.Where(ft => ft.FacilityId == facility.FacilityId);
                    return new ObservableCollection<FacilityTank>(facilityTanksDB.ToList());
                }
            }
        }

        public ObservableCollection<TransportTank> GetTransportTanks(Transport transport)
        {
            if (transport == null)
            {
                return null;
            }
            else
            {
                using (AppDbContext db = App.GetContext())
                {
                    var transportTanksDB = db.TransportTanks.Where(tt => tt.TransportId == transport.TransportId);
                    return new ObservableCollection<TransportTank>(transportTanksDB.ToList());
                }
            }
        }

        public ObservableCollection<ApplicatorTank> GetApplicatorTanks(Applicator applicator)
        {
            if (applicator == null)
            {
                return null;
            }
            else
            {
                using (AppDbContext db = App.GetContext())
                {
                    var applicatorTanksDB = db.ApplicatorTanks.Where(at => at.ApplicatorId == applicator.ApplicatorId);
                    return new ObservableCollection<ApplicatorTank>(applicatorTanksDB.ToList());
                }
            }
        }

/*        private async void AccessToChange()
        {
            if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Вы действительно хотите внести изменения в задание?", "Да", "Нет"))
            {
                IsAccessToChangeMemory = true;
            }
        }*/
        #endregion Methods
    }
}

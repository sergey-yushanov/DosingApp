using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class AssignmentViewModel : BaseViewModel
    {
        #region Attributes
        AssignmentsViewModel assignmentsViewModel;
        public Assignment Assignment { get; private set; }
        private string title;

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

        public ICommand ClearRecipeCommand { get; protected set; }
        public ICommand ClearSourceTypeCommand { get; protected set; }
        public ICommand ClearSourceFacilityCommand { get; protected set; }
        public ICommand ClearSourceTransportCommand { get; protected set; }
        public ICommand ClearSourceApplicatorCommand { get; protected set; }
        public ICommand ClearSourceFacilityTankCommand { get; protected set; }
        public ICommand ClearSourceTransportTankCommand { get; protected set; }
        public ICommand ClearSourceApplicatorTankCommand { get; protected set; }
        public ICommand ClearDestTypeCommand { get; protected set; }
        public ICommand ClearDestFacilityCommand { get; protected set; }
        public ICommand ClearDestTransportCommand { get; protected set; }
        public ICommand ClearDestApplicatorCommand { get; protected set; }
        public ICommand ClearDestFacilityTankCommand { get; protected set; }
        public ICommand ClearDestTransportTankCommand { get; protected set; }
        public ICommand ClearDestApplicatorTankCommand { get; protected set; }
        public ICommand ClearAgrYearCommand { get; protected set; }
        public ICommand ClearFieldCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public AssignmentViewModel(Assignment assignment)
        {
            Assignment = assignment;

            ClearRecipeCommand = new Command(ClearRecipe);
            ClearSourceTypeCommand = new Command(ClearSourceType);
            ClearSourceFacilityCommand = new Command(ClearSourceFacility);
            ClearSourceTransportCommand = new Command(ClearSourceTransport);
            ClearSourceApplicatorCommand = new Command(ClearSourceApplicator);
            ClearSourceFacilityTankCommand = new Command(ClearSourceFacilityTank);
            ClearSourceTransportTankCommand = new Command(ClearSourceTransportTank);
            ClearSourceApplicatorTankCommand = new Command(ClearSourceApplicatorTank);
            ClearDestTypeCommand = new Command(ClearDestType);
            ClearDestFacilityCommand = new Command(ClearDestFacility);
            ClearDestTransportCommand = new Command(ClearDestTransport);
            ClearDestApplicatorCommand = new Command(ClearDestApplicator);
            ClearDestFacilityTankCommand = new Command(ClearDestFacilityTank);
            ClearDestTransportTankCommand = new Command(ClearDestTransportTank);
            ClearDestApplicatorTankCommand = new Command(ClearDestApplicatorTank);
            ClearAgrYearCommand = new Command(ClearAgrYear);
            ClearFieldCommand = new Command(ClearField);
    }
        #endregion Constructor

        #region Properties
        public AssignmentsViewModel AssignmentsViewModel
        {
            get { return assignmentsViewModel; }
            set { SetProperty(ref assignmentsViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (Assignment.AssignmentId == 0) ? "Новое задание" : "Задание: " + Assignment.Name;
                return Assignment.Name; 
            }
            set
            {
                if (Assignment.Name != value)
                {
                    Assignment.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Note
        {
            get { return Assignment.Note; }
            set
            {
                if (Assignment.Note != value)
                {
                    Assignment.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public Recipe Recipe
        {
            get { return Assignment.Recipe; }
            set
            {
                if (Assignment.Recipe != value)
                {
                    Assignment.Recipe = value;
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
                IsSourceFacility = String.Equals(Assignment.SourceType, SourceDestType.Facility);
                IsSourceTransport = String.Equals(Assignment.SourceType, SourceDestType.Transport);
                IsSourceApplicator = String.Equals(Assignment.SourceType, SourceDestType.Applicator);
                return Assignment.SourceType; 
            }
            set 
            {
                if (Assignment.SourceType != value)
                {
                    Assignment.SourceType = value;
                    OnPropertyChanged(nameof(SourceType));
                }
            }
        }

        public string DestType
        {
            get
            {
                IsDestFacility = String.Equals(Assignment.DestType, SourceDestType.Facility);
                IsDestTransport = String.Equals(Assignment.DestType, SourceDestType.Transport);
                IsDestApplicator = String.Equals(Assignment.DestType, SourceDestType.Applicator);
                return Assignment.DestType;
            }
            set
            {
                if (Assignment.DestType != value)
                {
                    Assignment.DestType = value;
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
            get { return Assignment.IsSourceFacility; }
            set
            {
                if (Assignment.IsSourceFacility != value)
                {
                    Assignment.IsSourceFacility = value;
                    OnPropertyChanged(nameof(IsSourceFacility));
                }
            }
        }

        public Facility SourceFacility
        {
            get
            {
                SourceFacilityTanks = GetFacilityTanks(Assignment.SourceFacility);
                return Assignment.SourceFacility;
            }
            set
            {
                if (Assignment.SourceFacility != value)
                {
                    Assignment.SourceFacility = value;
                    OnPropertyChanged(nameof(SourceFacility));
                }
            }
        }

        public FacilityTank SourceFacilityTank
        {
            get { return Assignment.SourceFacilityTank; }
            set
            {
                if (Assignment.SourceFacilityTank != value)
                {
                    Assignment.SourceFacilityTank = value;
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
            get { return Assignment.IsDestFacility; }
            set
            {
                if (Assignment.IsDestFacility != value)
                {
                    Assignment.IsDestFacility = value;
                    OnPropertyChanged(nameof(IsDestFacility));
                }
            }
        }

        public Facility DestFacility
        {
            get
            {
                DestFacilityTanks = GetFacilityTanks(Assignment.DestFacility);
                return Assignment.DestFacility;
            }
            set
            {
                if (Assignment.DestFacility != value)
                {
                    Assignment.DestFacility = value;
                    OnPropertyChanged(nameof(DestFacility));
                }
            }
        }

        public FacilityTank DestFacilityTank
        {
            get { return Assignment.DestFacilityTank; }
            set
            {
                if (Assignment.DestFacilityTank != value)
                {
                    Assignment.DestFacilityTank = value;
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
            get { return Assignment.IsSourceTransport; }
            set
            {
                if (Assignment.IsSourceTransport != value)
                {
                    Assignment.IsSourceTransport = value;
                    OnPropertyChanged(nameof(IsSourceTransport));
                }
            }
        }

        public Transport SourceTransport
        {
            get
            {
                SourceTransportTanks = GetTransportTanks(Assignment.SourceTransport);
                return Assignment.SourceTransport; 
            }
            set
            {
                if (Assignment.SourceTransport != value)
                {
                    Assignment.SourceTransport = value;
                    OnPropertyChanged(nameof(SourceTransport));
                }
            }
        }

        public TransportTank SourceTransportTank
        {
            get { return Assignment.SourceTransportTank; }
            set
            {
                if (Assignment.SourceTransportTank != value)
                {
                    Assignment.SourceTransportTank = value;
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
            get { return Assignment.IsDestTransport; }
            set
            {
                if (Assignment.IsDestTransport != value)
                {
                    Assignment.IsDestTransport = value;
                    OnPropertyChanged(nameof(IsDestTransport));
                }
            }
        }

        public Transport DestTransport
        {
            get
            {
                DestTransportTanks = GetTransportTanks(Assignment.DestTransport);
                return Assignment.DestTransport; 
            }
            set
            {
                if (Assignment.DestTransport != value)
                {
                    Assignment.DestTransport = value;
                    OnPropertyChanged(nameof(DestTransport));
                }
            }
        }

        public TransportTank DestTransportTank
        {
            get { return Assignment.DestTransportTank; }
            set
            {
                if (Assignment.DestTransportTank != value)
                {
                    Assignment.DestTransportTank = value;
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
            get { return Assignment.IsSourceApplicator; }
            set
            {
                if (Assignment.IsSourceApplicator != value)
                {
                    Assignment.IsSourceApplicator = value;
                    OnPropertyChanged(nameof(IsSourceApplicator));
                }
            }
        }

        public Applicator SourceApplicator
        {
            get 
            {
                SourceApplicatorTanks = GetApplicatorTanks(Assignment.SourceApplicator);
                return Assignment.SourceApplicator; 
            }
            set
            {
                if (Assignment.SourceApplicator != value)
                {
                    Assignment.SourceApplicator = value;
                    OnPropertyChanged(nameof(SourceApplicator));
                }
            }
        }

        public ApplicatorTank SourceApplicatorTank
        {
            get { return Assignment.SourceApplicatorTank; }
            set
            {
                if (Assignment.SourceApplicatorTank != value)
                {
                    Assignment.SourceApplicatorTank = value;
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
            get { return Assignment.IsDestApplicator; }
            set
            {
                if (Assignment.IsDestApplicator != value)
                {
                    Assignment.IsDestApplicator = value;
                    OnPropertyChanged(nameof(IsDestApplicator));
                }
            }
        }

        public Applicator DestApplicator
        {
            get 
            {
                DestApplicatorTanks = GetApplicatorTanks(Assignment.DestApplicator);
                return Assignment.DestApplicator; 
            }
            set
            {
                if (Assignment.DestApplicator != value)
                {
                    Assignment.DestApplicator = value;
                    OnPropertyChanged(nameof(DestApplicator));
                }
            }
        }

        public ApplicatorTank DestApplicatorTank
        {
            get { return Assignment.DestApplicatorTank; }
            set
            {
                if (Assignment.DestApplicatorTank != value)
                {
                    Assignment.DestApplicatorTank = value;
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
            get { return Assignment.AgrYear; }
            set
            {
                if (Assignment.AgrYear != value)
                {
                    Assignment.AgrYear = value;
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
            get { return Assignment.Field; }
            set
            {
                if (Assignment.Field != value)
                {
                    Assignment.Field = value;
                    OnPropertyChanged(nameof(Field));
                }
            }
        }

        public ObservableCollection<Field> Fields
        {
            get { return fields; }
            set { SetProperty(ref fields, value); }
        }

        public double? VolumeRate
        {
            get { return Assignment.VolumeRate; }
            set
            {
                if (Assignment.VolumeRate != value)
                {
                    Assignment.VolumeRate = value;
                    OnPropertyChanged(nameof(VolumeRate));
                }
            }
        }

        public string Type
        {
            get { return Assignment.Type; }
            set
            {
                if (Assignment.Type != value)
                {
                    Assignment.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public ObservableCollection<string> TypeList
        {
            get { return new ObservableCollection<string>(CAssignmentType.GetList()); }
        }

        public double? Size
        {
            get { return Assignment.Size; }
            set
            {
                if (Assignment.Size != value)
                {
                    Assignment.Size = value;
                    OnPropertyChanged(nameof(Size));
                }
            }
        }

        public bool IsValid
        {
            get { return (!String.IsNullOrEmpty(Name) && (Recipe != null) && (VolumeRate != null)); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Commands
        private void ClearRecipe()
        {
            Recipe = null;
        }

        private void ClearSourceType()
        {
            ClearSourceFacility();
            ClearSourceTransport();
            ClearSourceApplicator();
            SourceType = null;
        }

        private void ClearSourceFacility()
        {
            ClearSourceFacilityTank();
            SourceFacility = null;
        }

        private void ClearSourceTransport()
        {
            ClearSourceTransportTank();
            SourceTransport = null;
        }

        private void ClearSourceApplicator()
        {
            ClearSourceApplicatorTank();
            SourceApplicator = null;
        }

        private void ClearSourceFacilityTank()
        {
            SourceFacilityTank = null;
        }

        private void ClearSourceTransportTank()
        {
            SourceTransportTank = null;
        }

        private void ClearSourceApplicatorTank()
        {
            SourceApplicatorTank = null;
        }

        private void ClearDestType()
        {
            ClearDestFacility();
            ClearDestTransport();
            ClearDestApplicator();
            DestType = null;
        }

        private void ClearDestFacility()
        {
            ClearDestFacilityTank();
            DestFacility = null;
        }

        private void ClearDestTransport()
        {
            ClearDestTransportTank();
            DestTransport = null;
        }

        private void ClearDestApplicator()
        {
            ClearDestApplicatorTank();
            DestApplicator = null;
        }

        private void ClearDestFacilityTank()
        {
            DestFacilityTank = null;
        }

        private void ClearDestTransportTank()
        {
            DestTransportTank = null;
        }

        private void ClearDestApplicatorTank()
        {
            DestApplicatorTank = null;
        }

        private void ClearAgrYear()
        {
            AgrYear = null;
        }

        private void ClearField()
        {
            Field = null;
        }
        #endregion Commands

        #region Methods
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
            Recipe = Recipes.FirstOrDefault(r => r.RecipeId == Assignment.RecipeId);
            AgrYear = AgrYears.FirstOrDefault(ay => ay.AgrYearId == Assignment.AgrYearId);
            Field = Fields.FirstOrDefault(f => f.FieldId == Assignment.FieldId);

            SourceFacility = Facilities.FirstOrDefault(f => f.FacilityId == Assignment.SourceFacilityId);
            DestFacility = Facilities.FirstOrDefault(f => f.FacilityId == Assignment.DestFacilityId);
            SourceTransport = Transports.FirstOrDefault(t => t.TransportId == Assignment.SourceTransportId);
            DestTransport = Transports.FirstOrDefault(t => t.TransportId == Assignment.DestTransportId);
            SourceApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Assignment.SourceApplicatorId);
            DestApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Assignment.DestApplicatorId);

            SourceFacilityTanks = GetFacilityTanks(SourceFacility);
            DestFacilityTanks = GetFacilityTanks(DestFacility);
            SourceTransportTanks = GetTransportTanks(SourceTransport);
            DestTransportTanks = GetTransportTanks(DestTransport);
            SourceApplicatorTanks = GetApplicatorTanks(SourceApplicator);
            DestApplicatorTanks = GetApplicatorTanks(DestApplicator);

            SourceFacilityTank = SourceFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Assignment.SourceFacilityTankId);
            DestFacilityTank = DestFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Assignment.DestFacilityTankId);
            SourceTransportTank = SourceTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Assignment.SourceTransportTankId);
            DestTransportTank = DestTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Assignment.DestTransportTankId);
            SourceApplicatorTank = SourceApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Assignment.SourceApplicatorTankId);
            DestApplicatorTank = DestApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Assignment.DestApplicatorTankId);
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
        #endregion Methods
    }
}

using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DosingApp.ViewModels
{
    public class MixtureViewModel : BaseViewModel
    {
        #region Attributes
        MixturesViewModel mixturesViewModel;
        public Mixture Mixture { get; private set; }
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
        #endregion Attributes

        #region Constructor
        public MixtureViewModel(Mixture mixture)
        {
            Mixture = mixture;
        }
        #endregion Constructor

        #region Properties
        public MixturesViewModel MixturesViewModel
        {
            get { return mixturesViewModel; }
            set { SetProperty(ref mixturesViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (Mixture.MixtureId == 0) ? "Новое задание" : "Задание: " + Mixture.Name;
                return Mixture.Name; 
            }
            set
            {
                if (Mixture.Name != value)
                {
                    Mixture.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Note
        {
            get { return Mixture.Note; }
            set
            {
                if (Mixture.Note != value)
                {
                    Mixture.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public Recipe Recipe
        {
            get { return Mixture.Recipe; }
            set
            {
                if (Mixture.Recipe != value)
                {
                    Mixture.Recipe = value;
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
                IsSourceFacility = String.Equals(Mixture.SourceType, SourceDestType.Facility);
                IsSourceTransport = String.Equals(Mixture.SourceType, SourceDestType.Transport);
                IsSourceApplicator = String.Equals(Mixture.SourceType, SourceDestType.Applicator);
                return Mixture.SourceType; 
            }
            set 
            {
                if (Mixture.SourceType != value)
                {
                    Mixture.SourceType = value;
                    OnPropertyChanged(nameof(SourceType));
                }
            }
        }

        public string DestType
        {
            get
            {
                IsDestFacility = String.Equals(Mixture.DestType, SourceDestType.Facility);
                IsDestTransport = String.Equals(Mixture.DestType, SourceDestType.Transport);
                IsDestApplicator = String.Equals(Mixture.DestType, SourceDestType.Applicator);
                return Mixture.DestType;
            }
            set
            {
                if (Mixture.DestType != value)
                {
                    Mixture.DestType = value;
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
            get { return Mixture.IsSourceFacility; }
            set
            {
                if (Mixture.IsSourceFacility != value)
                {
                    Mixture.IsSourceFacility = value;
                    OnPropertyChanged(nameof(IsSourceFacility));
                }
            }
        }

        public Facility SourceFacility
        {
            get
            {
                SourceFacilityTanks = GetFacilityTanks(Mixture.SourceFacility);
                return Mixture.SourceFacility;
            }
            set
            {
                if (Mixture.SourceFacility != value)
                {
                    Mixture.SourceFacility = value;
                    OnPropertyChanged(nameof(SourceFacility));
                }
            }
        }

        public FacilityTank SourceFacilityTank
        {
            get { return Mixture.SourceFacilityTank; }
            set
            {
                if (Mixture.SourceFacilityTank != value)
                {
                    Mixture.SourceFacilityTank = value;
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
            get { return Mixture.IsDestFacility; }
            set
            {
                if (Mixture.IsDestFacility != value)
                {
                    Mixture.IsDestFacility = value;
                    OnPropertyChanged(nameof(IsDestFacility));
                }
            }
        }

        public Facility DestFacility
        {
            get
            {
                DestFacilityTanks = GetFacilityTanks(Mixture.DestFacility);
                return Mixture.DestFacility;
            }
            set
            {
                if (Mixture.DestFacility != value)
                {
                    Mixture.DestFacility = value;
                    OnPropertyChanged(nameof(DestFacility));
                }
            }
        }

        public FacilityTank DestFacilityTank
        {
            get { return Mixture.DestFacilityTank; }
            set
            {
                if (Mixture.DestFacilityTank != value)
                {
                    Mixture.DestFacilityTank = value;
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
            get { return Mixture.IsSourceTransport; }
            set
            {
                if (Mixture.IsSourceTransport != value)
                {
                    Mixture.IsSourceTransport = value;
                    OnPropertyChanged(nameof(IsSourceTransport));
                }
            }
        }

        public Transport SourceTransport
        {
            get
            {
                SourceTransportTanks = GetTransportTanks(Mixture.SourceTransport);
                return Mixture.SourceTransport; 
            }
            set
            {
                if (Mixture.SourceTransport != value)
                {
                    Mixture.SourceTransport = value;
                    OnPropertyChanged(nameof(SourceTransport));
                }
            }
        }

        public TransportTank SourceTransportTank
        {
            get { return Mixture.SourceTransportTank; }
            set
            {
                if (Mixture.SourceTransportTank != value)
                {
                    Mixture.SourceTransportTank = value;
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
            get { return Mixture.IsDestTransport; }
            set
            {
                if (Mixture.IsDestTransport != value)
                {
                    Mixture.IsDestTransport = value;
                    OnPropertyChanged(nameof(IsDestTransport));
                }
            }
        }

        public Transport DestTransport
        {
            get
            {
                DestTransportTanks = GetTransportTanks(Mixture.DestTransport);
                return Mixture.DestTransport; 
            }
            set
            {
                if (Mixture.DestTransport != value)
                {
                    Mixture.DestTransport = value;
                    OnPropertyChanged(nameof(DestTransport));
                }
            }
        }

        public TransportTank DestTransportTank
        {
            get { return Mixture.DestTransportTank; }
            set
            {
                if (Mixture.DestTransportTank != value)
                {
                    Mixture.DestTransportTank = value;
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
            get { return Mixture.IsSourceApplicator; }
            set
            {
                if (Mixture.IsSourceApplicator != value)
                {
                    Mixture.IsSourceApplicator = value;
                    OnPropertyChanged(nameof(IsSourceApplicator));
                }
            }
        }

        public Applicator SourceApplicator
        {
            get 
            {
                SourceApplicatorTanks = GetApplicatorTanks(Mixture.SourceApplicator);
                return Mixture.SourceApplicator; 
            }
            set
            {
                if (Mixture.SourceApplicator != value)
                {
                    Mixture.SourceApplicator = value;
                    OnPropertyChanged(nameof(SourceApplicator));
                }
            }
        }

        public ApplicatorTank SourceApplicatorTank
        {
            get { return Mixture.SourceApplicatorTank; }
            set
            {
                if (Mixture.SourceApplicatorTank != value)
                {
                    Mixture.SourceApplicatorTank = value;
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
            get { return Mixture.IsDestApplicator; }
            set
            {
                if (Mixture.IsDestApplicator != value)
                {
                    Mixture.IsDestApplicator = value;
                    OnPropertyChanged(nameof(IsDestApplicator));
                }
            }
        }

        public Applicator DestApplicator
        {
            get 
            {
                DestApplicatorTanks = GetApplicatorTanks(Mixture.DestApplicator);
                return Mixture.DestApplicator; 
            }
            set
            {
                if (Mixture.DestApplicator != value)
                {
                    Mixture.DestApplicator = value;
                    OnPropertyChanged(nameof(DestApplicator));
                }
            }
        }

        public ApplicatorTank DestApplicatorTank
        {
            get { return Mixture.DestApplicatorTank; }
            set
            {
                if (Mixture.DestApplicatorTank != value)
                {
                    Mixture.DestApplicatorTank = value;
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
            get { return Mixture.AgrYear; }
            set
            {
                if (Mixture.AgrYear != value)
                {
                    Mixture.AgrYear = value;
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
            get { return Mixture.Field; }
            set
            {
                if (Mixture.Field != value)
                {
                    Mixture.Field = value;
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
            get { return Mixture.VolumeRate; }
            set
            {
                if (Mixture.VolumeRate != value)
                {
                    Mixture.VolumeRate = value;
                    OnPropertyChanged(nameof(VolumeRate));
                }
            }
        }

        public string Unit
        {
            get { return Mixture.Unit; }
            set
            {
                if (Mixture.Unit != value)
                {
                    Mixture.Unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public ObservableCollection<string> UnitList
        {
            get { return new ObservableCollection<string>(RecipeComponentUnit.GetList()); }
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
            Recipe = Recipes.FirstOrDefault(r => r.RecipeId == Mixture.RecipeId);
            AgrYear = AgrYears.FirstOrDefault(ay => ay.AgrYearId == Mixture.AgrYearId);
            Field = Fields.FirstOrDefault(f => f.FieldId == Mixture.FieldId);

            SourceFacility = Facilities.FirstOrDefault(f => f.FacilityId == Mixture.SourceFacilityId);
            DestFacility = Facilities.FirstOrDefault(f => f.FacilityId == Mixture.DestFacilityId);
            SourceTransport = Transports.FirstOrDefault(t => t.TransportId == Mixture.SourceTransportId);
            DestTransport = Transports.FirstOrDefault(t => t.TransportId == Mixture.DestTransportId);
            SourceApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Mixture.SourceApplicatorId);
            DestApplicator = Applicators.FirstOrDefault(a => a.ApplicatorId == Mixture.DestApplicatorId);

            SourceFacilityTanks = GetFacilityTanks(SourceFacility);
            DestFacilityTanks = GetFacilityTanks(DestFacility);
            SourceTransportTanks = GetTransportTanks(SourceTransport);
            DestTransportTanks = GetTransportTanks(DestTransport);
            SourceApplicatorTanks = GetApplicatorTanks(SourceApplicator);
            DestApplicatorTanks = GetApplicatorTanks(DestApplicator);

            SourceFacilityTank = SourceFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Mixture.SourceFacilityTankId);
            DestFacilityTank = DestFacilityTanks?.FirstOrDefault(ft => ft.FacilityTankId == Mixture.DestFacilityTankId);
            SourceTransportTank = SourceTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Mixture.SourceTransportTankId);
            DestTransportTank = DestTransportTanks?.FirstOrDefault(tt => tt.TransportTankId == Mixture.DestTransportTankId);
            SourceApplicatorTank = SourceApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Mixture.SourceApplicatorTankId);
            DestApplicatorTank = DestApplicatorTanks?.FirstOrDefault(at => at.ApplicatorTankId == Mixture.DestApplicatorTankId);
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

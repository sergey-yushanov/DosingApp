using DosingApp.Models;

namespace DosingApp.ViewModels
{
    public class TransportTankViewModel : BaseViewModel
    {
        #region Attributes
        TransportViewModel transportViewModel;
        public TransportTank TransportTank { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public TransportTankViewModel(TransportTank transportTank)
        {
            TransportTank = transportTank;
        }
        #endregion Constructor

        #region Properties
        public TransportViewModel TransportViewModel
        {
            get { return transportViewModel; }
            set { SetProperty(ref transportViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Транспорт: " + TransportViewModel.Transport.Name + ((TransportTank.TransportTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + TransportTank.Name);
                return TransportTank.Name;
            }
            set
            {
                if (TransportTank.Name != value)
                {
                    TransportTank.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public double? Volume
        {
            get { return TransportTank.Volume; }
            set
            {
                if (TransportTank.Volume != value)
                {
                    TransportTank.Volume = value;
                    OnPropertyChanged(nameof(Volume));
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

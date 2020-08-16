using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class MixturesViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Assignment> assignments;
        private Assignment selectedAssignment;

        public ICommand SaveCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixturesViewModel()
        {
            SaveCommand = new Command(SaveMixture);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Assignment> Assignments
        {
            get { return assignments; }
            set { SetProperty(ref assignments, value); }
        }

        public Assignment SelectedAssignment
        {
            get { return selectedAssignment; }
            set
            {
                if (selectedAssignment != value)
                {
                    Mixture newMixture = new Mixture() { Assignment = value };
                    MixtureViewModel tempMixture = new MixtureViewModel(newMixture) { MixturesViewModel = this };
                    selectedAssignment = null;
                    OnPropertyChanged(nameof(SelectedAssignment));
                    Application.Current.MainPage.Navigation.PushAsync(new MixturePage(tempMixture));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void SaveMixture(object mixtureInstance)
        {
            MixtureViewModel mixtureViewModel = mixtureInstance as MixtureViewModel;
            if (mixtureViewModel.Mixture != null && mixtureViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (mixtureViewModel.Mixture.MixtureId == 0)
                    {
                        db.Entry(mixtureViewModel.Mixture).State = EntityState.Added;
                    }
                    else
                    {
                        db.Mixtures.Update(mixtureViewModel.Mixture);
                    }
                    db.SaveChanges();
                }
            }
        }
        #endregion Commands

        #region Methods
        public void LoadAssignments()
        {
            using (AppDbContext db = App.GetContext())
            {
                Assignments = new ObservableCollection<Assignment>(db.Assignments.ToList());
            }
        }
        #endregion Methods
    }
}

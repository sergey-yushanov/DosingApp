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
    public class MixersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Mixer> mixers;
        private Mixer selectedMixer;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixersViewModel()
        {
            CreateCommand = new Command(CreateMixer);
            DeleteCommand = new Command(DeleteMixer);
            SaveCommand = new Command(SaveMixer);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Mixer> Mixers
        {
            get { return mixers; }
            set { SetProperty(ref mixers, value); }
        }

        public Mixer SelectedMixer
        {
            get { return selectedMixer; }
            set
            {
                if (selectedMixer != value)
                {
                    MixerViewModel tempMixer = new MixerViewModel(value) { MixersViewModel = this };
                    selectedMixer = null;
                    OnPropertyChanged(nameof(SelectedMixer));
                    Application.Current.MainPage.Navigation.PushAsync(new MixerPage(tempMixer));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateMixer()
        {
            Application.Current.MainPage.Navigation.PushAsync(new MixerPage(new MixerViewModel(new Mixer()) { MixersViewModel = this }));
        }

        private void DeleteMixer(object mixerInstance)
        {
            MixerViewModel mixerViewModel = mixerInstance as MixerViewModel;
            if (mixerViewModel.Mixer != null && mixerViewModel.Mixer.MixerId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Mixers.Remove(mixerViewModel.Mixer);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private async void SaveMixer(object mixerInstance)
        {
            MixerViewModel mixerViewModel = mixerInstance as MixerViewModel;
            if (mixerViewModel.Mixer != null)
            {
                if (!mixerViewModel.IsValid)
                {
                    await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название установки", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (mixerViewModel.Mixer.IsUsedMixer)
                    {
                        var usedMixer = db.Mixers.FirstOrDefault(m => m.IsUsedMixer == true);
                        if (usedMixer != null && usedMixer.MixerId != mixerViewModel.Mixer.MixerId)
                        {
                            if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Попытка изменить активную установку. Выполнить изменение?", "Да", "Нет"))
                            {
                                usedMixer.IsUsedMixer = false;
                                db.Mixers.Update(usedMixer);
                                db.SaveChanges();
                            }
                            else
                            {
                                mixerViewModel.Mixer.IsUsedMixer = false;
                            }
                        }
                    }
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (mixerViewModel.Mixer.MixerId == 0)
                    {
                        db.Entry(mixerViewModel.Mixer).State = EntityState.Added;
                    }
                    else
                    {
                        db.Mixers.Update(mixerViewModel.Mixer);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadMixers()
        {
            using (AppDbContext db = App.GetContext())
            {
                Mixers = new ObservableCollection<Mixer>(db.Mixers.ToList());
            }
        }
        #endregion Methods

    }
}

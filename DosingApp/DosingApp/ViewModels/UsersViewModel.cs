using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<User> users;
        private User selectedUser;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public UsersViewModel()
        {
            CreateCommand = new Command(CreateUser);
            DeleteCommand = new Command(DeleteUser);
            SaveCommand = new Command(SaveUser);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<User> Users 
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    UserViewModel tempUser = new UserViewModel(value) { UsersViewModel = this };
                    selectedUser = null;
                    OnPropertyChanged(nameof(SelectedUser));
                    Application.Current.MainPage.Navigation.PushAsync(new UserPage(tempUser));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateUser()
        {
            Application.Current.MainPage.Navigation.PushAsync(new UserPage(new UserViewModel(new User()) { UsersViewModel = this }));
        }

        private void DeleteUser(object userInstance)
        {
            UserViewModel userViewModel = userInstance as UserViewModel;
            if (userViewModel.User != null && userViewModel.User.UserId != 0)
            {
                using (UserDbContext db = App.GetUserContext())
                {
                    db.Users.Remove(userViewModel.User);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveUser(object userInstance)
        {
            UserViewModel userViewModel = userInstance as UserViewModel;

            if (!userViewModel.IsValidUsername)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Имя пользователя не указано", "Ok");
                return;
            }

            if (IsExistUser(userViewModel.User))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Пользователь с таким именем существует", "Ok");
                return;
            }

            if (!userViewModel.IsValidPassword)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Пароль пользователя не задан", "Ok");
                return;
            }

            if (userViewModel.User != null && userViewModel.IsValidUsername && userViewModel.IsValidPassword)
            {
                using (UserDbContext db = App.GetUserContext())
                {
                    userViewModel.User.PasswordSalt = CryptoService.GenerateSalt();
                    userViewModel.User.PasswordHash = CryptoService.ComputeHash(userViewModel.Password, userViewModel.User.PasswordSalt);

                    if (userViewModel.User.UserId == 0)
                    {
                        db.Entry(userViewModel.User).State = EntityState.Added;
                    }
                    else
                    {
                        db.Users.Update(userViewModel.User);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadUsers()
        {
            using (UserDbContext db = App.GetUserContext())
            {
                Users = new ObservableCollection<User>(db.Users.ToList());
            }
        }

        private bool IsExistUser(User user)
        {
            if (user != null)
            {
                User existUser = Users.FirstOrDefault(u => u.Username == user.Username);
                return existUser != null && existUser.UserId != user.UserId;
            }
            else
            {
                return false;
            }
        }
        #endregion Methods
    }
}

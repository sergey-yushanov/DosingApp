using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        #region Attributes
        UsersViewModel usersViewModel;
        public User User { get; private set; }
        private string password;
        private string title;
        #endregion Attributes

        #region Constructor
        public UserViewModel(User user)
        {
            User = user;
        }
        #endregion Constructor

        #region Properties
        public UsersViewModel UsersViewModel
        {
            get { return usersViewModel; }
            set { SetProperty(ref usersViewModel, value); }
        }

        public string Username
        {
            get 
            {
                Title = (User.UserId == 0) ? "Новый пользователь" : "Пользователь: " + User.Username;
                return User.Username; 
            }
            set
            {
                if (User.Username != value)
                {
                    User.Username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string DisplayName
        {
            get { return User.DisplayName; }
            set
            {
                if (User.DisplayName != value)
                {
                    User.DisplayName = value;
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public bool AccessJobParams
        {
            get { return User.AccessJobParams; }
            set
            {
                if (User.AccessJobParams != value)
                {
                    User.AccessJobParams = value;
                    OnPropertyChanged(nameof(AccessJobParams));
                }
            }
        }

        public bool AccessMainMenu
        {
            get { return User.AccessMainMenu; }
            set
            {
                if (User.AccessMainMenu != value)
                {
                    User.AccessMainMenu = value;
                    OnPropertyChanged(nameof(AccessMainMenu));
                }
            }
        }

        public bool AccessMainParams
        {
            get { return User.AccessMainParams; }
            set
            {
                if (User.AccessMainParams != value)
                {
                    User.AccessMainParams = value;
                    OnPropertyChanged(nameof(AccessMainParams));
                }
            }
        }

        public bool AccessAdditionalParams
        {
            get { return User.AccessAdditionalParams; }
            set
            {
                if (User.AccessAdditionalParams != value)
                {
                    User.AccessAdditionalParams = value;
                    OnPropertyChanged(nameof(AccessAdditionalParams));
                }
            }
        }

        public bool AccessAdminParams
        {
            get { return User.AccessAdminParams; }
            set
            {
                if (User.AccessAdminParams != value)
                {
                    User.AccessAdminParams = value;
                    OnPropertyChanged(nameof(AccessAdminParams));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (IsAdminUsername && App.IsActiveUserAdmin())
                {
                    SetProperty(ref password, value);
                }

                if (!IsAdminUsername && App.ActiveUser.AccessAdminParams)
                {
                    SetProperty(ref password, value);
                }
            }
        }

        public bool IsValidUsername
        {
            get { return (!String.IsNullOrEmpty(Username)); }
        }

        public bool IsValidPassword
        {
            get { return (!String.IsNullOrEmpty(Password)); }
        }

        public bool IsAdminUsername
        {
            get { return Admin.IsAdminUsername(Username); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}

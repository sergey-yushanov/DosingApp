using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models
{
    class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        public User() { }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            return (!String.IsNullOrEmpty(this.Username) && !String.IsNullOrEmpty(this.Password));
        }
    }
}

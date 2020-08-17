using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models
{
    public static class Admin
    {
        public const string Username = "admin";
        public const string DisplayName = "admin";
        public const string Password = "admin";

        public static User GetUser()
        {
            User user = new User();
            user.Username = Username;
            user.DisplayName = DisplayName;
            user.PasswordSalt = CryptoService.GenerateSalt();
            user.PasswordHash = CryptoService.ComputeHash(Password, user.PasswordSalt);
            user.AccessJobParams = true;
            user.AccessMainMenu = true;
            user.AccessMainParams = true;
            user.AccessAdditionalParams = true;
            user.AccessAdminParams = true;
            return user;
        }

        public static bool IsAdminUsername(string username)
        {
            return String.Equals(username, Username);
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public bool AccessJobParams { get; set; }
        public bool AccessMainMenu { get; set; }
        public bool AccessMainParams { get; set; }
        public bool AccessAdditionalParams { get; set; }
        public bool AccessAdminParams { get; set; }
    }
}

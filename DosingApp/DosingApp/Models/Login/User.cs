using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        //[Index(IsUnique = true)]
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}

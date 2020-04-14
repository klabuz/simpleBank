using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country {get;set;}
        //public string SSN { get; set; }

        public List<Account> Accounts { get; set; }
        public List<User> Contacts { get; set; }

        public User()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Accounts = new List<Account>();
            Contacts = new List<User>();
        }
    }
}
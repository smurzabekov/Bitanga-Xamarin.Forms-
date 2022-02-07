using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.Models
{
    public class User
    {
        public User()
        {
        }
        public User(string name, string phone, string iD, string email, string social, string birthday, string role)
        {
            Name = name;
            Phone = phone;
            ID = iD;
            Email = email;
            Social = social;
            Birthday = birthday;
            Role = role;
        }
        public string Role { get; set; }

        public string Name {
            get;
            set;
        }
        public string Phone {
            get;
            set;
        }
        public string ID {
            get;
            set;
        }
        public string Email {
            get;
            set;
        }
        public string Social {
            get;
            set;
        }
        public string Birthday {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.Models
{
    public class CurrentUser
    {
        public CurrentUser()
        {
        }
        public CurrentUser(string name, string phone, string iD, string email, string birthday, string social, Wallet wallet, string role)
        {
            this.wallet = wallet;
            Name = name;
            Phone = phone;
            ID = iD;
            Email = email;
            Birthday = birthday;
            Social = social;
            Role = role;
        }
        public string Role { get; }
        public Wallet wallet { get; }
        public string Name {
            get;
        }
        public string Phone {
            get;
        }
        public string ID {
            get;
        }
        public string Email {
            get;
        }
        public string Birthday {
            get;
        }
        public string Social {
            get;
        }
        public override string ToString() => $"CURRENT USER {Name} {Email} {ID} {Phone} {wallet.ID}: {wallet.Balance} {Birthday}";
    }
}

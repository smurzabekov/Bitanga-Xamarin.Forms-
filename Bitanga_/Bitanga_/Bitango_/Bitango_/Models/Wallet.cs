using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.Models
{
    /// <summary>
    /// Кошелек пользователя в iiko biz
    /// </summary>
    public class Wallet
    {
        public Wallet(int balance, string iD)
        {
            Balance = balance;
            ID = iD;
        }
        /// <summary>
        /// Баланс кошелька пользователя
        /// </summary>
        public int Balance { get; }
        /// <summary>
        /// ID пользователя
        /// </summary>
        public string ID { get; }

    }
}

using RentSystem.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class Account
    {
        
        public int ID { get; set; }
        public int AccountID { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }
        public AccountState State { get; set; }
    }
}

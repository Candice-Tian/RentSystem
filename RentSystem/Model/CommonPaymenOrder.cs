using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class CommonPaymenOrder
    {
        public int Id { get; set; }
        public string PaymentAccount { get; set; }
        public decimal Money { get; set; }

        public PayType payType { get; set; }
    }
}

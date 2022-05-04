using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class RefundOrder
    {
        public int Id { get; set; }

        public string OrderId { get; set; }

        public int AccountId { get; set; }

        public int Money { get; set; }

        public PayType PayType { get; set; }

        public RefundOrderState State { get; set; }

        public string PaymentAccount { get; set; }
    }
}

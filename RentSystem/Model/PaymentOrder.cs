using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class PaymentOrder
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool InvoiceApply { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string Message { get; set; }

        public bool Status { get; set; }

    }
}

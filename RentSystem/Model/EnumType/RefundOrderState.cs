using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public enum RefundOrderState
    {
        Init=0,
        HasSentRequest=1,
        SuccessResponse=2,
        FailedResponse=3,
        FaildWithoutResponse=4

    }

}

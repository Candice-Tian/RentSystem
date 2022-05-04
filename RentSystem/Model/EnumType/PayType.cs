using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayType
    {
        NotSet=0,
        AliPay=1,
        WeiXinPay=2,
        PayPal=3
    }
}

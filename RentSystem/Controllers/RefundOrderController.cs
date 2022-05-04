using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RentSystem.Model;
using RentSystem.Service;

namespace RentSystem.Controllers
{
    [ApiController]
    public class RefundOrderController : CommonController
    {
        public IOrderService _orderService;

        public RefundOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("precharge/refund-orders")]
        public ApiResult<OrderResponse> RefundOrder([FromBody]RefundOrder refundOrder)
        {
            return GenActionResultGenericEx(_orderService.RefundMoney(refundOrder));
        }
    }
}

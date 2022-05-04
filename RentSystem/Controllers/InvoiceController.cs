using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Model;
using RentSystem.Service;

namespace RentSystem.Controllers
{
    [ApiController]
    public class InvoiceController : CommonController
    {
        public IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("precharge/invoice-order")]
        public ApiResult<bool> PostInvoice(string orderId)
        {
            var applyResult = _invoiceService.PostInvoice(orderId);
            return GenActionResultGenericEx<bool>(applyResult.Item2, applyResult.Item2 ? ApiResultCode.Success : ApiResultCode.Failed, applyResult.Item1);
        }
    }
}

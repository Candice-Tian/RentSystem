using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Model;

namespace RentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        protected ApiResult<T> GenActionResultGenericEx<T>(T datas, ApiResultCode code = ApiResultCode.Success, string message = "", Exception ex = null)
        {
            if (ex != null)
            {
                code = ApiResultCode.Failed;
                message = string.IsNullOrWhiteSpace(message) ? ex.Message : $"{message}\n{ex.Message}";

            }
            else if (datas == null && code == ApiResultCode.Success)
            {
                message = "成功";
            }
            var result = new ApiResult<T>
            {
                Code = code,
                Message = message,
                Data = datas
            };
            return result;
        }

    }
}

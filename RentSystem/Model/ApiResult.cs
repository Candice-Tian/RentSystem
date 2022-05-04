using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Model
{
    public class ApiResult<T>
    {
        public ApiResultCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

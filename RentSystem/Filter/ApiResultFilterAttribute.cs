using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Filter
{
    public class ApiResultFilterAttribute:Attribute,IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var isDefined = controllerActionDescriptor.EndpointMetadata.Any(a => a.GetType().Equals(typeof(ApiIgnoreAttribute)));
                if (isDefined)
                {
                    return;
                }

            }
            // 返回结果为JsonResult的请求进行Result包装
            if (context.Result != null)
            {
                if (context.Result is ObjectResult)
                {
                    var result = context.Result as ObjectResult;
                    context.Result = new JsonResult(new { code = 200, msg = "success", data = result.Value });

                }
                else if (context.Result is EmptyResult)
                {
                    context.Result = new JsonResult(new { code = 200, msg = "success", data = new { } });
                }
                else if (context.Result is ContentResult)
                {
                    var result = context.Result as ContentResult;
                    context.Result = new JsonResult(new { code = result.StatusCode, msg = result.Content });
                }
                else
                {
                    throw new Exception($"未经处理的Result类型：{ context.Result.GetType().Name}");
                }

            }
        }
        /// <summary>
        /// 执行方法体之前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //不做修改
        }

    }
}

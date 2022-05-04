using FluentAssertions;
using NSubstitute;
using RentSystem.Controllers;
using RentSystem.Model;
using RentSystem.Service;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.Data;
using Xunit;

namespace UnitTest.Controller
{
    public class RefundOrderControllerTest
    {
        public static IOrderService orderSerice = Substitute.For<IOrderService>();
        public RefundOrderController refundOrderController = new RefundOrderController(orderSerice);
       
        [Fact]
        public void RefundOrder_ShouldReturnCorrect_WhenServiceReturnTrue()
        {
            var refundOrder= TestData.ConstructRefundOrder();
            var serviceResponse = new OrderResponse() { Message = "订单申请退款成功", Status = true };
            orderSerice.RefundMoney(refundOrder).Returns(serviceResponse);

            var result = refundOrderController.RefundOrder(refundOrder);
            var expectedResult = new ApiResult<OrderResponse>() { Code = ApiResultCode.Success, Message = "", Data = serviceResponse };

            result.Should().BeEquivalentTo(expectedResult);
        }

    }
}

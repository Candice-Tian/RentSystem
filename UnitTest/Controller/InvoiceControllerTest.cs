using FluentAssertions;
using NSubstitute;
using RentSystem.Controllers;
using RentSystem.Model;
using RentSystem.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest.Controller
{
    public class InvoiceControllerTest
    {
        public static IInvoiceService invoiceService = Substitute.For<IInvoiceService>();
        public InvoiceController invoiceController = new InvoiceController(invoiceService);

        [Fact]
        public void PostInvoice_ShouldReturnCorrect_WhenServiceReturnTrue()
        {
            string orderId = "001";
            Tuple<string, bool> invoiceResponse = new Tuple<string, bool>("test", true);
            invoiceService.PostInvoice(orderId).Returns(invoiceResponse);

            var result = invoiceController.PostInvoice(orderId);
            var expectedResult = new ApiResult<bool>() { Code = ApiResultCode.Success, Message = "test", Data = true };

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

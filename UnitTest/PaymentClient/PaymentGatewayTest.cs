using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using RentSystem.Model;
using RentSystem.PaymentClient;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnitTest.Data;
using Xunit;

namespace UnitTest.PaymentClient
{
    public class PaymentGatewayTest
    {
        [Fact]
        public void RefundMoneyOrderAsync_ShouldReturnTrue_WhenCallPaymentGatewaySuccess()
        {
            //Setup
            var order = TestData.ConstructRefundOrder();
            var paymentResult = new CommonPaymenResponse() { Status = true };
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(paymentResult)) 
                }); 

            var client = new HttpClient(mockHttpMessageHandler.Object);


            //Act
            var sut = new PaymentGatewayClient(client);

            var result = sut.RefundMoneyOrder(order);

            //Assert
            var expectedResult = new CommonPaymenResponse() {Status=true };
            result.Should().BeEquivalentTo(expectedResult);

        }


        [Fact]
        public void RefundMoneyOrderAsync_ShouldReturnTrue_WhenCallPaymentGatewayFailed()
        {
            //Setup
            var order = TestData.ConstructRefundOrder();
            var paymentResult = new CommonPaymenResponse() { Status = true };
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.GatewayTimeout,
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);


            //Act
            var sut = new PaymentGatewayClient(client);

            var result = sut.RefundMoneyOrder(order);

            //Assert
            var expectedResult = new CommonPaymenResponse() { Message = HttpStatusCode.GatewayTimeout.ToString() };
            result.Should().BeEquivalentTo(expectedResult);

        }
    }
}

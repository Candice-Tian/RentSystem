using FluentAssertions;
using RentSystem.Model;
using RentSystem.Repository;
using Xunit;

namespace UnitTest.Repository
{
    public class RefundOrderRepositoryTest
    {
        private RefundOrderRepository testSub = new RefundOrderRepository();
        [Fact]
        public void PostRefundOrder_ShouldReturnTrue_WhenInsertDBSuccess()
        {
            var refundOrder = new RefundOrder() {Id=1,AccountId=1,Money=300,PayType=PayType.AliPay,State=RefundOrderState.Init,PaymentAccount="12345678901",OrderId="test001" };
            var result = testSub.PostRefundOrder(refundOrder);

            Assert.True(result);

        }

        [Fact]
        public void PostRefundOrder_ShouldChangeValue_WhenValueExistedInDB()
        {
            var refundOrder = new RefundOrder() { AccountId = 2, Money = 300, PayType = PayType.AliPay, State = RefundOrderState.SuccessResponse, PaymentAccount = "12345678901", OrderId = "test001" };
             testSub.PostRefundOrder(refundOrder);

            refundOrder.State = RefundOrderState.HasSentRequest;
            var result = testSub.PostRefundOrder(refundOrder);

            var actualValue = testSub.GetOrderByOrderId(refundOrder.OrderId);

            Assert.True(result);
            Assert.NotNull(actualValue);
            actualValue.State.Should().Be(refundOrder.State);

        }
    }
}

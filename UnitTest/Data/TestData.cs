using RentSystem.Model;

namespace UnitTest.Data
{
    public static class TestData
    {
        public static RefundOrder ConstructRefundOrder()
        {
            return new RefundOrder()
            {
                Id = 1,
                AccountId = 1,
                Money = 600,
                OrderId = "test001",
                PaymentAccount = "12345678901",
                PayType = PayType.AliPay,
                State = RefundOrderState.Init
            };
        }
    }
}

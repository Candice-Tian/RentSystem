using FluentAssertions;
using RentSystem.Model;
using RentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest.Repository
{
    public class PaymentOrderRepositoryTest
    {
        private PaymentOrderRepository testSub = new PaymentOrderRepository();

        public PaymentOrderRepositoryTest()
        {
            string SQL = "TRUNCATE TABLE  PaymentOrder";
            DBContext.Execute(SQL);

            string insertSQL = "Insert Into PaymentOrder Values(1,'001',1,'2019-01-01',0)";
            DBContext.Execute(insertSQL);
        }

        [Fact]
        public void GetPaymentOrderByOrderId_ShouldReturnCorrectResult_WhenDBHasValue()
        {
            var expectedResult = new PaymentOrder() { AccountId = 1, Id = 1, OrderId = "001", InvoiceApply = false, CreateTime = new DateTime(2019, 1, 1) };
            var result = testSub.GetPaymentOrderByOrderId(expectedResult.OrderId);

            result.CreateTime.Should().Be(expectedResult.CreateTime);
            result.AccountId.Should().Be(expectedResult.AccountId);
            result.OrderId.Trim().Should().Be(expectedResult.OrderId);
            result.InvoiceApply.Should().Be(expectedResult.InvoiceApply);
        }

        [Fact]
        public void PostPaymentOrderById_ShouldeUpdateSuccess()
        {
            string orderId = "001";

            var originalOrder = testSub.GetPaymentOrderByOrderId(orderId);

            originalOrder.InvoiceApply.Should().Be(false);

            originalOrder.InvoiceApply = true;
            testSub.PostPaymentOrderById(originalOrder);
            var updatedOrder = testSub.GetPaymentOrderByOrderId(orderId);

            updatedOrder.InvoiceApply.Should().Be(true);

        }
    }
}

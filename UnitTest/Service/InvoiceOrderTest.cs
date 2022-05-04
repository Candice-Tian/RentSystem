using FluentAssertions;
using NSubstitute;
using RentSystem.Model;
using RentSystem.MQClient;
using RentSystem.Repository;
using RentSystem.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest.Service
{
    public class InvoiceTest
    {
        public IPaymentOrderRepository paymentOrderRepository;
        public IMQClient mqClient;
        public InvoiceService serice;

        public InvoiceTest()
        {
            paymentOrderRepository = Substitute.For<IPaymentOrderRepository>();
            mqClient = Substitute.For<IMQClient>();
            serice = new InvoiceService(paymentOrderRepository, mqClient);
        }

        [Fact]
        public void PostInvoice_ShouldReturnCorrect_WhenApplyInvoiceSuccess()
        {
            string orderId = "test001";
            var paymentOrder = new PaymentOrder() { AccountId = 1, OrderId = orderId, CreateTime = new DateTime(2020, 1, 1), InvoiceApply = false };

            paymentOrderRepository.GetPaymentOrderByOrderId(orderId).Returns(paymentOrder);
            paymentOrderRepository.PostPaymentOrderById(paymentOrder).Returns(true);

            var result = serice.PostInvoice(orderId);

            var expectedResult = new Tuple<string, bool>("发票申请成功", true);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void PostInvoice_ShouldReturnFailed_WhenOrderHasAppliedInvoice()
        {
            string orderId = "test002";
            var paymentOrder = new PaymentOrder() { AccountId = 1, OrderId = orderId, CreateTime = new DateTime(2020, 1, 1), InvoiceApply = true };

            paymentOrderRepository.GetPaymentOrderByOrderId(orderId).Returns(paymentOrder);

            var result = serice.PostInvoice(orderId);

            var expectedResult = new Tuple<string, bool>("订单发票已开", false);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void PostInvoice_ShouldReturnFailed_WhenOrderNotExisting()
        {
            string orderId = "test002";
            PaymentOrder order = null;
            paymentOrderRepository.GetPaymentOrderByOrderId(orderId).Returns(order);

            var result = serice.PostInvoice(orderId);

            var expectedResult = new Tuple<string, bool>("订单不存在", false);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}



using FluentAssertions;
using Moq;
using NSubstitute;
using RentSystem.Model;
using RentSystem.Model.EnumType;
using RentSystem.PaymentClient;
using RentSystem.Repository;
using RentSystem.Service;
using System.Net.Http;
using UnitTest.Data;
using Xunit;

namespace UnitTest.Service
{
    public class RefundMoneyServiceTest
    {
        public IAccountRepository accountRepository;
        public IRefundOrderRepository refundOrderRepository;
        public IPaymentGatewayClient paymentGatewayClient;
        public HttpClient client;

        public RefundMoneyServiceTest()
        {
            accountRepository = Substitute.For<IAccountRepository>();
            refundOrderRepository = Substitute.For<IRefundOrderRepository>();
            client = Substitute.For<HttpClient>();
            paymentGatewayClient = Substitute.For<IPaymentGatewayClient>();

        }

        /// <summary>
        /// 合理退款
        /// </summary>
        [Fact]
        public void RefundMoney_ShouldReturnSuccess_WhenAccountNormalAndRefundSuccess()
        {        
            //setUp
            var refundOrder = TestData.ConstructRefundOrder();
            var account = new Account() { AccountID = 1, Name = "test", Money = refundOrder.Money+100 , State = AccountState.Normal};
          
            accountRepository.GetAccountById(refundOrder.AccountId).Returns(account);
            paymentGatewayClient.RefundMoneyOrder(refundOrder).Returns(new CommonPaymenResponse() { Status = true });
            refundOrderRepository.PostRefundOrderAndAccount(refundOrder, account).Returns(true);

            //Action
            var orderService = new RefundOrderService(accountRepository, refundOrderRepository, paymentGatewayClient);
            var result = orderService.RefundMoney(refundOrder);

            //Assert
            result.Status.Should().BeTrue();
            result.Message.Should().Be("申请退款成功");
        }

        /// <summary>
        /// 当余额冻结时
        /// </summary>
        [Fact]
        public void RefundMoney_ShouldReturnSuccess_WhenAccountFreeze()
        {
            //setUp
            var refundOrder = TestData.ConstructRefundOrder();
            var account = new Account() { AccountID = 1, Name = "test", Money = refundOrder.Money + 100, State = AccountState.Freeze };

            accountRepository.GetAccountById(refundOrder.AccountId).Returns(account);


            //Action
            var orderService = new RefundOrderService(accountRepository, refundOrderRepository, paymentGatewayClient);
            var result = orderService.RefundMoney(refundOrder);

            var expectResult = new OrderResponse() { Message = "用户账户处于冻结状态无法退款，请查实", OrderId = refundOrder.OrderId, Status = false };
            //Assert
            result.Should().BeEquivalentTo(expectResult);
        }


        /// <summary>
        /// 当余额不足时
        /// </summary>
        [Fact]
        public void RefundMoney_ShouldReturnSuccess_WhenAccountMoneyLessThanRefundMoney()
        {
            //setUp
            var refundOrder = TestData.ConstructRefundOrder();
            refundOrder.Money = 400;
            var account = new Account() { AccountID = 1, Name = "test", Money = 350, State = AccountState.Normal };

            accountRepository.GetAccountById(refundOrder.AccountId).Returns(account);


            //Action
            var orderService = new RefundOrderService(accountRepository, refundOrderRepository, paymentGatewayClient);
            var result = orderService.RefundMoney(refundOrder);

            var expectResult = new OrderResponse() { Message = "用户余额不足无法退款，请查实", OrderId = refundOrder.OrderId, Status = false };
            //Assert
            result.Should().BeEquivalentTo(expectResult);
        }

        /// <summary>
        /// 当退款的支付账户有问题时
        /// </summary>
        [Fact]
        public void RefundMoney_ShouldReturnSuccess_WhenRefundPaymentGatewayFailed()
        {
            //setUp
            var refundOrder = TestData.ConstructRefundOrder();
            refundOrder.PaymentAccount = "123";

            var account = new Account() { AccountID = 1, Name = "test", Money = refundOrder.Money+100, State = AccountState.Normal };

            accountRepository.GetAccountById(refundOrder.AccountId).Returns(account);
            paymentGatewayClient.RefundMoneyOrder(refundOrder).Returns(new CommonPaymenResponse() { Status = false,Message="账户信息不正确" });

            //Action
            var orderService = new RefundOrderService(accountRepository, refundOrderRepository, paymentGatewayClient);
            var result = orderService.RefundMoney(refundOrder);

            var expectResult = new OrderResponse() { Message = "账户信息不正确,退款失败", OrderId = refundOrder.OrderId, Status = false };

            //Assert
            result.Should().BeEquivalentTo(expectResult);
        }


    }
}

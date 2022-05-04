using Microsoft.AspNetCore.Mvc;
using RentSystem.DI;
using RentSystem.Model;
using RentSystem.PaymentClient;
using RentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Service
{
    public interface IOrderService : IDependency
    {
        public OrderResponse RefundMoney(RefundOrder refundOrder);
    }

    public class RefundOrderService
    {
        private IAccountRepository _accountRepository;
        private IRefundOrderRepository _refundOrderRepository;
        private IPaymentGatewayClient _paymentGatewayClient;

        public RefundOrderService(IAccountRepository accountRepository,IRefundOrderRepository refundOrderRepository, IPaymentGatewayClient paymentGatewayClient)
        {
            _accountRepository = accountRepository;
            _refundOrderRepository = refundOrderRepository;
            _paymentGatewayClient = paymentGatewayClient;
        }

        public OrderResponse RefundMoney(RefundOrder refundOrder)
        {
            var result = new OrderResponse() { OrderId = refundOrder.OrderId,Status = false};

            var account = _accountRepository.GetAccountById(refundOrder.AccountId);
            Tuple<string,bool> dbResult = CheckAccountAndOrder(refundOrder,account);
            if (!dbResult.Item2)
            {
                result.Message = dbResult.Item1;
                return result;
            }
    
            var refundResponse = _paymentGatewayClient.RefundMoneyOrder(refundOrder);
            if (!refundResponse.Status)
            {
                result.Message = refundResponse.Message+",退款失败";
                return result;
            }

            if (refundResponse.Status && PostAccountAndRefundOrder(refundOrder, account))
            {
                result.Message = "申请退款成功";
                result.Status = true;
                return result;
            }
            result.Message = "申请退款失败，请重试";
            return result;
           
        }
        /// <summary>
        /// 检测账户信息
        /// </summary>
        /// <param name="refundOrder"></param>
        /// <returns></returns>
        private Tuple<string, bool> CheckAccountAndOrder(RefundOrder refundOrder,Account account)
        {
            Tuple<string, bool> result = new Tuple<string, bool>(string.Empty, true);
            var refundOrderExisting = _refundOrderRepository.GetOrderByOrderId(refundOrder.OrderId);

            if (account == null )
            {
                result = new Tuple<string, bool>("用户信息不正确，请查实", false);
            }
            if(refundOrderExisting != null)
            {
                result = new Tuple<string, bool>("订单已发送，请不要重复发送申请", false);
            }
            if (account.State == Model.EnumType.AccountState.Freeze)
            {
                result = new Tuple<string, bool>("用户账户处于冻结状态无法退款，请查实", false);
            }
            if (refundOrder.Money > account.Money)
            {
                result = new Tuple<string, bool>("用户余额不足无法退款，请查实", false);
            }
            return result;
        }

        private bool PostAccountAndRefundOrder(RefundOrder refundOrder,Account account)
        {
            refundOrder.State = RefundOrderState.HasSentRequest;
            account.Money -= refundOrder.Money;
            return _refundOrderRepository.PostRefundOrderAndAccount(refundOrder, account);
        }
    }
}

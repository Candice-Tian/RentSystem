using Newtonsoft.Json;
using RentSystem.DI;
using RentSystem.Model;
using RentSystem.MQClient;
using RentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Service
{
    public interface IInvoiceService:IDependency
    {
        public Tuple<string, bool> PostInvoice(string orderId);
    }
    public class InvoiceService: IInvoiceService
    {
        public IPaymentOrderRepository paymentOrderRepository;
        public IMQClient mqClient;
        public InvoiceService(IPaymentOrderRepository repository,IMQClient client)
        {
            paymentOrderRepository = repository;
            mqClient = client;
        }

        /// <summary>
        /// 申请开发票
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Tuple<string,bool> PostInvoice(string orderId)
        {
            Tuple<PaymentOrder, string> orderState = CheckOrderState(orderId);
            PaymentOrder order = orderState.Item1;
            if (order == null)
            {
                return new Tuple<string, bool>(orderState.Item2, false);
            }
            mqClient.Publish(JsonConvert.SerializeObject(order));
            order.InvoiceApply = true;
            paymentOrderRepository.PostPaymentOrderById(order);

            return new Tuple<string, bool>("发票申请成功",true);
        }

        private Tuple<PaymentOrder, string> CheckOrderState(string orderId)
        {
           var order = paymentOrderRepository.GetPaymentOrderByOrderId(orderId);
            if (order == null)
            {
                return new Tuple<PaymentOrder, string>(null, "订单不存在");
            }
            if (order.InvoiceApply)
            {
                return new Tuple<PaymentOrder, string>(null, "订单发票已开");
            }
            return new Tuple<PaymentOrder, string>(order, "");
           
        }
    }
}

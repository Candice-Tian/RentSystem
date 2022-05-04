using RentSystem.DI;
using RentSystem.Model;

namespace RentSystem.Repository
{
    public interface IPaymentOrderRepository : IDependency
    {
        public PaymentOrder GetPaymentOrderByOrderId(string orderId);
        public bool PostPaymentOrderById(PaymentOrder order);
    }
    public class PaymentOrderRepository: IPaymentOrderRepository
    {
        public PaymentOrder GetPaymentOrderByOrderId(string orderId)
        {
            string sql = "Select * from PaymentOrder Where OrderId='"+orderId+"'";
            var result = DBContext.QueryFirstOrDefault<PaymentOrder>(sql);
            return result;
        }


        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool PostPaymentOrderById(PaymentOrder order)
        {
            string sql = "Update PaymentOrder Set InvoiceApply=@InvoiceApply Where OrderId=@OrderId";
            var count = DBContext.Execute(sql, order);
            return count > 0 ? true : false;
        }
    }
}

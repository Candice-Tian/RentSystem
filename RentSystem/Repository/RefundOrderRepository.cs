using RentSystem.DI;
using RentSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Repository
{
    public interface IRefundOrderRepository:IDependency
    {
        public RefundOrder GetOrderByOrderId(string orderId);
        public bool PostRefundOrder(RefundOrder refundOrder);
        public bool PostRefundOrderAndAccount(RefundOrder refundOrder, Account account);
    }
    public class RefundOrderRepository: IRefundOrderRepository
    {

        public RefundOrder GetOrderByOrderId(string orderId)
        {
            string sql = "select * from RefoundOrder where OrderId='"+orderId+"'";
            RefundOrder result = DBContext.QueryFirstOrDefault<RefundOrder>(sql, orderId);
            return result;
        }


        /// <summary>
        /// 新增和修改RefundOrder
        /// </summary>
        /// <param name="refundOrder"></param>
        /// <returns></returns>
        public bool PostRefundOrder(RefundOrder refundOrder)
        {
            string sql = string.Empty;
            int count = 0; 
            if (GetOrderByOrderId(refundOrder.OrderId) != null)
            {
                sql = "Update RefoundOrder Set Money=@Money,PayType=@PayType,State=@State,PaymentAccount=@PaymentAccount Where OrderId=@OrderId";
                count = DBContext.Execute(sql, refundOrder);
            }
            else
            {
                sql = "Insert into RefoundOrder Values(@AccountId,@Money,@PayType,@State,@PaymentAccount,@OrderId)";
                count = DBContext.Execute(sql, refundOrder);
            }
          
            return count > 0 ? true : false;
        }

        /// <summary>
        /// 插入一个RefundOrder, 修改Account
        /// </summary>
        /// <param name="refundOrder"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool PostRefundOrderAndAccount(RefundOrder refundOrder,Account account)
        {
            Dictionary<string, object> sqlDic = new Dictionary<string, object>();
            string insertRefundOrderSql = "Insert into RefoundOrder Values(@AccountId,@Money,@PayType,@State,@PaymentAccount,@OrderId)";
            string updateAccountSql = "update Account set Name=@Name,AccountId=@AccountId,Money=@Money Where AccountId=@AccountId";
            sqlDic.Add(insertRefundOrderSql, refundOrder);
            sqlDic.Add(updateAccountSql, account);

            int count = DBContext.ExecuteTransition(sqlDic);
            return count == 2 ? true : false;
        }
    }
}

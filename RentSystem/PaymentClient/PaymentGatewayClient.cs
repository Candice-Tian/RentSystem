using Newtonsoft.Json;
using RentSystem.DI;
using RentSystem.Helper;
using RentSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentSystem.PaymentClient
{
    public interface IPaymentGatewayClient:IDependency
    {
        public CommonPaymenResponse RefundMoneyOrder(RefundOrder order);
    }
    public class PaymentGatewayClient:IPaymentGatewayClient
    {
        private static string paymentGateway = AppConfigurtaionServices.Configuration.GetSection("Connections").GetSection("paymentGateway").Value;
       
        public HttpClient Client;

        public PaymentGatewayClient(HttpClient client)
        {
            Client = client;
        }

        /// <summary>
        /// 处理请求和返回
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public CommonPaymenResponse RefundMoneyOrder(RefundOrder order)
        {
            string content = ConvertRequestBody(order);
            var result = PostMessageAsync(content).Result;
            return result;
        }

        /// <summary>
        /// 发请求
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<CommonPaymenResponse> PostMessageAsync(string content)
        {
            var result = new CommonPaymenResponse();
            try
            {
                HttpContent httpContent = new StringContent(content);
                var response = await Client.PostAsync(paymentGateway, httpContent);
                if (response.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
                {
                    return new CommonPaymenResponse() { Status = false, Message = response.StatusCode.ToString() };
                }
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync();
                 result = JsonConvert.DeserializeObject<CommonPaymenResponse>(responseString.Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.Message = ex.ToString();
                result.Status = false;
            }
            return result;

        }



        /// <summary>
        /// 根据PayType转化为不同的支付格式
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string ConvertRequestBody(RefundOrder order)
        {
            var paymentAccount = new CommonPaymenOrder() { PaymentAccount = order.PaymentAccount, Money = order.Money };
            return JsonConvert.SerializeObject(paymentAccount);
        }
    }
}

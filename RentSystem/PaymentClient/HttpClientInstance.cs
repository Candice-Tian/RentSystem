using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentSystem.PaymentClient
{
    public class HttpClientInstance
    {
        private object locker = new object();
        private HttpClient Client;

        public HttpClient PaymentClientInstance()
        {
            if (Client == null)
            {
                lock (locker)
                {
                    if (Client == null)
                    {
                        Client = new HttpClient();
                    }
                }
            }
            return Client;
        }
    }
}

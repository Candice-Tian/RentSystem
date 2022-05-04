using EasyNetQ;
using RabbitMQ.Client;
using RentSystem.DI;
using RentSystem.Helper;
using RentSystem.Model;
using System;
using System.Text;

namespace RentSystem.MQClient
{
    public interface IMQClient : IDependency
    {
        public void Publish(string msg, ConnectionFactory factory=null);
    }

    public class MQOrderClient:IMQClient
    {
        private static string MQConnection = AppConfigurtaionServices.Configuration.GetSection("Connections").GetSection("MQConnection").Value;
        private static string QueueName = AppConfigurtaionServices.Configuration.GetSection("Connections").GetSection("QueueName").Value;

        public void Publish(string msg , ConnectionFactory factory=null)
        {
            if (factory == null)
            {
                factory = new ConnectionFactory();
            }

            factory.Uri = new Uri(MQConnection);
            using (var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    var properties = channel.CreateBasicProperties();
                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish("", QueueName, properties,buffer);
                }
            }
        }
    }
}

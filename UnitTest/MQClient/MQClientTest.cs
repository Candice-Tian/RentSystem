using NSubstitute;
using RabbitMQ.Client;
using RentSystem.MQClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.MQClient
{
    public class MQClientTest
    {
        public void Publish_ShouldPublishService()
        {
            var factory = Substitute.For<ConnectionFactory>();
            var connection = Substitute.For<IConnection>();
            factory.CreateConnection().Returns(connection);
            string message = "TestPublish";

            var MQClient = new MQOrderClient();
            MQClient.Publish(message,factory);


        }

    }
}

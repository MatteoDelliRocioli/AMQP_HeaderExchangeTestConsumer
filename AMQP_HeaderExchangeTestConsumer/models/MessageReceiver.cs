using AMQP_HeaderExchangeTestPublisher;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQP_HeaderExchangeTestConsumer.models
{
    public class MessageReceiver
    {
        public void ListenForMessages()
        {
            var connectionfactory = new MyConnectionFactory();

            //create a connection
            using (var connection = connectionfactory.CreateConnection())
            {
                //create a channel
                using (var channel = connection.CreateModel())
                {
                    //Declare/Create a queue
                    Dictionary<string, object> queueArgs = new Dictionary<string, object>();
                    queueArgs.Add("category", "animal");
                    queueArgs.Add("type", "mammal");

                    channel.QueueDeclare(queue: "queueTestForHeader",
                                            durable: true,
                                            autoDelete: false,
                                            exclusive: false,
                                            arguments: queueArgs);

                    Subscription subscription = new Subscription(model: channel, queueName: "queueTestForHeader", autoAck: false);

                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();      //queue consumption
                        StringBuilder messageBuilder = new StringBuilder();
                        string message = Encoding.UTF8.GetString(deliveryArguments.Body);
                        messageBuilder.Append("Message from queue: ").Append(message).Append(". ");
                        foreach (string headerKey in deliveryArguments.BasicProperties.Headers.Keys)
                        {
                            byte[] value = deliveryArguments.BasicProperties.Headers[headerKey] as byte[];
                            messageBuilder.Append("Header key: ").Append(headerKey).Append(", value: ").Append(Encoding.UTF8.GetString(value)).Append("; ");
                        }

                        Console.WriteLine(messageBuilder.ToString());
                        subscription.Ack(deliveryArguments);
                    }
                }
            }
        }
    }
}

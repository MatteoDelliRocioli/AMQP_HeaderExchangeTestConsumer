using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace AMQP_HeaderExchangeTestConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
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
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();
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

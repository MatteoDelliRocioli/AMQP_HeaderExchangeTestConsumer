using System;
using System.Collections.Generic;
using System.Text;
using AMQP_HeaderExchangeTestConsumer.models;
using AMQP_HeaderExchangeTestPublisher;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace AMQP_HeaderExchangeTestConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please for the first message to arrive");

            MessageReceiver messageReceiver = new MessageReceiver();
            messageReceiver.ListenForMessages();

            Console.ReadLine();
        }
    }
}

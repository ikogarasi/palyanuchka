﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Restaurant.Service.Email.Messages;
using Restaurant.Service.Email.Repository;
using System.Text;

namespace Restaurant.Service.Email.Messaging
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly EmailRepository _emailRepository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPaymentUpdate_Exchange";
        private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";

        private string queueName = "";

        public RabbitMQPaymentConsumer(EmailRepository emailRepository)
        {
            _emailRepository = emailRepository;

            var factory = new ConnectionFactory
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);

            _channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(content);
                HandleMessage(updatePaymentResultMessage).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(UpdatePaymentResultMessage message)
        {
            try
            {
                await _emailRepository.SendAndLogEmail(message);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

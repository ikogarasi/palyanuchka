using Newtonsoft.Json;
using PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Restaurant.MessageBus;
using Restaurant.Service.PaymentAPI.Messages;
using Restaurant.Services.PaymentAPI.RabbitMQSender;
using System.Text;

namespace Restaurant.Service.PaymentAPI.Messaging
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IRabbitMQPaymentMessageSender _rabbitMQPaymentMessageSender;
        private readonly IProcessPayment _processPayment;
        private IConnection _connection;
        private IModel _channel;

        private const string PAYMENT_QUEUE_NAME = "orderpaymentprocesstopic";

        public RabbitMQPaymentConsumer(IRabbitMQPaymentMessageSender rabbitMQPaymentMessageSender, IProcessPayment processPayment)
        {
            _rabbitMQPaymentMessageSender = rabbitMQPaymentMessageSender;
            _processPayment = processPayment;

            var factory = new ConnectionFactory
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: PAYMENT_QUEUE_NAME, false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(content);
                HandleMessage(paymentRequestMessage).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(PAYMENT_QUEUE_NAME, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(PaymentRequestMessage paymentRequestMessage)
        {
            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            try
            {
                _rabbitMQPaymentMessageSender.SendMessage(updatePaymentResultMessage);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

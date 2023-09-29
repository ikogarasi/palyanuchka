using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Restaurant.Service.OrderAPI.Messages;
using Restaurant.Service.OrderAPI.Repository;
using System.Text;

namespace Restaurant.Service.OrderAPI.Messaging
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPaymentUpdate_Exchange";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        private string queueName = "";

        public RabbitMQPaymentConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            var factory = new ConnectionFactory
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);

            _channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");
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
                await _orderRepository.UpdateOrderPaymentStatus(message.OrderId, message.Status);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using PaymentProcessor;
using Restaurant.MessageBus;
using Restaurant.Service.PaymentAPI.Messages;
using System.Text;

namespace Restaurant.Service.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _paymentMessageTopic;
        private readonly string _subscriptionPayment;
        private readonly string _orderPaymentProcessTopic;
        private readonly string _orderUpdatePaymentResultTopic;

        private readonly ServiceBusProcessor _orderPaymentProcessor;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _processPayment;

        public AzureServiceBusConsumer(IProcessPayment processPayment, IConfiguration configuration, IMessageBus messageBus)
        {
            _serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            _subscriptionPayment = configuration.GetValue<string>("SubscriptionPaymentName");
            _paymentMessageTopic = configuration.GetValue<string>("PaymentMessageTopic");
            _orderUpdatePaymentResultTopic = configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _messageBus = messageBus;

            _orderPaymentProcessor = client.CreateProcessor(_orderPaymentProcessTopic, _subscriptionPayment);
            _processPayment = processPayment;
        }

        public async Task Start()
        {
            _orderPaymentProcessor.ProcessMessageAsync += ProcessPayment;
            _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderPaymentProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _orderPaymentProcessor.StopProcessingAsync();
            await _orderPaymentProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPayment(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            try
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, _orderUpdatePaymentResultTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}

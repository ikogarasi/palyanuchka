using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using PaymentProcessor;
using Restaurant.MessageBus;
using Restaurant.Service.Email.Messages;
using Restaurant.Service.Email.Repository;
using Restaurant.Service.Email.Repository.IRepository;
using System.Text;

namespace Restaurant.Service.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _subscriptionEmail;
        private readonly string _orderUpdatePaymentResultTopic;

        private readonly EmailRepository _emailRepository;
        private readonly ServiceBusProcessor _orderPaymentProcessor;
        private readonly IMessageBus _messageBus;

        public AzureServiceBusConsumer(EmailRepository _emailRepository, IConfiguration configuration, IMessageBus messageBus)
        {
            _serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            _subscriptionEmail = configuration.GetValue<string>("SubscriptionName");
            _orderUpdatePaymentResultTopic = configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _messageBus = messageBus;

            _orderPaymentProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _subscriptionEmail);
        }

        public async Task Start()
        {
            _orderPaymentProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
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

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            try
            {
                await _emailRepository.SendAndLogEmail(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}

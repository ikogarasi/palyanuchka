using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Restaurant.MessageBus;
using Restaurant.Service.OrderAPI.Messages;
using Restaurant.Service.OrderAPI.Models;
using Restaurant.Service.OrderAPI.Repository;
using Restaurant.Services.OrderAPI.Messages;
using System.Text;

namespace Restaurant.Service.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _checkoutMessageTopic;
        private readonly string _checkoutSubscriptionName;
        private readonly string _paymentMessageTopic;
        private readonly string _orderUpdatePaymentResultTopic;

        private readonly OrderRepository _orderRepository;

        private readonly ServiceBusProcessor _checkOutProcessor;
        private readonly ServiceBusProcessor _orderUpdatePaymentStatusProcessor;
        private readonly IMessageBus _messageBus;

        public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            _checkoutMessageTopic = configuration.GetValue<string>("CheckoutMessageTopic");
            _checkoutSubscriptionName = configuration.GetValue<string>("SubscriptionCheckoutName");
            _paymentMessageTopic = configuration.GetValue<string>("PaymentMessageTopic");
            _orderUpdatePaymentResultTopic = configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(_serviceBusConnectionString);
            _messageBus = messageBus;

            _checkOutProcessor = client.CreateProcessor(_checkoutMessageTopic, _checkoutSubscriptionName);
            _orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _checkoutSubscriptionName);
        }

        public async Task Start()
        {
            _checkOutProcessor.ProcessMessageAsync += OnCheckoutMessageReceive;
            _checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await _checkOutProcessor.StartProcessingAsync();

            _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceive;
            _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _checkOutProcessor.StopProcessingAsync();
            await _checkOutProcessor.DisposeAsync();

            await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await _orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnOrderPaymentUpdateReceive(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);

            await args.CompleteMessageAsync(args.Message);
        }

        private async Task OnCheckoutMessageReceive(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDate = checkoutHeaderDto.PickupDate
            };

            foreach (var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.ProductName,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }
            
            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email,
            };

            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, _paymentMessageTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}

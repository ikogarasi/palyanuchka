using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Restaurant.MessageBus
{
    public class AzureServiceMessageBus : IMessageBus
    {
        private readonly string connectionString = "Endpoint=sb://palyanuchkarestaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NVnb6YuQyPosd6vQVeN23/OSDidrJxTCGYwBIQEkPeU=";

        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicName);
            string jsonMessage = JsonConvert.SerializeObject(message);
            var msg = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(msg);

            await client.DisposeAsync();
        }
    }
}

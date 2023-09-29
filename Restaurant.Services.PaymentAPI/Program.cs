using PaymentProcessor;
using Restaurant.MessageBus;
using Restaurant.Service.PaymentAPI.Extensions;
using Restaurant.Service.PaymentAPI.Messaging;
using Restaurant.Services.PaymentAPI.RabbitMQSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProcessPayment, ProcessPayment>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus, AzureServiceMessageBus>();
builder.Services.AddSingleton<IRabbitMQPaymentMessageSender, RabbitMQPaymentMessageSender>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAzureServiceBusConsumer();

app.Run();

using Microsoft.EntityFrameworkCore;
using Restaurant.MessageBus;
using Restaurant.Service.Email.DbContexts;
using Restaurant.Service.Email.Extensions;
using Restaurant.Service.Email.Messaging;
using Restaurant.Service.Email.Repository;
using Restaurant.Service.Email.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(_
    => _.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSingleton(new EmailRepository(optionBuilder.Options));

builder.Services.AddSingleton<IMessageBus, AzureServiceMessageBus>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
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

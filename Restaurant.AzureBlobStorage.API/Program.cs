using Azure.Storage.Blobs;
using Restaurant.Services.AzureBlobStorageAPI.Repository;
using Restaurant.Services.AzureBlobStorageAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(_ =>
{
    return new BlobContainerClient(builder.Configuration.GetConnectionString("AzureStorage"), 
        builder.Configuration.GetValue<string>("BlobContainerName"));
});
builder.Services.AddTransient<IAzureStorageRepository, AzureStorageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

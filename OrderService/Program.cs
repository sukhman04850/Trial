using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderService;
using OrderService.Interfaces;
using OrderService.KafkaConsumer;
using OrderService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<OrderDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnectionString")));
builder.Services.AddScoped<IOrderInterface, OrderRepository>();
builder.Services.AddSingleton<KafkaConsumers>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var cancellationToken = new CancellationToken();
var kafkaConsumers = app.Services.GetRequiredService<KafkaConsumers>(); 
kafkaConsumers.ExecuteAsync(cancellationToken);


app.Run();



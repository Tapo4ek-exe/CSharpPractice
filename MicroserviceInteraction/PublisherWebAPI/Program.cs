using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapPost("/api/send", ([FromBody] string message) =>
{
    try
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "api-queue",
                                 exclusive: false,
                                 autoDelete: false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "api-queue",
                                 body: body);

            app.Logger.LogInformation("Message sent to consumer");
            return message;
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex.Message);
        return "Unexpected error";
    }
})
.WithName("SendMessageToConsumer");

app.Run();

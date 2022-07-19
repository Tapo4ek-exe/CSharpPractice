using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapGet("/api/receive", () =>
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

            var result = channel.BasicGet("api-queue", true);
            if (result != null)
            {
                var body = result.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                app.Logger.LogInformation($"Message received from publisher: {message}");
                return message;
            }
            else
            {
                app.Logger.LogInformation("Queue is empty");
                return "Queue is empty";
            }
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex.Message);
        return "Unexpected error";
    }
})
.WithName("GetMessageFromPublisher");

app.Run();

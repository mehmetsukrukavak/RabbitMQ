// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMq.DirextPublisher;


var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
var exchangeName = "logs-direct";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, exchangeName, routeKey,null);
});

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);
    string message = $"Log : {x}, log-type : {log},  {DateTime.Now}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";
    channel.BasicPublish(exchangeName, routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderilmiştir: {message}");
});

Console.ReadLine();
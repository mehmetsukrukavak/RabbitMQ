// See https://aka.ms/new-console-template for more information


using System.Text;
using RabbitMQ.Client;
using RabbitMq.TopicPublisher;


var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
var exchangeName = "logs-topic";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Topic);
Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"Log : {x}, log-type : {log1}-{log2}-{log3},  {DateTime.Now}";

    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchangeName, routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderilmiştir: {message}");
});

Console.ReadLine();
// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-fanout",durable:true, type:ExchangeType.Fanout);

Enumerable.Range(1,50).ToList().ForEach(x =>
{
    string message = $"Message : {x}, {DateTime.Now}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout","", null,messageBody);

    Console.WriteLine($"Messaj Gönderilmiştir: {message}");
});

Console.ReadLine();
// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

//var factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://smdhxzvz:9uTPKN_LZgM1e83qKgFE_w5gB9VMh0GQ@chimpanzee.rmq.cloudamqp.com/smdhxzvz");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue", true, false, false );
Enumerable.Range(1,500).ToList().ForEach(x =>
{
    string message = $"Message : {x}, {DateTime.Now}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty,"hello-queue", null,messageBody);

    Console.WriteLine($"Messaj Gönderilmiştir: {message}");
});

Console.ReadLine();

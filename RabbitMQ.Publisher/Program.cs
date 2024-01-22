// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Shared;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

//var factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://smdhxzvz:9uTPKN_LZgM1e83qKgFE_w5gB9VMh0GQ@chimpanzee.rmq.cloudamqp.com/smdhxzvz");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue", true, false, false);
Enumerable.Range(1, 10).ToList().ForEach(x =>
{
    // string message = $"Message : {x}, {DateTime.Now}";
    //
    // var messageBody = Encoding.UTF8.GetBytes(message);
   
    Product product = new Product { Id = x, Name = $"Procuct {x}", Price = 18.50M, Stock = 50 };
    var productJsonString = JsonSerializer.Serialize(product);
    var messageBody = Encoding.UTF8.GetBytes(productJsonString);
    
    channel.BasicPublish(string.Empty,"hello-queue", null,messageBody);
    Console.WriteLine($"Messaj Gönderilmiştir: {product.Name}");
});

Console.ReadLine();
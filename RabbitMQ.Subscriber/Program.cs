using System.Net.Http.Headers;
using System.Runtime.Loader;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Shared;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };


//var factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://smdhxzvz:9uTPKN_LZgM1e83qKgFE_w5gB9VMh0GQ@chimpanzee.rmq.cloudamqp.com/smdhxzvz");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false);
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("hello-queue", true,consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    //Thread.Sleep(1500);
    Product product = JsonSerializer.Deserialize<Product>(message);
    Console.WriteLine($"Gelen Ürün : {product.Id} - {product.Name} - ${product.Price} - {product.Stock} piece(s)");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
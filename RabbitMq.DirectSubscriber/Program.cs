using System.Runtime.Loader;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();


channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
var queueName = "direct-queue-Error";
channel.BasicConsume(queueName, true,consumer);

Console.WriteLine("Loglar Dinleniyor...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine($"Gelen Log : {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
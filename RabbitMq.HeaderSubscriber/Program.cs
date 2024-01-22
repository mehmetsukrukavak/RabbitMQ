using System.Runtime.Loader;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
var exchangeName = "header-exchange";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Headers);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format","pdf");
headers.Add("shape","a4");
headers.Add("x-match","all");
// headers.Add("shapes","a4");
// headers.Add("x-match","any");

channel.QueueBind(queueName, exchangeName,String.Empty, headers);
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
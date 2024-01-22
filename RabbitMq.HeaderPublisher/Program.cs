// See https://aka.ms/new-console-template for more information


using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
var exchangeName = "header-exchange";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format","pdf");
headers.Add("shape","a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish(exchangeName, string.Empty,properties,Encoding.UTF8.GetBytes("Header Mesajım"));

Console.WriteLine("Mesaj Gönderilmiştir.");
Console.ReadLine();
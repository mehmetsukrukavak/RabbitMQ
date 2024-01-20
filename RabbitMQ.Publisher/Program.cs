// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://smdhxzvz:9uTPKN_LZgM1e83qKgFE_w5gB9VMh0GQ@chimpanzee.rmq.cloudamqp.com/smdhxzvz");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue", true, false, false );

string message = $"hello world {DateTime.Now}";

var messageBody = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(string.Empty,"hello-queue", null,messageBody);

Console.WriteLine("Messaj Gönderilmiştir");
Console.ReadLine();

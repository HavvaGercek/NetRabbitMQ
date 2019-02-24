using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NetRabbitMQ.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //RabbitMQ bağlantısı oluşturduk
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //mail kuyruğuna erişiyoruz
                channel.QueueDeclare(queue: "mail", durable: false, exclusive: false, autoDelete: false, arguments: null);

                //Kuyruktan mail ile ilgili var olan verileri alıyoruz
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    EmailTemplate email = JsonConvert.DeserializeObject<EmailTemplate>(message);
                    Console.WriteLine($"Başlık: {email.Title}, Mail: {email.Email}, Mesaj: {email.Message}");
                };
                channel.BasicConsume(queue: "mail", autoAck: true, consumer: consumer);

                Console.WriteLine(" Mesaj başarılı ile kuyrukdan alındı.");
                Console.ReadLine();
            }
        }
    }

    public class EmailTemplate
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

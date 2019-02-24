using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace NetRabbitMQ.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var email = new EmailTemplate {
                 Title = "RabbitMQ",
                 Message = "RabbitMQ Deneme",
                 Email = "rabbitmq@deneme.com"
            };

            var factory = new ConnectionFactory() { HostName = "localhost" }; //RabbitMQ bağlantımızı oluşturuyoruz
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "mail", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = JsonConvert.SerializeObject(email); //Modeli json string formatına dönüştürüyoruz

                var body = Encoding.UTF8.GetBytes(message); //gönderilecek değeri byte'a çeviriyoruz

                //Mesajı RabbitMQ'ya ekliyoruz
                channel.BasicPublish(exchange: "", routingKey: "mail", basicProperties: null, body: body);

                Console.WriteLine("Gönderilen mail içeriği:", message);
            }

            Console.WriteLine(" Mailiniz başarı ile kuyruğa alındı.");
            Console.ReadLine();

        }
    }
}

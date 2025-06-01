using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


Console.WriteLine("🔎 Aguardando mensagens da fila 'fila_ocorrencias'...");

// Cria conexão
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declara fila (precaução)
channel.QueueDeclare(queue: "fila_ocorrencias",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

// Cria consumer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"📥 Nova ocorrência recebida:\n{message}");
    Console.ResetColor();
};

// Começa consumir
channel.BasicConsume(queue: "fila_ocorrencias", autoAck: true, consumer: consumer);

Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();

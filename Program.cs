using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class RabbitMQConsumer
{
    public static void ConsumeMessage()
    {
        // Create a connection factory
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",  // Replace with your RabbitMQ server's address if different
            UserName = "guest",     // Replace with your RabbitMQ username
            Password = "guest"      // Replace with your RabbitMQ password
        };

        try
        {
            // Establish a connection to RabbitMQ
            using (var connection = factory.CreateConnection())
            {
                // Create a channel for communication
                using (var channel = connection.CreateModel())
                {
                    var queueName = "hello_queue";  // Queue name to consume from

                    // Declare the queue (make sure it exists)
                    channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    // Create a consumer to listen for messages
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Received message: {message}");
                    };

                    // Start consuming from the queue
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine("Waiting for messages...");
                    Console.ReadLine();  // Keep the consumer alive
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void Main(string[] args)
    {
        // Call the ConsumeMessage method to start consuming messages
        ConsumeMessage();
    }
}

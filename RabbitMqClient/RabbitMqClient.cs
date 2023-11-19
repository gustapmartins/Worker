using RabbitMQ.Client.Events;
using Worker.Interface;
using RabbitMQ.Client;
using System.Text;
using Worker.DTO;
using Worker.Ultis;
using Worker.Model;

namespace Worker.RabbitMqClient;

public class RabbitMqClient: RabbitMqBase
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly IConvert _convert;

    public RabbitMqClient()
    {
        _connection = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672
        }.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: Commons.Constants.RABBITMQ_TICKETS,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        _convert = new Ultis.Convert();
    }

    public void ExecuteAsync()
    {

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                var result = _convert.DeserializeMessage<Ticket>(message);

                Console.WriteLine($"{result.Show.Name} | {result.Price} | {result.QuantityTickets}");

                _channel.BasicAck(ea.DeliveryTag, false);
                
            }
            catch
            {
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: Commons.Constants.RABBITMQ_TICKETS,
                            autoAck: false,
                            consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public void StopListening()
    {
        _channel.Close();
        _connection.Close();
        
    }
}

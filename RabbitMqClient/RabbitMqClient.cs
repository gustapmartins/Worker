using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using Worker.Interface;
using RabbitMQ.Client;
using Worker.Model;
using Worker.Data;
using Worker.Enum;
using System.Text;

namespace Worker.RabbitMqClient;

public class RabbitMqClient: RabbitMqBase
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly IConvert _convert;
    private readonly TicketContext _dbContext;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public RabbitMqClient(TicketContext dbContext)
    {
        _connection = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672
        }.CreateConnection();
        _channel = _connection.CreateModel();
        _dbContext = dbContext;
        _channel.QueueDeclare(queue: Commons.Constants.RABBITMQ_TICKETS,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        _convert = new Ultis.Convert();

        _cancellationTokenSource = new CancellationTokenSource();
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

                var result = _convert.DeserializeMessage<Carts>(message);

                SaveToDatabase(result);
               
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

        Task.Run(() => MonitorarStatusWorker(), _cancellationTokenSource.Token);

        Console.WriteLine("Pressione [enter] para sair.");
        Console.ReadLine();
    }

    private void SaveToDatabase(Carts cart)
    {
        try
        {

            Carts cartExist = _convert.HandleErrorAsync(() => 
                _dbContext.Carts.Include(c => c.CartList).Include(t => t.Users).FirstOrDefault(s => s.Id == cart.Id))!;

            foreach (var CartItem in cartExist.CartList)
            {
                if (CartItem.statusPayment == StatusPayment.Pedding)
                {
                    CartItem.statusPayment = StatusPayment.Aproved;
                    _dbContext.SaveChanges();
                    Console.WriteLine("Dados salvos no banco de dados com sucesso!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar no banco de dados: {ex.Message}");
        }
    }

    private async Task MonitorarStatusWorker()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            // Exibir informações de status no console
            var date = DateTime.Now;

            Console.WriteLine($"Worker está ativo, {date}");

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public void StopListening()
    {
        _channel.Close();
        _connection.Close();
    }
}
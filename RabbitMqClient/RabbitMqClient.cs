using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using Worker.Interface;
using RabbitMQ.Client;
using System.Text;
using Worker.Model;
using Worker.Data;
using Worker.DTO;

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

                var result = _convert.DeserializeMessage<BuyTicketDto>(message);

                Console.WriteLine($"{result.TicketId}| {result.Email} | {result.QuantityTickets}");

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

    private void SaveToDatabase(BuyTicketDto buyTicket)
    {
        try
        {
            Tickets findTicket = _dbContext.Tickets
            .Include(t => t.Show)
                .ThenInclude(s => s.Category)
            .FirstOrDefault(t => t.Id == buyTicket.TicketId)!;

            Users findUser = _dbContext.Users.Include(t => t.Tickets).FirstOrDefault(t => t.Email == buyTicket.Email)!;

            Tickets ticket = new()
            {
                Price = findTicket.Price,
                QuantityTickets = findTicket.QuantityTickets,
                Show = new()
                {
                    Name = findTicket.Show!.Name,
                    Local = findTicket.Show!.Local,
                    Date = findTicket.Show!.Date,
                    Description = findTicket.Show!.Description,
                    Category = new()
                    {
                        Name = findTicket.Show.Category!.Name,
                        Description = findTicket.Show.Category!.Description,
                    }
                }
            };

            findUser.Tickets.Add(findTicket!);
            _dbContext.SaveChanges();
            Console.WriteLine("Dados salvos no banco de dados com sucesso!");
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

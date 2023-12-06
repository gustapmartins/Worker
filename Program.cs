using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Worker.Data;
using Worker.RabbitMqClient;

class Program
{
    static void Main(string[] args)
    {
        // Configuração do serviço
        var serviceProvider = new ServiceCollection()
            .AddDbContext<TicketContext>(options =>
                options.UseNpgsql("User ID=postgres;Password=2525;Host=localhost;Port=5432;Database=Ticket;Pooling=true;Connection Lifetime=0;")
            )
            .BuildServiceProvider();

        var scope = serviceProvider.CreateScope();

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TicketContext>();

            // Iniciar o RabbitMqClient
            Console.WriteLine("Iniciando o aplicativo...");

            var rabbitMqClient = new RabbitMqClient(dbContext);

            // Inicia a execução assíncrona
            rabbitMqClient.ExecuteAsync();
        }
        catch (Exception ex)
        {
            // Lida com exceções, se necessário
            Console.WriteLine($"Erro: {ex.Message}");
        }
        
    }
}

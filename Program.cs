using Worker.RabbitMqClient;

Console.WriteLine("Iniciando o aplicativo...");

var rabbitMqClient = new RabbitMqClient();

// Inicia a execução assíncrona
rabbitMqClient.ExecuteAsync();

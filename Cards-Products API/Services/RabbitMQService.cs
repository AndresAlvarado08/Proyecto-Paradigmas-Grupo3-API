using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Cards_Products_API.Services;

public class RabbitMQService : IDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private RabbitMQ.Client.IModel? _channel;

    public RabbitMQService()
    {
        _factory = new ConnectionFactory
        {
            HostName = "26.155.73.119",
            UserName = "admin",
            Password = "admin123"
        };
    }

    private void EnsureConnected()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
    }

    public void PublicarCompra(object compra)
    {
        EnsureConnected();

        var mensaje = JsonSerializer.Serialize(compra);
        var body = Encoding.UTF8.GetBytes(mensaje);

        _channel!.BasicPublish(
            exchange: "compras.exchange",
            routingKey: "compra.nueva",
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"📤 Compra publicada: {mensaje}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
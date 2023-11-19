using FreeStuff.Shared.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FreeStuff.Shared.Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
    private readonly IBus              _bus;
    private readonly ILogger<EventBus> _logger;

    public EventBus(IBus bus, ILogger<EventBus> logger)
    {
        _bus    = bus;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            await _bus.Publish(message, cancellationToken);
            _logger.LogInformation("Message of type {MessageType} published successfully.", typeof(T).FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error publishing message of type {MessageType}.",
                typeof(T).FullName
            );
            throw;
        }
    }
}

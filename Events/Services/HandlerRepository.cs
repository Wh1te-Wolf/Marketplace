using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Events.Services;

public class HandlerRepository : IHandlerRepository
{
    private readonly Dictionary<string, Type> _handlers = new Dictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;

    public HandlerRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task HandleAsync(string handler, IMarketplaceEvent marketplaceEvent)
    {
        if(!_handlers.TryGetValue(handler, out Type handlerType))
            return;

        using IServiceScope serviceScope = _serviceProvider.CreateScope();

        IEventHandler? eventHandler = serviceScope.ServiceProvider.GetRequiredService(handlerType) as IEventHandler;

        await eventHandler?.HandleAsync(marketplaceEvent);
    }

    public Task Initialize()
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        IEnumerable<Type>? types = entryAssembly?.GetTypes().Concat(Assembly.GetExecutingAssembly().GetTypes());
        if (types is null)
            return Task.CompletedTask;

        foreach (Type type in types) 
        {
            if (type.CustomAttributes.Any(a => a.AttributeType == typeof(MarketplaceEventHandlerAttribute)))
            {
                _handlers.Add(type.FullName, type);
            }
        }

        return Task.CompletedTask;
    }
}

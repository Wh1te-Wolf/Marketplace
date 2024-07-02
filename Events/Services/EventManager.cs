using Events.Entities;
using Events.Options;
using Events.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Events.Services;

public class EventManager : IEventManager
{
    private const string _exchange = "MarketplaceExchange";
    private readonly IOptions<EventManagerConnectionOptions> _connectionOptions;
    private IConnection _connection;
    private readonly List<IModel> _channels = new List<IModel>();
    private readonly JsonSerializerSettings _jsonSettings = new() { TypeNameHandling = TypeNameHandling.All };

    private List<TopicFilterStack> _topicFilterStacks { get; set; } = new List<TopicFilterStack>();

    private readonly ILogger<EventManager> _logger;
    private readonly IHandlerRepository _handlerRepository; 

    public EventManager(
        IOptions<EventManagerConnectionOptions> connectionOptions,
        ILogger<EventManager> logger,
        IHandlerRepository handlerRepository)
    {
        _logger = logger;
        _handlerRepository = handlerRepository;
        _connectionOptions = connectionOptions;
    }

    public void AddTopicFilterStack(TopicFilterStack topicFilterStack)
    { 
        _topicFilterStacks.Add(topicFilterStack);
    }

    public Task Initialize()
    {
        EventManagerConnectionOptions connectionOptions = _connectionOptions.Value;

        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            UserName = connectionOptions.Username,
            Password = connectionOptions.Password,
            VirtualHost = connectionOptions.Host,
            HostName = connectionOptions.BaseUrl,
            DispatchConsumersAsync = true
        };

        _connection = connectionFactory.CreateConnection();
        CreateExchange();
        CreateQueues();


        return Task.CompletedTask;
    }

    private void CreateExchange()
    { 
        using IModel channel = _connection.CreateModel();
        channel.ExchangeDeclare(_exchange, ExchangeType.Topic, true, false);
    }

    private void CreateQueues()
    {
        foreach (var topicFilterStack in _topicFilterStacks)
        {
            CreateQueue(topicFilterStack.Topic);
        }
    }

    private void CreateQueue(string topic) 
    {
        string routingKey = topic;
        string queueName = GetConsumerQueueName(routingKey);

        IModel channel = _connection.CreateModel();
        channel.QueueDeclare(queueName, true, false, false);
        channel.QueueBind(queueName, _exchange, routingKey, null);
        _channels.Add(channel);

        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (sender, recivedEvent) =>
        {
            byte[] body = recivedEvent.Body.ToArray();
            IMarketplaceEvent marketplaceEvent = Deserialaze(body);
            await ProcessMessage(marketplaceEvent, recivedEvent.RoutingKey);
            channel.BasicAck(recivedEvent.DeliveryTag, false);
        };

        channel.BasicQos(0, 10, true);
        channel.BasicConsume(queueName, false, consumer);
    }

    private async Task ProcessMessage(IMarketplaceEvent marketplaceEvent, string topic)
    {
        TopicFilterStack? subscribers = _topicFilterStacks.FirstOrDefault(t => t.Topic == topic);
        if (subscribers is null)
            return;
        foreach (ActionFilterStack action in subscribers.Actions)
        {
            if (action.EventType != marketplaceEvent.Type || action.EventSubType != marketplaceEvent.SubType)
                continue;
            foreach (HandlerConfiguration handler in action.Handlers)
            {
                await _handlerRepository.HandleAsync(handler.HandlerPath, marketplaceEvent);
            }              
        }             
    }

    private IMarketplaceEvent Deserialaze(byte[] data)
    {
        try
        { 
            string json = Encoding.Unicode.GetString(data);
            IMarketplaceEvent marketplaceEvent = JsonConvert.DeserializeObject<IMarketplaceEvent>(json, _jsonSettings);

            return marketplaceEvent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private string GetConsumerQueueName(string topic)
        => $"{topic}.{AppDomain.CurrentDomain.FriendlyName}";

    public Task ProduceAsync(string topic, IMarketplaceEvent marketplaceEvent)
    {
        byte[] body = Serialaze(marketplaceEvent);

        using IModel channel = _connection.CreateModel();

        string routingKey = topic;

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.ContentType = "application/json";
        properties.Persistent = true;
        properties.DeliveryMode = 2;

        channel.BasicPublish(_exchange, routingKey, properties, body);

        return Task.CompletedTask;
    }

    private byte[] Serialaze(IMarketplaceEvent marketplaceEvent)
    {
        try
        {
            string json = JsonConvert.SerializeObject(marketplaceEvent, _jsonSettings);
            byte[] data = Encoding.Unicode.GetBytes(json);

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

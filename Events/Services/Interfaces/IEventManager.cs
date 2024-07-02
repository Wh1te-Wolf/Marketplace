using Events.Entities;
using Events.Options;

namespace Events.Services.Interfaces;

public interface IEventManager
{
    Task ProduceAsync(string topic, IMarketplaceEvent marketplaceEvent);

    Task Initialize();

    void AddTopicFilterStack(TopicFilterStack topicFilterStack);
}

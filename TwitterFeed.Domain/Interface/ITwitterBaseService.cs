using TwitterFeed.Infra.Models;

namespace TwitterFeed.Infra.Interface;

public interface ITwitterBaseService
{
    Task<IEnumerable<CustomTwitterModel>> GetTimeline();
}
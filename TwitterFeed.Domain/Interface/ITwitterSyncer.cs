namespace TwitterFeed.Infra.Interface;

public interface ITwitterSyncer
{
    Task SyncTwitterTimelineToTelegram();

}
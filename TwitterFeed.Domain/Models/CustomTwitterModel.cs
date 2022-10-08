using Tweetinvi.Models.Entities;

namespace TwitterFeed.Infra.Models;

public class CustomTwitterModel
{
    public TweetType Type { get; set; }
    public string TweetText { get; set; }
    public string? MediaLink { get; set; }


    public enum TweetType
    {
        Text,
        Photo,
        Video
    }
}
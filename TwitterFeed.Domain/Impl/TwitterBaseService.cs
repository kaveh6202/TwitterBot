using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TwitterFeed.Infra.Helper;
using TwitterFeed.Infra.Interface;
using TwitterFeed.Infra.Models;

namespace TwitterFeed.Infra.Impl;

public class TwitterBaseService : ITwitterBaseService
{
    private readonly ITwitterClient _twitterClient;
    private readonly FilterTweetsModel _filterTweets;
    private readonly IRepository _repository;
    private readonly ILogger<TwitterBaseService> _logger;

    public TwitterBaseService(FilterTweetsModel filterTweetModel, IRepository repository, ITwitterClient twitterClient, ILogger<TwitterBaseService> logger)
    {
        _filterTweets = filterTweetModel;
        _repository = repository;
        _twitterClient = twitterClient;
        _logger = logger;
    }
    public async Task<IEnumerable<CustomTwitterModel>> GetTimeline()
    {
        var lastId = _repository.GetLastTimelineId();

        ITweet[] tweets;
        if (lastId <= 0)
        {
            tweets = await _twitterClient.Timelines.GetHomeTimelineAsync();
        }
        else
        {
            tweets = await _twitterClient.Timelines.GetHomeTimelineAsync(new GetHomeTimelineParameters()
            {
                SinceId = lastId
            });
        }
        if (tweets == null || !tweets.Any()) return Enumerable.Empty<CustomTwitterModel>();

        lastId = tweets.Max(i => i.Id);

        var tweetsFiltered = FilterTweets(tweets).ToArray();
        _logger.LogInformation($"{DateTime.Now} Fetched {tweets.Count()} - Kept : {tweetsFiltered.Count()}");

        var tweetDetails = new List<CustomTwitterModel>();
        foreach (var tweet in tweetsFiltered)
        {
            tweetDetails.Add(GetTweetDetail(tweet));
        }

        _repository.UpdateLastTimelineId(lastId);
        return tweetDetails;
    }

    #region pivate methods
    private IEnumerable<ITweet> FilterTweets(IEnumerable<ITweet> tweets)
    {
        //filter out Replies 
        tweets = tweets.Where(i => i.InReplyToUserId == null);

        //include accounts
        if (_filterTweets.ContainsAccounts.Any(i => !string.IsNullOrEmpty(i)))
        {
            tweets = tweets.Where(i => _filterTweets.ContainsAccounts.Contains(i.CreatedBy.ScreenName.ToUpper()) &&
                             (!i.IsRetweet || _filterTweets.ContainsAccounts.Contains(i.RetweetedTweet.CreatedBy.ScreenName.ToUpper()))).ToList();
        }

        //exclude accounts 
        tweets = tweets.Where(i => !_filterTweets.ExcludeAccounts.Contains(i.CreatedBy.ScreenName.ToUpper()));
        tweets = tweets.Where(i =>
            i.IsRetweet == false ||
            !_filterTweets.ExcludeAccounts.Contains(i.RetweetedTweet.CreatedBy.ScreenName.ToUpper()));

        //exclude hastags
        var exTagFilter = new List<ITweet>();
        foreach (var tweet in tweets)
        {
            var hashTags = tweet.Hashtags.Select(i => i.Text);
            var shouldNotAdd = false;
            foreach (var tag in hashTags)
            {
                if (_filterTweets.ExcludeHashTags.Contains(tag))
                {
                    shouldNotAdd = true;
                    break;
                }
            }
            if (!shouldNotAdd)
            {
                exTagFilter.Add(tweet);
            }
        }
        tweets = exTagFilter.ToList();

        //exclude words 
        var wordFilter = new List<ITweet>();
        foreach (var tweet in tweets)
        {
            var words = tweet.Text.Split(' ');
            var shouldNotAdd = false;
            foreach (var word in words)
            {
                if (_filterTweets.ExcludeWords.Contains(word))
                {
                    shouldNotAdd = true;
                    break;
                }
            }
            if (!shouldNotAdd)
            {
                wordFilter.Add(tweet);
            }
        }
        tweets = wordFilter.ToList();

        //include hastags
        var incTagFilter = new List<ITweet>();
        if (_filterTweets.ContainsHashTags.Any(i => !string.IsNullOrEmpty(i)))
        {
            foreach (var tweet in tweets)
            {
                var hashTags = tweet.Hashtags.Select(i => i.Text);
                foreach (var tag in hashTags)
                {
                    if (_filterTweets.ContainsHashTags.Contains(tag))
                    {
                        incTagFilter.Add(tweet);
                        break;
                    }
                }
            }
            tweets = incTagFilter.ToList();
        }
       
        //include language
        //tweets = tweets.Where(i => i.Language == Language.Persian);

        return tweets;
    }
    private CustomTwitterModel GetTweetDetail(ITweet tweet)
    {
        var result = new CustomTwitterModel();
        try
        {
            var telegramText = "";
            var quotedText = "";
            ITweet targetTweet = null;
            if (tweet.IsRetweet)
            {
                targetTweet = tweet.RetweetedTweet;
            }
            else
            {
                targetTweet = tweet;
            }

            if (tweet.QuotedTweet != null)
            {
                quotedText = $@"

<b>{tweet.QuotedTweet.CreatedBy.Name}</b>

{tweet.QuotedTweet.Text}

{$"{tweet.QuotedTweet.RetweetCount:n0}"} ↩️       {$"{tweet.QuotedTweet.FavoriteCount:n0}"} ❤️ 

{DateTimeOffset.UtcNow.Subtract(tweet.QuotedTweet.CreatedAt.ToUniversalTime()).GetRelativeTimeInFarsi()}
{$"<a href='{tweet.QuotedTweet.Url}'>لینک توییت</a>"}
{$"<a href='https://twitter.com/{targetTweet.CreatedBy.ScreenName}'>@{tweet.QuotedTweet.CreatedBy.ScreenName}</a>"}
<b>{$"{tweet.QuotedTweet.CreatedBy.FollowersCount:n0}"}</b> Followers          <b>{$"{tweet.QuotedTweet.CreatedBy.FriendsCount:n0}"}</b> Followings

";
            }

            telegramText = $@"

<b>{targetTweet.CreatedBy.Name}</b>

{targetTweet.Text}

{$"{targetTweet.RetweetCount:n0}"} ↩️       {$"{targetTweet.FavoriteCount:n0}"} ❤️ 

{DateTimeOffset.UtcNow.Subtract(targetTweet.CreatedAt.ToUniversalTime()).GetRelativeTimeInFarsi()}
{$"<a href='{targetTweet.Url}'>لینک توییت</a>"}
{$"<a href='https://twitter.com/{targetTweet.CreatedBy.ScreenName}'>@{targetTweet.CreatedBy.ScreenName}</a>"}
<b>{$"{targetTweet.CreatedBy.FollowersCount:n0}"}</b> Followers          <b>{$"{targetTweet.CreatedBy.FriendsCount:n0}"}</b> Followings

";
            if (!string.IsNullOrEmpty(quotedText))
            {
                telegramText = telegramText + @"

⚠️ توییت بالا پاسخی است به توییت پایین ⚠️


" + quotedText;
            }

            var targetMedia = tweet.QuotedTweet != null ? targetTweet.QuotedTweet.Entities.Medias.Any() ? targetTweet.QuotedTweet.Entities.Medias : targetTweet.Entities.Medias : targetTweet.Entities.Medias;
            result.TweetText = telegramText;
            result.MediaLink = targetMedia.FirstOrDefault()?.MediaURL;
            if (targetMedia != null && targetMedia.Any())
            {
                if (targetMedia.FirstOrDefault().MediaType == "video")
                {
                    result.MediaLink = targetMedia.FirstOrDefault().VideoDetails.Variants.Where(i=>i.Bitrate>0).OrderBy(i=>i.Bitrate).FirstOrDefault().URL;
                    result.Type = CustomTwitterModel.TweetType.Video;
                }
                else
                {
                    result.Type = CustomTwitterModel.TweetType.Photo;
                }
            }
            else
            {
                result.Type = CustomTwitterModel.TweetType.Text;
            }
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error in get tweet detail");
            return null;
        }
    }
    #endregion
}
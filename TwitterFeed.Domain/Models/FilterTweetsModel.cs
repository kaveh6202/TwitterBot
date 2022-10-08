namespace TwitterFeed.Infra.Models;

public class FilterTweetsModel
{
    public IEnumerable<string> ExcludeAccounts { get; set; }
    public IEnumerable<string> ExcludeHashTags { get; set; }
    public IEnumerable<string> ExcludeWords { get; set; }
    public IEnumerable<string> ContainsHashTags { get; set; }
    public IEnumerable<string> ContainsAccounts { get; set; }
    public IEnumerable<string> ContainsLanguages { get; set; }
}

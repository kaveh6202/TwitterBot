namespace TwitterFeed.Infra.Interface;

public interface IRepository
{
    public long GetLastTimelineId();
    public void UpdateLastTimelineId(long id);
}

public class Repository : IRepository
{
    private readonly string _filePath;

    public Repository()
    {
        _filePath = Path.Combine(Environment.CurrentDirectory, "Files", "LastTwitterTimelineId.txt");
    }
    public long GetLastTimelineId()
    {
        if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "0");
        var id = File.ReadAllText(_filePath);
        return long.Parse(id);
    }

    public void UpdateLastTimelineId(long id)
    {
        File.WriteAllText(_filePath, id.ToString());
    }
}
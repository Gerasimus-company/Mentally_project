namespace Mentally_project.Services;

public interface IDataStorageService
{
    Task SaveUserAsync(User user);
    Task<User?> GetUserAsync();
    Task SaveTestResultAsync(TestResult result);
    Task<List<TestResult>> GetTestResultsAsync();
    Task<List<TestResult>> GetTestResultsByDateAsync(DateTime date);
    Task DeleteTestResultAsync(string testId, DateTime completedAt);
    Task SaveMoodForDateAsync(DateTime date, string mood);
    Task<string?> GetMoodForDateAsync(DateTime date);
}

public class User
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Theme { get; set; } = "dark";
    public string Language { get; set; } = "ru";
}

public class DataStorageService : IDataStorageService
{
    private readonly string _userKey = "user_data";
    private readonly string _resultsKey = "test_results";
    private readonly string _moodsKey = "mood_data";
    
    // In-memory storage for demo (would use Preferences/SQLite in production)
    private static User? _cachedUser;
    private static List<TestResult> _cachedResults = new();
    private static Dictionary<string, string> _cachedMoods = new();

    public async Task SaveUserAsync(User user)
    {
        _cachedUser = user;
        // In production: await Preferences.SetAsync(_userKey, JsonSerializer.Serialize(user));
        await Task.CompletedTask;
    }

    public async Task<User?> GetUserAsync()
    {
        // In production: var json = await Preferences.GetAsync(_userKey);
        return _cachedUser;
    }

    public async Task SaveTestResultAsync(TestResult result)
    {
        _cachedResults.Add(result);
        // In production: save to persistent storage
        await Task.CompletedTask;
    }

    public async Task<List<TestResult>> GetTestResultsAsync()
    {
        return _cachedResults.ToList();
    }

    public async Task<List<TestResult>> GetTestResultsByDateAsync(DateTime date)
    {
        return _cachedResults.Where(r => r.CompletedAt.Date == date.Date).ToList();
    }

    public async Task DeleteTestResultAsync(string testId, DateTime completedAt)
    {
        var result = _cachedResults.FirstOrDefault(r => 
            r.TestId == testId && r.CompletedAt == completedAt);
        if (result != null)
        {
            _cachedResults.Remove(result);
        }
        await Task.CompletedTask;
    }

    public async Task SaveMoodForDateAsync(DateTime date, string mood)
    {
        var key = date.ToString("yyyy-MM-dd");
        _cachedMoods[key] = mood;
        await Task.CompletedTask;
    }

    public async Task<string?> GetMoodForDateAsync(DateTime date)
    {
        var key = date.ToString("yyyy-MM-dd");
        _cachedMoods.TryGetValue(key, out var mood);
        return mood;
    }
}

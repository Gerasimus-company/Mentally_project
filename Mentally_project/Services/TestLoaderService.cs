using System.Linq;
using System.Text.Json;

namespace Mentally_project.Services;

public interface ITestLoaderService
{
    Task<List<TestDefinition>> LoadTestsAsync();
    Task<TestDefinition?> LoadTestByIdAsync(string testId);
    Task<bool> ImportTestFromJsonAsync(string jsonContent);
    Task<bool> DeleteTestAsync(string testId);
    Task<List<string>> GetUploadedTestIdsAsync();
}

public class TestLoaderService : ITestLoaderService
{
    private const string TestsFolder = "tests";
    private readonly Dictionary<string, TestDefinition> _loadedTests = new();
    private readonly List<string> _uploadedTestIds = new();

    public async Task<List<TestDefinition>> LoadTestsAsync()
    {
        var tests = new List<TestDefinition>();
        
        // Load built-in tests from embedded resources
        await LoadBuiltInTestsAsync(tests);
        
        return tests;
    }

    public async Task<TestDefinition?> LoadTestByIdAsync(string testId)
    {
        if (_loadedTests.TryGetValue(testId, out var test))
        {
            return test;
        }
        
        // Try to load from embedded resources
        return await LoadTestFromResourceAsync(testId);
    }

    public async Task<bool> ImportTestFromJsonAsync(string jsonContent)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var test = JsonSerializer.Deserialize<TestDefinition>(jsonContent, options);
            
            if (test == null)
                return false;
                
            // Validate the test structure
            if (!ValidateTest(test))
                return false;
            
            // Add to loaded tests
            _loadedTests[test.Id] = test;
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing test: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteTestAsync(string testId)
    {
        return _loadedTests.Remove(testId);
    }

    public async Task<List<string>> GetUploadedTestIdsAsync()
    {
        return _loadedTests.Keys.ToList();
    }

    private async Task LoadBuiltInTestsAsync(List<TestDefinition> tests)
    {
        // Load MBTI test from embedded resource
        var mbtiTest = await LoadTestFromResourceAsync("mbti_test_001");
        if (mbtiTest != null)
        {
            tests.Add(mbtiTest);
            _loadedTests[mbtiTest.Id] = mbtiTest;
        }
    }

    private async Task<TestDefinition?> LoadTestFromResourceAsync(string testId)
    {
        try
        {
            var fileName = $"{testId}.json";
            using var stream = await FileSystem.Current.OpenAppPackageFileAsync(Path.Combine(TestsFolder, fileName));
            if (stream == null)
                return null;
                
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var test = JsonSerializer.Deserialize<TestDefinition>(json, options);
            return test;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading test {testId}: {ex.Message}");
            return null;
        }
    }

    private bool ValidateTest(TestDefinition test)
    {
        if (string.IsNullOrWhiteSpace(test.Id))
            return false;
            
        if (string.IsNullOrWhiteSpace(test.Title))
            return false;
            
        if (test.Questions == null || test.Questions.Count == 0)
            return false;
            
        foreach (var question in test.Questions)
        {
            if (string.IsNullOrWhiteSpace(question.Text))
                return false;
                
            if (question.Answers == null || question.Answers.Count == 0)
                return false;
        }
        
        if (test.Results == null)
            return false;
            
        return true;
    }
}

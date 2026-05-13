namespace Mentally_project.Models;

public class TestDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int QuestionsCount { get; set; }
    public string Icon { get; set; } = string.Empty;
    public List<string> ColorGradient { get; set; } = new();
    public string? ImageUrl { get; set; }
    public List<Question> Questions { get; set; } = new();
    public ResultsConfig Results { get; set; } = new();
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<Answer> Answers { get; set; } = new();
}

public class Answer
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Dictionary<string, int> Score { get; set; } = new();
}

public class ResultsConfig
{
    public string CalculationType { get; set; } = string.Empty;
    public List<Dimension> Dimensions { get; set; } = new();
    public List<ResultType> Types { get; set; } = new();
}

public class Dimension
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class ResultType
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public class TestResult
{
    public string TestId { get; set; } = string.Empty;
    public string TestTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public string ResultCode { get; set; } = string.Empty;
    public string ResultTitle { get; set; } = string.Empty;
    public string ResultDescription { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public Dictionary<string, int> Scores { get; set; } = new();
    public List<string> SelectedAnswers { get; set; } = new();
}

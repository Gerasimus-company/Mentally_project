using Mentally_project.Models;
using Mentally_project.Services;

namespace Mentally_project.ViewModels;

public class TestPassingViewModel : BindableObject
{
    private readonly ITestLoaderService _testLoaderService;
    private readonly IResultsCalculatorService _resultsCalculatorService;
    private readonly IDataStorageService _storageService;
    
    private TestDefinition? _currentTest;
    private int _currentQuestionIndex;
    private readonly Dictionary<int, string> _answers = new();
    private bool _isCompleted;

    public event EventHandler<TestResult>? TestCompleted;

    public TestPassingViewModel(
        ITestLoaderService testLoaderService,
        IResultsCalculatorService resultsCalculatorService,
        IDataStorageService storageService)
    {
        _testLoaderService = testLoaderService;
        _resultsCalculatorService = resultsCalculatorService;
        _storageService = storageService;
    }

    public TestDefinition? CurrentTest
    {
        get => _currentTest;
        set => SetProperty(ref _currentTest, value);
    }

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        set
        {
            if (SetProperty(ref _currentQuestionIndex, value))
            {
                OnPropertyChanged(nameof(CurrentQuestionNumber));
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(IsFirstQuestion));
                OnPropertyChanged(nameof(IsLastQuestion));
            }
        }
    }

    public int CurrentQuestionNumber => CurrentQuestionIndex + 1;

    public Question? CurrentQuestion => 
        CurrentTest?.Questions.ElementAtOrDefault(CurrentQuestionIndex);

    public bool IsFirstQuestion => CurrentQuestionIndex == 0;

    public bool IsLastQuestion => 
        CurrentQuestionIndex == (CurrentTest?.Questions.Count - 1 ?? 0);

    public int TotalQuestions => CurrentTest?.Questions.Count ?? 0;

    public double Progress => 
        TotalQuestions > 0 ? (double)(CurrentQuestionIndex + 1) / TotalQuestions : 0;

    public async Task LoadTestAsync(string testId)
    {
        CurrentTest = await _testLoaderService.LoadTestByIdAsync(testId);
        CurrentQuestionIndex = 0;
        _answers.Clear();
        _isCompleted = false;
    }

    public void SelectAnswer(string answerId)
    {
        if (CurrentQuestion != null)
        {
            _answers[CurrentQuestion.Id] = answerId;
        }
    }

    public bool HasSelectedAnswer => 
        CurrentQuestion != null && _answers.ContainsKey(CurrentQuestion.Id);

    public string? GetSelectedAnswerId => 
        CurrentQuestion != null && _answers.TryGetValue(CurrentQuestion.Id, out var id) ? id : null;

    public void GoToNextQuestion()
    {
        if (!IsLastQuestion)
        {
            CurrentQuestionIndex++;
        }
    }

    public void GoToPreviousQuestion()
    {
        if (!IsFirstQuestion)
        {
            CurrentQuestionIndex--;
        }
    }

    public async Task<TestResult?> CompleteTestAsync()
    {
        if (CurrentTest == null || _isCompleted)
            return null;

        // Validate all questions are answered
        if (_answers.Count != CurrentTest.Questions.Count)
            return null;

        _isCompleted = true;
        
        // Calculate result
        var result = _resultsCalculatorService.CalculateResult(CurrentTest, _answers);
        
        // Save to storage
        await _storageService.SaveTestResultAsync(result);
        
        TestCompleted?.Invoke(this, result);
        
        return result;
    }

    public void Reset()
    {
        CurrentTest = null;
        CurrentQuestionIndex = 0;
        _answers.Clear();
        _isCompleted = false;
    }
}

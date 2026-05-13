using Mentally_project.Models;
using Mentally_project.Services;

namespace Mentally_project.Pages;

public partial class TestPassingPage : ContentPage
{
    private readonly ITestLoaderService _testLoaderService;
    private readonly IResultsCalculatorService _resultsCalculatorService = new ResultsCalculatorService();
    private readonly IDataStorageService _storageService = new DataStorageService();
    private TestDefinition? _currentTest;
    private int _currentQuestionIndex;
    private readonly Dictionary<int, string> _answers = new();

    public TestPassingPage(ITestLoaderService testLoaderService)
    {
        _testLoaderService = testLoaderService;
        InitializeComponent();
        BindingContext = this;
    }

    public async Task LoadTestAsync(string testId)
    {
        _currentTest = await _testLoaderService.LoadTestByIdAsync(testId);
        _currentQuestionIndex = 0;
        _answers.Clear();
        
        if (_currentTest != null)
        {
            UpdateQuestionDisplay();
            UpdateProgress();
        }
    }

    private void UpdateQuestionDisplay()
    {
        if (_currentTest == null || _currentQuestionIndex >= _currentTest.Questions.Count)
            return;

        var question = _currentTest.Questions[_currentQuestionIndex];
        QuestionNumberLabel.Text = $"{_currentQuestionIndex + 1}/{_currentTest.Questions.Count}";
        QuestionTextLabel.Text = question.Text;
        
        // Clear and rebuild answers
        AnswersStackLayout.Children.Clear();
        foreach (var answer in question.Answers)
        {
            var button = new Button
            {
                Text = answer.Text,
                FontFamily = "RMR",
                FontSize = 14,
                TextColor = Colors.White,
                BackgroundColor = Color.FromArgb("#25D9d9d9"),
                CornerRadius = 12,
                Padding = new Thickness(15),
                Margin = new Thickness(0, 5),
                HorizontalOptions = LayoutOptions.Fill,
                CommandParameter = answer.Id
            };
            button.Clicked += OnAnswerSelected;
            AnswersStackLayout.Children.Add(button);
        }
    }

    private void UpdateProgress()
    {
        if (_currentTest == null)
            return;
            
        ProgressLabel.Text = $"{_currentQuestionIndex + 1}/{_currentTest.Questions.Count}";
        var progress = (double)(_currentQuestionIndex + 1) / _currentTest.Questions.Count;
        ProgressBar.Progress = progress;
    }

    private async void OnAnswerSelected(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string answerId)
        {
            // Highlight selected answer
            foreach (var child in AnswersStackLayout.Children)
            {
                if (child is Button btn)
                {
                    btn.BackgroundColor = Color.FromArgb("#25D9d9d9");
                }
            }
            button.BackgroundColor = Color.FromArgb("#667eea");
            
            // Store answer
            var question = _currentQuestionIndex < _currentTest?.Questions.Count ? _currentTest.Questions[_currentQuestionIndex] : null;
            if (question != null)
            {
                _answers[question.Id] = answerId;
            }
            
            // Enable Next button
            NextButton.IsEnabled = true;
        }
    }

    private void OnPreviousClicked(object sender, EventArgs e)
    {
        if (_currentQuestionIndex > 0)
        {
            _currentQuestionIndex--;
            UpdateQuestionDisplay();
            UpdateProgress();
            var questionId = _currentTest?.Questions.Count > _currentQuestionIndex ? _currentTest.Questions[_currentQuestionIndex].Id : -1;
            NextButton.IsEnabled = questionId >= 0 && _answers.ContainsKey(questionId);
        }
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_currentTest == null)
            return;

        if (_currentQuestionIndex < _currentTest.Questions.Count - 1)
        {
            _currentQuestionIndex++;
            UpdateQuestionDisplay();
            UpdateProgress();
            NextButton.IsEnabled = false;
        }
        else
        {
            // Complete test
            await CompleteTestAsync();
        }
    }

    private async Task CompleteTestAsync()
    {
        if (_currentTest == null || _answers.Count != _currentTest.Questions.Count)
            return;

        try
        {
            // Calculate result
            var result = _resultsCalculatorService.CalculateResult(_currentTest, _answers);
            
            // Save to storage
            await _storageService.SaveTestResultAsync(result);
            
            // Navigate to results page
            var resultPage = new ResultPage(result);
            await Navigation.PushAsync(resultPage);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось завершить тест: {ex.Message}", "OK");
        }
    }
}

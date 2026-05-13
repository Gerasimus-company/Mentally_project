using Mentally_project.Models;
using Mentally_project.Services;

namespace Mentally_project.Pages;

public partial class TestsListPage : ContentPage
{
    private readonly ITestLoaderService _testLoaderService;
    private List<TestDefinition> _tests = [];

    public TestsListPage(ITestLoaderService testLoaderService)
    {
        _testLoaderService = testLoaderService;
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTestsAsync();
    }

    private async Task LoadTestsAsync()
    {
        try
        {
            _tests = await _testLoaderService.LoadTestsAsync();
            TestsCollectionView.ItemsSource = _tests;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось загрузить тесты: {ex.Message}", "OK");
        }
    }

    private async void OnTestSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is TestDefinition selectedTest)
        {
            TestsCollectionView.SelectedItem = null; // Clear selection
            
            // Navigate to test passing page
            var testPassingPage = new TestPassingPage(_testLoaderService);
            await testPassingPage.LoadTestAsync(selectedTest.Id);
            await Navigation.PushAsync(testPassingPage);
        }
    }

    private async void OnImportTestClicked(object sender, EventArgs e)
    {
        try
        {
            // In production, use FilePicker to select a JSON file
            // For now, show a message
            await DisplayAlertAsync("Импорт теста", 
                "Выберите JSON файл с тестом. В реальной версии откроется файловый менеджер.", 
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось импортировать тест: {ex.Message}", "OK");
        }
    }
}

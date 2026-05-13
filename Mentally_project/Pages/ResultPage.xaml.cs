using Mentally_project.Models;
using Mentally_project.Services;

namespace Mentally_project.Pages;

public partial class ResultPage : ContentPage
{
    private readonly TestResult _result;

    public ResultPage(TestResult result)
    {
        _result = result;
        InitializeComponent();
        BindingContext = this;
        
        // Populate result data
        ResultCodeLabel.Text = _result.ResultCode;
        ResultTitleLabel.Text = _result.ResultTitle;
        ResultDescriptionLabel.Text = _result.ResultDescription;
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = $"Мой результат теста: {_result.ResultTitle} ({_result.ResultCode})",
            Title = "Mentally - Результат теста"
        });
    }

    private async void OnSaveToDiaryClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Сохранено", "Результат сохранён в дневник", "OK");
        await Navigation.PopToRootAsync();
    }

    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}

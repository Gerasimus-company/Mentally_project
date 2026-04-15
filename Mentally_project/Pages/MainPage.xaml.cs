namespace Mentally_project.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть настройки", "OK");
        // await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnTestsClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть тесты", "OK");
        // await Navigation.PushAsync(new TestsPage());
    }

    private async void OnAdviceClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть советы", "OK");
        // await Navigation.PushAsync(new AdvicePage());
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть помощь", "OK");
        // await Navigation.PushAsync(new HelpPage());
    }

    private async void OnDiaryClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть дневник", "OK");
        // await Navigation.PushAsync(new DiaryPage());
    }

    private async void OnSummaryClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Нажатие", "Открыть сводку", "OK");
        // await Navigation.PushAsync(new SummaryPage());
    }
}
namespace Mentally_project.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Настройки", "Страница настроек в разработке", "OK");
        // await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnTestsClicked(object sender, EventArgs e)
    {
        var testsPage = new TestsListPage(new TestLoaderService());
        await Navigation.PushAsync(testsPage);
    }

    private async void OnAdviceClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Советы", "Страница советов в разработке", "OK");
        // await Navigation.PushAsync(new AdvicePage());
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Помощь", "Страница помощи в разработке", "OK");
        // await Navigation.PushAsync(new HelpPage());
    }

    private async void OnDiaryClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Дневник", "Страница дневника в разработке", "OK");
        // await Navigation.PushAsync(new DiaryPage());
    }

    private async void OnSummaryClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Сводка", "Страница сводки в разработке", "OK");
        // await Navigation.PushAsync(new SummaryPage());
    }
}
using Microsoft.Extensions.Logging;

namespace Mentally_project
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("RobotoMono-Regular.ttf", "RMR");
                    fonts.AddFont("RobotoMono-Italic.ttf", "RMI");
                    fonts.AddFont("RobotoMono-Light.ttf", "RML");
                    fonts.AddFont("RobotoMono-LightItalic.ttf", "RMLI");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

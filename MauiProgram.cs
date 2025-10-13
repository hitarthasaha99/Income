using Blazored.Toast;
using Income.Validators.SCH0_0;
using Microsoft.Extensions.Logging;

namespace Income
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddSingleton(new HttpClient());
            builder.Services.AddSingleton<Income.Services.NetworkService>();
            //builder.Services.AddSingleton<Income.Services.IncomeService>();

            builder.Services.AddSingleton<Viewmodels.SCH0_0.Block_0_1_VM>();
            builder.Services.AddSingleton<Viewmodels.SCH0_0.Block_2_1_VM>();
            //builder.Services.AddValidatorsFromAssemblyContaining<Block_0_1_Validator>();
            //builder.Services.AddFluentValidationAutoValidation();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

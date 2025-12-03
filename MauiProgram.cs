using Blazored.Toast;
using FluentValidation;
using Income.Common;
using Income.Database.Models.HIS_2026;
using Income.Services;
using Income.Validators.HIS2026;
using Income.Validators.SCH0_0;
using Income.Viewmodels;
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
            builder.Services.AddSingleton<Income.Services.LocalizationService>();
            builder.Services.AddSingleton<LanguageState>();
            //builder.Services.AddSingleton<Income.Services.IncomeService>();

            builder.Services.AddSingleton<Viewmodels.SCH0_0.Block_0_1_VM>();
            builder.Services.AddSingleton<Viewmodels.SCH0_0.Block_2_1_VM>();
            builder.Services.AddSingleton<Viewmodels.SCH0_0.Block_2_2_VM>();
            builder.Services.AddSingleton<Comment_vm>();
            builder.Services.AddValidatorsFromAssemblyContaining<Block_0_1_Validator>();
            builder.Services.AddValidatorsFromAssemblyContaining<Block7Validator>();
            builder.Services.AddValidatorsFromAssemblyContaining<Block_4_Validator>();
            builder.Services.AddValidatorsFromAssemblyContaining<Block_4_Q5_Validator>();
            builder.Services.AddScoped<IValidator<Tbl_Block_4>, Block_4_Validator>();


            //builder.Services.AddValidatorsFromAssemblyContaining<Block_0_1_Validator>();
            //builder.Services.AddFluentValidationAutoValidation();
            builder.Services.Configure<AppConfig>(
    builder.Configuration.GetSection("AppConfig"));

            builder.Services.AddSingleton<IGlobalExceptionHandler, GlobalExceptionHandler>();
            builder.Services.AddSingleton<ILoggingService, LoggingService>();
            builder.Services.AddSingleton<Viewmodels.Warning_VM>();


            builder.Services.AddBlazoredToast();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

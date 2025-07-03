using System.Reflection;
using ColetorA41.Services;
using ColetorA41.ViewModel;
using ColetorA41.Views;
using ColetorA41.Views.Calculo;
using ColetorA41.Views.Monitor;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using ColetorA41.Pages;

using System.Globalization; //FAS

namespace ColetorA41
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR"); //FAS
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR"); //FAS

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Gotham-Medium.ttf", "Gottham");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Registrar Arquivos de configuracao
            builder.AddAppSettings();

            


#if DEBUG
            builder.Logging.AddDebug();
#endif

            #region Services
            builder.Services.AddSingleton<TotvsService>();
            builder.Services.AddSingleton<TotvsService46>();
            #endregion

            #region HttpClient
            builder.Services.AddHttpClient("coletor", httpClient =>
            {

            })
            .ConfigurePrimaryHttpMessageHandler(() => DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler());

            /*new HttpClientHandler
            {
                
                //ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            });
            */
            
               //DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler()
            
            
            #endregion

            #region Views
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<LoginAlmoxa>();
            builder.Services.AddTransient<Loading>();
            builder.Services.AddTransient<LoadingCalculo>();
            builder.Services.AddTransient<EstabTec>();
            builder.Services.AddTransient<DadosNF>();
            builder.Services.AddTransient<ExtrakitView>();
            builder.Services.AddTransient<LeituraENC>();
            builder.Services.AddTransient<ColetorA41.Views.Calculo.Resumo>();
            builder.Services.AddTransient<ResumoDetalhe>();
            builder.Services.AddTransient<ResumoDetalhePago>();
            builder.Services.AddTransient<ResumoDetalheEntrada>(); //FAS
            builder.Services.AddTransient<ResumoDetalheItem>();
            builder.Services.AddTransient<Erro>();
            builder.Services.AddTransient<Processos>();
            builder.Services.AddTransient<ResumoFinal>();
            builder.Services.AddTransient<Embalagem>();
            builder.Services.AddTransient<EmbalagemLoading>();
            builder.Services.AddTransient<EmbalagemPrimeiraNota>();
            builder.Services.AddTransient<Reparo>();
            builder.Services.AddTransient<ReparoEdicaoItemReparo>();
            builder.Services.AddTransient<ZoomTransporte>();
            builder.Services.AddTransient<LeituraPagtos>();
          
            #endregion

            #region ViewModel
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<CalculoViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddSingleton<MonitorViewModel>();
            #endregion

            return builder.Build();
        }

        private static void AddAppSettings(this MauiAppBuilder builder)
        {
            

#if DEBUG
            builder.AddJsonSettings("appsettings.desenv.json");
#endif

#if !DEBUG
            builder.AddJsonSettings("appsettings.prod.json");
#endif
        }

        private static void AddJsonSettings(this MauiAppBuilder builder, string fileName)
        {
            using Stream stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"ColetorA41.{fileName}");

            if (stream != null)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
                builder.Configuration.AddConfiguration(config);
            }
        }
    }
}

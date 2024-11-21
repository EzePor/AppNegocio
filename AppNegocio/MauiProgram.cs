using AppNegocio.Models.Commons;
using AppNegocio.Models.Details;
using AppNegocio.Services;
using AppNegocio.ViewModels.PedidoVM;
using AppNegocio.Views.PedidoV;
using Microsoft.Extensions.Logging;

namespace AppNegocio
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<GenericService<Cliente>, GenericService<Cliente>>();
            builder.Services.AddSingleton<GenericService<Producto>,GenericService<Producto>> ();
            builder.Services.AddSingleton<GenericService<ModoPago>, GenericService<ModoPago>>();
            builder.Services.AddSingleton<GenericService<Producto>, GenericService<Producto>> ();
            builder.Services.AddSingleton<GenericService<Impresion>, GenericService<Impresion>>();
            builder.Services.AddSingleton<GenericService<Pedido>, GenericService<Pedido>>();

            builder.Services.AddTransient<CrearPedidoViewModel>();
            builder.Services.AddTransient<CrearPedidoView>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

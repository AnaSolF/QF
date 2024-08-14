using Microsoft.Extensions.Logging;
//Agregamos referencia
using QF.DataAccess;
using QF.ViewModels;
using QF.Views;

namespace QF
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
            //Agregamos debajo de builder. Creamos la instancia db /Se crea la 1° vez que se ejecuta la app
            var dbContext = new ProjectDBContext();
            dbContext.Database.EnsureCreated();
            //Liberamos la bd
            dbContext.Dispose();

            //Instanciamos base de datos(Usa el contexto de la bd)
            builder.Services.AddDbContext<ProjectDBContext>();
            //Agregamos para hacer binding con ParticipantePage
            builder.Services.AddTransient<ParticipantePage>();
            builder.Services.AddTransient<ParticipanteViewModel>();
            //Agregamos para hacer el binding con MainPage,
            //que en este momento serìa la pàgina de ediciòn
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CreatePartViewModel>();

            //Agregamos funcionalidad para ir desde una pàgina a otra. Sòlo redirigimos a Participante
            Routing.RegisterRoute(nameof(ParticipantePage), typeof(ParticipantePage));


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

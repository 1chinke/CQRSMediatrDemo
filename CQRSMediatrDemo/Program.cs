using Demo.Mediatr;
using Demo.Mediatr.Behaviors;
using Demo.Repository;
using Demo.Validators.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;


// Serilogu iki a�amal� olarak yap�land�r�yoruz.
// 1) ASP.NET Core uygulamas�n�n ba�lat�lmas�
// 2) Loglama ayarlar�n�n appsettings.json dosyas�ndan okunarak uygulaman�n di�er a�amalr�nda kullan�m�

// Buras� birinci k�s�m. Hen�z uygulama ba�lang�� a�amas�nda oldu�u i�in sadece console'a eri�imizi var.

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

try
{
    Log.Information("Web host ba�lat�l�yor....");
    var builder = WebApplication.CreateBuilder(args);

    // Serilog ikinci k�s�m: Ayarlar� appsettings.json dosyas�ndan al�yoruz.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //vmo: DI'lar buraya ekleniyor
    builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
    builder.Services.AddSingleton<IKullaniciRepo, KullaniciRepo>();

    builder.Services.AddMediatR(typeof(MediatrEntryPoint).Assembly); //bunun i�in nugetten mediatr.dependencyinjection paketini eklemek gerekiyor.
                                                                     //MediatrEntryPoint: Mediatr dizinindeki bo� class
    builder.Services.AddValidatorsFromAssembly(typeof(DomainValidationEntryPoint).Assembly);

    //Mediatr pipeline behavior DI. Buradaki s�raya g�re pipelinedaki her bir behavior �al���yor
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


    

    var app = builder.Build();

    // Her bir API call i�in loglama...
    /*app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "API {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
    });*/



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();


    if (!app.Environment.IsDevelopment())
    {        
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>();

                var detail = "";
                var status = StatusCodes.Status500InternalServerError;
                var title = "HATA!!!";
                if (ex != null)
                {
                    if (ex.Error is ValidationException)
                    {                        
                        status = StatusCodes.Status400BadRequest;
                        title = "Veri Do�rulama Hatas�!";
                        detail = ex.Error.Message;
                    } else
                    {
                        detail = ex.Error.Message;
                    }
                } else
                {
                    detail = "Beklenmeyen hata. L�tfen RequestId ile birlikte teknik deste�e bildirin";
                }

                var pd = new ProblemDetails
                {
                    Title = title,
                    Status = status,
                    Detail = detail,                    
                };

                pd.Extensions.Add("RequestId", context.TraceIdentifier);
                //pd.Extensions.Add("User", context.User);
              
                context.Response.StatusCode = status;
             
                await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
            });
        });
    }


    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;

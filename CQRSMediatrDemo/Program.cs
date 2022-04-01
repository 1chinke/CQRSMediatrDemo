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


// Serilogu iki aþamalý olarak yapýlandýrýyoruz.
// 1) ASP.NET Core uygulamasýnýn baþlatýlmasý
// 2) Loglama ayarlarýnýn appsettings.json dosyasýndan okunarak uygulamanýn diðer aþamalrýnda kullanýmý

// Burasý birinci kýsým. Henüz uygulama baþlangýç aþamasýnda olduðu için sadece console'a eriþimizi var.

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

try
{
    Log.Information("Web host baþlatýlýyor....");
    var builder = WebApplication.CreateBuilder(args);

    // Serilog ikinci kýsým: Ayarlarý appsettings.json dosyasýndan alýyoruz.
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

    builder.Services.AddMediatR(typeof(MediatrEntryPoint).Assembly); //bunun için nugetten mediatr.dependencyinjection paketini eklemek gerekiyor.
                                                                     //MediatrEntryPoint: Mediatr dizinindeki boþ class
    builder.Services.AddValidatorsFromAssembly(typeof(DomainValidationEntryPoint).Assembly);

    //Mediatr pipeline behavior DI. Buradaki sýraya göre pipelinedaki her bir behavior çalýþýyor
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


    

    var app = builder.Build();

    // Her bir API call için loglama...
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
                        title = "Veri Doðrulama Hatasý!";
                        detail = ex.Error.Message;
                    } else
                    {
                        detail = ex.Error.Message;
                    }
                } else
                {
                    detail = "Beklenmeyen hata. Lütfen RequestId ile birlikte teknik desteðe bildirin";
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

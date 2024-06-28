using System.Net;
using Entities.ErrorModel;
using Entities.Expections;
using Microsoft.AspNetCore.Diagnostics;
using NuGet.Protocol;
using Services.Contracts;

namespace WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                //Response olarak dönecek hatanın yapısının json dosyası olduğunu belirtecez
                context.Response.ContentType = "application/json";

                //Hatayı yakalamaya çalışacaz
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    
                    //Status Code'u eror tipine göre switch case ile ayarlicaz
                    
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,//NotFound ise StaCod 400 olsun dedik
                        _ => StatusCodes.Status500InternalServerError //NotFound değilse StaCod 500 olarak ayarla
                    };
                    
                    //Loglama yapıcaz hata var olması durumunda
                    logger.LogError($"Something went wrong : {contextFeature.Error}");
                    
                    //Hata objesini ErrorDetails classı sayesinde oluşturacaz
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        message = contextFeature.Error.Message
                    }.ToString());
                    
                }
            });
        });
    }
}
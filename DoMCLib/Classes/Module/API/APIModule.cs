using DoMCModuleControl;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel;

namespace DoMCLib.Classes.Module.API
{
    [Description("API")]
    public partial class APIModule : AbstractModuleBase
    {
        WebApplication _app;
        public bool IsStarted { get; private set; } = false;
        public APIModule(IMainController MainController) : base(MainController)
        {
        }

        public async Task StartServer()
        {
            if (IsStarted) return;
            try
            {
                var builder = WebApplication.CreateBuilder();
                builder.Services.AddLogging(logging =>
                {
                    //logging.AddConsole();
                    logging.AddFile("Logs/restApi.log");
                    logging.SetMinimumLevel(LogLevel.Debug);
                });
                // Настройка сервисов
                builder.Services.AddControllers()
                    .AddApplicationPart(this.GetType().Assembly)
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        _app?.Logger.LogInformation("Controllers registered");
                    });
                ;

                //builder.WebHost.UseUrls("http://0.0.0.0:8080");
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(8080);// (8080, listenOptions => listenOptions.UseHttp());
                });
                builder.Services.AddControllers(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseRouteTransformer()));
                });

                _app = builder.Build();
                // Обработка ошибок
                _app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Internal Server Error. Check logs for details.");
                        _app.Logger.LogError("Unhandled error in request: {Path}", context.Request.Path);
                    });
                });

                //тут другие настройки сервера
                // ...
                // например _app.UseCookiePolicy();
                // ....


                // Настройка маршрутов
                _app.UseRouting();
                _app.MapControllers();

                _app.MapGet("/statusA", () => new { status = "Running", timestamp = DateTime.UtcNow });
                _app.MapGet("/statusB/{id}", (int id) => new { status = $"Running {id}" });
                _app.MapGet("/statusC", async (HttpContext context) =>
                {
                    // Сложная логика
                    return Results.Ok(new { status = "Running", timestamp = DateTime.UtcNow });
                });
                _app.MapGet("/statusD", () => Results.Json(new { status = "Running" }, statusCode: 200));
                _app.MapGet("/debug/routes", ([FromServices] IEnumerable<EndpointDataSource> endpointDataSources) =>
                {
                    try
                    {
                        _app.Logger.LogDebug("Accessing endpoint data sources");
                        var routes = new List<string>();

                        foreach (var dataSource in endpointDataSources)
                        {
                            foreach (var endpoint in dataSource.Endpoints)
                            {
                                var httpMethodMetadata = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>();
                                var routeEndpoint = endpoint as RouteEndpoint;

                                string displayName = endpoint.DisplayName ?? "Unnamed endpoint";
                                if (httpMethodMetadata != null && routeEndpoint != null)
                                {
                                    var methods = string.Join(",", httpMethodMetadata.HttpMethods);
                                    var pattern = routeEndpoint.RoutePattern.RawText;
                                    displayName = $"HTTP: {methods} {pattern}";
                                }

                                routes.Add(displayName);
                            }
                        }

                        if (!routes.Any())
                        {
                            _app.Logger.LogInformation("No routes found");
                            return Results.Ok("No routes found.");
                        }

                        _app.Logger.LogDebug("Returning {Count} routes", routes.Count);
                        return Results.Ok(string.Join("\n", routes));
                    }
                    catch (Exception ex)
                    {
                        _app.Logger.LogError(ex, "Error retrieving routes: {Message}", ex.Message);
                        return Results.Problem("Failed to retrieve routes: " + ex.Message, statusCode: 500);
                    }
                });

                await _app.StartAsync();
                MainController.GetObserver().Notify("REST.APIModule.Started", null);
                IsStarted = true;
                MainController.GetLogger(this.GetType().GetDescriptionOrName()).Add(DoMCModuleControl.Logging.LoggerLevel.Information, "Сервер REST APIModule запущен");
                _app.Logger.LogInformation("Server started on http://0.0.0.0:8080");
            }
            catch (Exception ex)
            {
                MainController.GetLogger(this.GetType().GetDescriptionOrName()).Add(DoMCModuleControl.Logging.LoggerLevel.Critical, "Ошибка при запуске сервера: ", ex);
                _app?.Logger.LogError(ex, "Failed to start server");
            }
        }
        public async Task StopServer()
        {
            if (!IsStarted || _app == null) return;
            try
            {
                await _app.StopAsync();
                _app.Logger.LogInformation("Server stopped");
                MainController.GetObserver().Notify("REST.APIModule.Stopped", null);
                MainController.GetLogger(this.GetType().GetDescriptionOrName()).Add(DoMCModuleControl.Logging.LoggerLevel.Information, "Сервер REST APIModule остановлен");
            }
            catch (Exception ex)
            {
                MainController.GetLogger(this.GetType().GetDescriptionOrName()).Add(DoMCModuleControl.Logging.LoggerLevel.Critical, "Ошибка: ", ex);
                _app.Logger.LogError(ex, "Failed to stop server");

            }
            finally
            {
                IsStarted = false;
                _app = null;
            }
        }
        public class LowercaseRouteTransformer : IOutboundParameterTransformer
        {
            public string TransformOutbound(object value) => value?.ToString()?.ToLowerInvariant();
        }

    }
}

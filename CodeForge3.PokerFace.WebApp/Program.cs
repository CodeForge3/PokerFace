using CodeForge3.PokerFace.Configurations;
using CodeForge3.PokerFace.MachineLearning.Extensions;
using CodeForge3.PokerFace.Services.Extensions;
using MudBlazor.Services;
using CodeForge3.PokerFace.WebApp.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(PokerFaceConfiguration.ConfigurationRoot);

builder.Services.AddYoloDetectionHandler(PokerFaceConfiguration.CurrentYoloModel);

builder.Services.AddPokerAppService();

builder.Services.AddMudServices();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();

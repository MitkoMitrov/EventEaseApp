using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventEaseApp.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options => {
    options.DetailedErrors = true;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(30);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(60);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
});

// Register application services
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddSingleton<AttendanceService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

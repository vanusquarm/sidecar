using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Main Business API", Version = "v1" });
});

// Configure HttpClient for Sidecar communication
var sidecarBaseUrl = builder.Configuration.GetValue<string>("SidecarSettings:BaseUrl") 
    ?? "http://localhost:5001";

builder.Services.AddHttpClient("SidecarClient", client =>
{
    client.BaseAddress = new Uri(sidecarBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true) // Always enable swagger for this demo
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

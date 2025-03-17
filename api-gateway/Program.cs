using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Добавляем контроллеры
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("auth", new OpenApiInfo { Title = "Auth Service", Version = "v1" });
    c.SwaggerDoc("cart", new OpenApiInfo { Title = "Cart Service", Version = "v1" });
});

var app = builder.Build();

// Включение Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/auth/swagger/v1/swagger.json", "Auth Service");
    options.SwaggerEndpoint("/cart/swagger/v1/swagger.json", "Cart Service");
    options.RoutePrefix = string.Empty;
});

// Используем YARP для перенаправления запросов
app.MapReverseProxy();

app.UseAuthorization();
app.MapControllers();

app.Run("http://*:9000");
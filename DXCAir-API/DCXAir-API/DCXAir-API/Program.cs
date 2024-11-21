var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Origen del frontend
              .AllowAnyHeader()  // Permite cualquier encabezado
              .AllowAnyMethod();  // Permite cualquier método HTTP
    });
});

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    if (logger == null) throw new ArgumentNullException(nameof(logger));

    logger.LogInformation("Hello, this is a log message!");

    return "Hello World!";
});

// Usar CORS
app.UseCors("AllowLocalhost4200");

// Habilitar Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";  // Puedes cambiar la ruta si lo deseas
});

// Configurar rutas para los controladores
app.MapControllers();

app.Run();

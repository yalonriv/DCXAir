var builder = WebApplication.CreateBuilder(args);

// Agregar servicios para controlar las solicitudes HTTP
builder.Services.AddControllers();  // Asegúrate de llamar a AddControllers()

// Si estás usando Swagger, agrega los servicios de Swagger también
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar configuración de CORS (si es necesario)
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

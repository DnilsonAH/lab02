var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Servicios necesarios para swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Dnis",
        Version = "v1",
        Description = "API para gestionar tareas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Teo",
            Email = "dni@tecsup.edu.pe",
            Url = new Uri("http://localhost:5281")
        }
    });
});

var app = builder.Build();


// Middleware de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","API de Tasks v1");
        options.RoutePrefix = "swagger"; // ruta de swgager en  /swagger
    });
}

// HTTPS y rutas
app.MapControllers();                    //Mapear controladores

app.Run();
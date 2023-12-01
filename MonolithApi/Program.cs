using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MonolithApi.Context;
using MonolithApi.Data;
using MonolithApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Register

builder.Services.AddDbContext<AppDatabaseContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DataContext")
        )
    );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Monolith api",
        Description = "This is an api of our monolith backend for our Architecture class (M1, Expert Dev Logiciel Mob Iot, YNOV)",
        Contact = new OpenApiContact
        {
            Name = "Sada Demba Thiam",
            Url = new Uri("https://github.com/SadaDemba")
        }

    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var provider = scope.ServiceProvider;
var context = provider.GetRequiredService<AppDatabaseContext>();
//context.Database.Migrate();
DataSeeder.Initialize(context);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

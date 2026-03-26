using FastEndpoints;
using FastEndpoints.Security;
using car_stock_api.Infrastructure.Database;
using car_stock_api.Infrastructure.Repositories;

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddFastEndpoints()
    .AddSingleton<DbConnectionFactory>()
    .AddSingleton<DBInitialiser>()
    // .AddSingleton<DbSeeder>() // Uncomment this line if you want to seed the database
    .AddScoped<CarRepository>()
    .AddScoped<UserRepository>()
    .AddAuthorization()
    .AddAuthenticationJwtBearer(s =>
    {
        s.SigningKey = bld.Configuration["Jwt:SigningKey"];
    });

var app = bld.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitialiser = scope.ServiceProvider
        .GetRequiredService<DBInitialiser>();

    await dbInitialiser.InitialiseAsync();

    // var seeder = scope.ServiceProvider // Uncomment this block if you want to seed the database
    //     .GetRequiredService<DbSeeder>();
    // await seeder.SeedAsync();
}

app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
    });

app.Run();

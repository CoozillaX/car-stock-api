using FastEndpoints;
using car_stock_api.Infrastructure.Database;
using car_stock_api.Infrastructure.Repositories;

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddFastEndpoints()
    .AddSingleton<DbConnectionFactory>()
    .AddSingleton<DBInitialiser>()
    .AddScoped<CarRepository>()
    .AddScoped<UserRepository>();

var app = bld.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitialiser = scope.ServiceProvider
        .GetRequiredService<DBInitialiser>();

    await dbInitialiser.InitialiseAsync();
}

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.Run();

using Ecommerce.Gateway.YARP.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContext<AppdbContext>(op =>
{
    op.UseNpgsql(builder.Configuration.GetConnectionString("PostreSql"));
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors(x =>x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapGet("/", () => "Hello World!");

app.MapReverseProxy();

using(var scope = app.Services.CreateScope())
{
    var srv = scope.ServiceProvider;
    var context = srv.GetRequiredService<AppdbContext>();
    context.Database.Migrate();
}

app.Run();

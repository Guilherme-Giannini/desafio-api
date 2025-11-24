using DesafioApi.Application.Interfaces;
using DesafioApi.Infrastructure.Data;
using DesafioApi.Infrastructure.Lock;
using DesafioApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseInMemoryDatabase("DesafioDb"));

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

var redisConn = builder.Configuration["Redis:Connection"] ?? "redis:6379";
builder.Services.AddSingleton<ILockService>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RedisLockService>>();
    return new RedisLockService(redisConn, logger);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();
    await repo.SeedAsync();
}

app.Run();
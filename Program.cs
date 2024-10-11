using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.BackgroundServices;
using WebApplication2.Persistence.Context;
using WebApplication2.PolygonIntegration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<UsersContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<PolygonClient>();
builder.Services.AddSingleton<PeriodicNotificationHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicNotificationHostedService>());
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new EmailNotificationService(
                    "sandbox.smtp.mailtrap.io", 587, "9f809b9b8730be", "9adcce0d7e6d22"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
//"twork3320@gmail.com","twork@332000"


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
    if (dbContext.Database.GetMigrations().Any())
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();

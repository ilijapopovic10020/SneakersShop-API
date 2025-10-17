using Microsoft.EntityFrameworkCore;
using SneakersShop.API.Core;
using SneakersShop.API.Extensions;
using SneakersShop.Application.Emails;
using SneakersShop.Application.Handling;
using SneakersShop.Application.Logging;
using SneakersShop.Application.Payment;
using SneakersShop.Application.Uploads;
using SneakersShop.Application.UseCases.DTO.Searches;
using SneakersShop.DataAccess;
using SneakersShop.Implementation.Emails;
using SneakersShop.Implementation.Handling;
using SneakersShop.Implementation.Logging;
using SneakersShop.Implementation.Paymenet;
using SneakersShop.Implementation.Uploads;

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();
builder.Configuration.Bind(settings);

// Add services to the container.
builder.Services.AddDbContext<SneakersShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton(settings);

builder.Services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();
builder.Services.AddTransient<IUseCaseLogger, ConsoleUseCaseLogger>();
builder.Services.AddTransient<IBase64FileUploader, Base64FileUploader>();

builder.Services.AddTransient<ICommandHandler, CommandHandler>();
builder.Services.AddTransient<IQueryHandler, QueryHandler>();
builder.Services.AddTransient<IEmailSender, FakeEmailSender>();

builder.Services.AddTransient<IPaymentProcessor, MockPaymentProcessor>();

builder.Services.AddJwt(settings);
builder.Services.AddUseCases();
builder.Services.AddValidators();
builder.Services.AddApplicationUser();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using AutoMapper;
using FraudDetection.Api;
using FraudDetection.Data.AnomalyLog;
using FraudDetection.Data.Entity;
using FraudDetection.Data.Transaction;
using FraudDetection.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Register DbContext with PostgreSQL connection
builder.Services.AddDbContext<FraudDetectionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAnomalyLogRepository, AnomalyLogRepository>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IAnomalyLogService, AnomalyLogService>();
builder.Services.AddTransient<IFraudDetectionService, FraudDetectionService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Your React app's URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var config = new MapperConfiguration(c => {
    c.AddProfile<DefaultAutoMapperProfile>();
});

builder.Services.AddSingleton(s => config.CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

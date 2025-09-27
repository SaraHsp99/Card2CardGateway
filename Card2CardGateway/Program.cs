using Card2CardGateway.Application;
using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Application.UseCases.Transfer.Options;
using Card2CardGateway.Application.UseCases.Transfer.Services;
using Card2CardGateway.Application.UseCases.Transfer.Validators;
using Card2CardGateway.Infrastructure.Persistence.Repositories;
using Card2CardGateway.Infrastructure.Persistence;
using FluentValidation;
using Card2CardGateway.Infrastructure.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.Configure<TransferOptions>(
    builder.Configuration.GetSection("TransferOptions"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Card2CardDb"));

builder.Services.AddTransient<IValidator<TransferRequestDto>, TransferRequestValidator>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TransactionService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();



app.UseMiddleware<CorrelationIdMiddleware>();

app.MapControllers();

app.Run();

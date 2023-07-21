using SehatNotebook.DataService.Data;
using SehatNotebook.DataService.IConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddDbContext<AppDBContext>(options=>
    options.UseSqlite(config.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddApiVersioning(opt=>
{    
    opt.ReportApiVersions=true;
    opt.AssumeDefaultVersionWhenUnspecified=true;
    opt.DefaultApiVersion=Microsoft.AspNetCore.Mvc.ApiVersion.Default;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

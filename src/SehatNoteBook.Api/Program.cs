using System.Text;
using SehatNotebook.DataService.Data;
using SehatNotebook.DataService.IConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SehatNoteBook.Authentication.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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

var key=Encoding.ASCII.GetBytes( config["JwtConfig:Secret"]);

var tokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey= true,
        IssuerSigningKey=new SymmetricSecurityKey(key),
        ValidateIssuer=false,
        ValidateAudience=false,
        ValidateLifetime=false,
        RequireExpirationTime=true
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.Configure<JwtConfig>(config.GetSection("JwtConfig"));

builder.Services.AddAuthentication(option=> {
    option.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt => {
    jwt.SaveToken=true;
    jwt.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddDefaultIdentity<IdentityUser>(opt=>
        opt.SignIn.RequireConfirmedAccount=true)
        .AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

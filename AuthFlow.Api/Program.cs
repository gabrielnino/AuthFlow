using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthFlow.Application.Interfaces;
using AuthFlow.Infraestructure.ExternalServices;
using AuthFlow.Infraestructure.Operations;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Use_cases.Interface.Operations;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDBDbContext");
builder.Services.AddDbContext<AuthFlowDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddLogging(logginBuilder =>
{
    logginBuilder.ClearProviders();
    logginBuilder.AddConsole();
    logginBuilder.AddDebug();
}
);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedSqlServerCache(redisOptons =>
{
    string connection = builder.Configuration.GetConnectionString("Redis");
    redisOptons.ConnectionString  = connection;
    redisOptons.SchemaName = "dbo";
    redisOptons.TableName = "CacheItems";
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});


var app = builder.Build();
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed((host) => true)
.AllowCredentials());


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

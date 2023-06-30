using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDBDbContext");
builder.Services.AddDbContext<AuthFlowDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddLogging(logginBuilder =>
{
    logginBuilder.ClearProviders();
    logginBuilder.AddConsole();
    logginBuilder.AddDebug();
}
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerGen();



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };

//});

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

app.UseAuthorization();

app.MapControllers();

app.Run();

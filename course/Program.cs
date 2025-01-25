using course.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters.ValidateIssuerSigningKey = true;
    o.TokenValidationParameters.ValidIssuer = course.Constants.Iss;
    o.TokenValidationParameters.ValidAudience = course.Constants.Aud;
    o.TokenValidationParameters.IssuerSigningKey = course.Constants.SymmetricSecurityKey;
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
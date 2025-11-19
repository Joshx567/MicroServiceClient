using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServiceClient.Application;
using ServiceClient.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// 1. Controladores y Dapper
// -----------------------
builder.Services.AddControllers();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// -----------------------
// 2. Inyección de dependencias
// -----------------------
builder.Services.AddScoped<IClientConnectionProvider, NpgsqlClientConnectionProvider>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// -----------------------
// 3. UserContext
// -----------------------
builder.Services.AddHttpContextAccessor();
// builder.Services.AddScoped<IUserContext, UserContext>();  // activar si lo implementas

// -----------------------
// 4. JWT
// -----------------------
var jwtKey = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// -----------------------
// 5. CORS
// -----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// -----------------------
// 6. Swagger
// -----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -----------------------
// Middleware
// -----------------------
app.UseCors("AllowWebApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // necesario para JWT
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PruebaSisteCredito.Infrastructure.Repositories.Impl;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.EntityFrameworkCore;
using PruebaSisteCredito.Domain.Context;


var builder = WebApplication.CreateBuilder(args);


// Clave secreta para firmar el token
var jwtKey = builder.Configuration["JWT:Key"] ?? "SuperSecretKey123!";
var issuer = builder.Configuration["JWT:Issuer"] ?? "PruebaSisteCredito";
var audience = builder.Configuration["JWT:Audience"] ?? "SisteCredito";

// Configurar el DbContext con la cadena de conexión desde appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IOverTimeRepository, OverTimeRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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

// Habilitar autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RHOnly", policy => policy.RequireRole("RH"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});
// Configura Kestrel para escuchar en todas las interfaces y en el puerto 80
//builder.WebHost.ConfigureKestrel(options => {  options.ListenAnyIP(80); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

// Usar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

//app.UseHttpsRedirection();
app.MapControllers();



app.Run();


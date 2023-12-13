
using Airline_Project.Controllers;
using Airline_Project.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using UniversityWebApplnUsingAsp.Net.Models.Domain;
using UniversityWebApplnUsingAsp.Net.Respositories.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//for entity
builder.Services.AddDbContext<DatabaseContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));
builder.Services.AddDbContext<AirlineDatabaseContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));

//builder.Services.AddDbContext<UniversitySystemContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
});
//for identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

//adding authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// adding jwt bearer

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EnrolledStudent", policy =>
        policy.RequireRole("Student"));
});
builder.Services.AddTransient<UniversityWebApplnUsingAsp.Net.Respositories.Abstract.Interface.ITokenService, TokenService>();
builder.Services.AddTransient<AdminFlightDetailController>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
    options.WithOrigins("*").
    AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

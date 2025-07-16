using Microsoft.EntityFrameworkCore;
using todoTask.Data;
using todoTask.Repositories;
using todoTask.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region CORS - Cross-Origin Resource Sharing
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        });
});

#endregion

#region Database Connection 
builder.Services.AddDbContext<MKTodotaskDbContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region AuthDbContext 
builder.Services.AddDbContext<AuthDbcontext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("AuthConeection")));
#endregion

#region Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Dependency Injection
builder.Services.AddScoped<ITodotaskRepository, SQLTodotaskRepository>();
#endregion

#region IDentity Core

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("todoTask")
    .AddEntityFrameworkStores<AuthDbcontext>()
    .AddDefaultTokenProviders();

/*
This are the password that we waant to configure, Like how many digits do
we want and how many uppercase or LOwercase and this length
*/
builder.Services.Configure<IdentityOptions>(option =>
{
    // This are Default option for Password 
    option.Password.RequireDigit = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});

#endregion

#region Jwt-Json Web Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
#endregion



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

// Add this for Jwt Authentication
app.UseAuthentication();

app.UseAuthorization();

// Cros Config
app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();

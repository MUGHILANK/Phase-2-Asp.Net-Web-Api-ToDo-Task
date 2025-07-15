using Microsoft.EntityFrameworkCore;
using todoTask.Data;
using todoTask.Repositories;
using todoTask.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//For image uploading
builder.Services.AddHttpContextAccessor();

#region cros - Cross Origin Resource Sharing
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database

builder.Services.AddDbContext<MKTodotaskDbContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Automapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region DI
builder.Services.AddScoped<ITodotaskRepository,SQLTodotaskRepository>();

//DI of the image Upload File 
builder.Services.AddScoped<IimageRepository,LocalImageRepository>();

#endregion

#region JWT Configuration


#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Image Config
// Image Static FIle COnfig, Now serving Static file From Our API
app.UseStaticFiles(new StaticFileOptions
{ 
    // Physical Folder
    FileProvider=new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    // Routing Path
    RequestPath = "/Images"

});

#endregion

app.UseAuthorization();

//Cors Config
app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();

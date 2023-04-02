using Application.Configurations;
using Data.Entities;
using Data.Hubs;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Utility.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
// Add services to the container.
builder.Services.AddDbContext<CarRentalContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
);
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSignalR();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDependenceInjection();
builder.Services.AddAutoMapper(typeof (GeneralProfile));

var app = builder.Build();

app.UseCors(x => x
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseJwt();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<LocationHub>("/locationHub");
});

app.MapControllers();

app.Run();

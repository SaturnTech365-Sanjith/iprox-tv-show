using AutoMapper;
using Iprox.Application.TvShowsApi.Configurations;
using Iprox.Domain.Interface.IRepositories;
using Iprox.Domain.Interface;
using Iprox.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Iprox.Domain.Core;
using Iprox.Application.TvShowsApi.Interfaces;
using Iprox.Application.TvShowsApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

string? connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IShowApiService, ShowApiService>();
builder.Services.AddScoped<IShowCoreService, ShowCoreService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



// Configure the HTTP request pipeline.
WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowedAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

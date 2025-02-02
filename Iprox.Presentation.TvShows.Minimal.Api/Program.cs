using AutoMapper;
using Iprox.Application.TvShowsApi.Interfaces;
using Iprox.Application.TvShowsApi.Services;
using Iprox.Domain.Interface.IRepositories;
using Iprox.Domain.Interface;
using Iprox.Presentation.TvShows.Minimal.Api.Configuration;
using Iprox.Presentation.TvShows.Minimal.Api.Endpoints.V1.V0;
using Iprox.Infrastructure.Persistence;
using Iprox.Application.TvShowsApi.Configurations;
using Iprox.Domain.Core;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.0", SwaggerConfig.CreateApiInfo("v1", "Iprox TV Shows API v1.0", "Iprox TV Shows is the world's leading source for television content, providing the latest information on newly released TV shows."));
    c.DocumentFilter<HideEndpointsFilter>();

});

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Iprox TV Shows API v1.0");
    });
}
app.UseHttpsRedirection();
app.UseCors("AllowedAll");
app.MapShowEndpointV1V0();
app.Run();
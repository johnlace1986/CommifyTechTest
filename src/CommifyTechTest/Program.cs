using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommifyTechTest;
using CommifyTechTest.Application;
using CommifyTechTest.Application.Settings;
using CommifyTechTest.Persistence;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.

builder.Host.ConfigureContainer<ContainerBuilder>(
    containerBuilder =>
    {
        containerBuilder.RegisterModule(new PresentationModule());
        containerBuilder.RegisterModule(new PersistenceModule());
        containerBuilder.RegisterModule(new ApplicationModule());
    });

builder.Services.Configure<TaxBandSettings>(builder.Configuration.GetSection("TaxBandSettings"));
builder.Services.AddSingleton(sp => sp.GetService<IOptions<TaxBandSettings>>().Value);

builder.Services.AddQuartz();
builder.Services.AddQuartzServer();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
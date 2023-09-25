using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommifyTechTest;
using CommifyTechTest.Application;
using CommifyTechTest.Jobs;
using CommifyTechTest.Services;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Impl;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.

builder.Host.ConfigureContainer<ContainerBuilder>(
    containerBuilder =>
    {
        containerBuilder.RegisterModule(new PresentationModule());
        containerBuilder.RegisterModule(new ApplicationModule());
    });

builder.Services.AddSingleton<IEmployeesParser, EmployeesParser>();

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
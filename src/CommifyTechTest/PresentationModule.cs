using Autofac;
using CommifyTechTest.Jobs;
using CommifyTechTest.Services;
using MediatR;
using Quartz;
using Quartz.Impl;

namespace CommifyTechTest;

public class PresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(context =>
        {
            var mediator = context.Resolve<IMediator>();

            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.JobFactory = new AddEmployeeJobFactory(mediator);
            scheduler.Start().GetAwaiter().GetResult();

            return scheduler;
        })
        .As<IScheduler>()
        .SingleInstance();

        builder.RegisterType<EmployeesParser>().As<IEmployeesParser>();
    }
}

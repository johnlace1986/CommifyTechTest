using Autofac;
using CommifyTechTest.Jobs;
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
            scheduler.JobFactory = new AddEmployeesJobFactory(mediator);
            scheduler.Start().GetAwaiter().GetResult();

            return scheduler;
        })
        .As<IScheduler>()
        .SingleInstance();
    }
}

using Autofac;
using CommifyTechTest.Application.CommandHandlers;

namespace CommifyTechTest.Application;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<AddEmployeeCommandHandler>().AsImplementedInterfaces();
    }
}

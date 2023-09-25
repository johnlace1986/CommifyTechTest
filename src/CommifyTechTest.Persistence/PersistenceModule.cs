using Autofac;
using CommifyTechTest.Application.Infrastructure.Repositories;
using CommifyTechTest.Persistence.Repositories;

namespace CommifyTechTest.Persistence;

public class PersistenceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
    }
}

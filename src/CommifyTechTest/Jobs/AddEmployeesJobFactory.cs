using MediatR;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace CommifyTechTest.Jobs;

public class AddEmployeesJobFactory : PropertySettingJobFactory
{
    private readonly IMediator _mediator;

    public AddEmployeesJobFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return new AddEmployeesJob(_mediator);
    }
}

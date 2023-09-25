using MediatR;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace CommifyTechTest.Jobs;

public class AddEmployeeJobFactory : PropertySettingJobFactory
{
    private readonly IMediator _mediator;

    public AddEmployeeJobFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return new AddEmployeeJob(_mediator);
    }
}

using CommifyTechTest.Application.Commands;
using MediatR;

namespace CommifyTechTest.Application.CommandHandlers;

internal class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand>
{
    public Task Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

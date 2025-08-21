using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class DeleteEmployeeCommand : IRequest<int>
    {
        public string Id { get; set; }
    }
}
namespace EmployeeManagementSolu.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
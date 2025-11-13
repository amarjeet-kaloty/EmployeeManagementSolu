namespace Domain.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Validates if a department exists in the Department Solution.
        /// </summary>
        Task ValidateDepartmentExistsAsync(Guid departmentId, CancellationToken cancellationToken);
    }
}

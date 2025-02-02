using PANDA.Repository.Model;

namespace PANDA.Repository.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department> AddAsync(Department Department, CancellationToken cancellationToken);
        Task<Department> GetDepartmentAsync(int departmentId, CancellationToken cancellationToken);
        Task<Department> GetDepartmentAsync(string departmentCode, CancellationToken cancellationToken);
        Task<bool> IsExistingDepartment(int departmentId, CancellationToken cancellationToken);
        Task<bool> IsExistingDepartment(string departmentCode, CancellationToken cancellationToken);
    }
}
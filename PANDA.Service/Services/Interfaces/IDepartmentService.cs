using PANDA.ClientModel.Model.Department;
using PANDA.Repository.Model;

namespace PANDA.Service.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest createDepartmentRequest, CancellationToken cancellationToken);
        Task<Department> GetDepartmentAsync(string departmentCode, CancellationToken cancellationToken);
        Task<Department> GetDepartmentAsync(int departmentId, CancellationToken cancellationToken);
        Task<GetDepartmentResponse> GetDepartmentResponseByDepartmentCodeAsync(string departmentCode, CancellationToken cancellationToken);
    }
}
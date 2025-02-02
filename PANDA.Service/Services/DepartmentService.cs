using PANDA.ClientModel.Model.Department;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;
using PANDA.Service.Exceptions;
using PANDA.Service.Services.Interfaces;

namespace PANDA.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<GetDepartmentResponse> GetDepartmentResponseByDepartmentCodeAsync(string departmentCode, CancellationToken cancellationToken)
        {
            Department department = await GetDepartmentAsync(departmentCode, cancellationToken);

            return new GetDepartmentResponse
            {
                Code = department.Code,
                Name = department.Name,
                Id = department.Id
            };
        }

        public async Task<Department> GetDepartmentAsync(string departmentCode, CancellationToken cancellationToken)
        {
            await ThrowIfDepartmentDoesNotExist(departmentCode, cancellationToken);

            Department department = await _departmentRepository.GetDepartmentAsync(departmentCode, cancellationToken);
            return department;
        }

        public async Task<Department> GetDepartmentAsync(int departmentId, CancellationToken cancellationToken)
        {
            await ThrowIfDepartmentDoesNotExist(departmentId, cancellationToken);

            Department department = await _departmentRepository.GetDepartmentAsync(departmentId, cancellationToken);
            return department;
        }

        public async Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest createDepartmentRequest, CancellationToken cancellationToken)
        {
            await ThrowIfDepartmentExists(createDepartmentRequest.Code, cancellationToken);

            Department department = new Department()
            {
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Code = createDepartmentRequest.Code,
                Name = createDepartmentRequest.Name
            };

            await _departmentRepository.AddAsync(department, cancellationToken);

            return new CreateDepartmentResponse
            {
                Code = department.Code,
                Name = department.Name,
                Id = department.Id
            };
        }

        private async Task ThrowIfDepartmentDoesNotExist(string departmentCode, CancellationToken cancellationToken)
        {
            if (!await _departmentRepository.IsExistingDepartment(departmentCode, cancellationToken))
            {
                throw new HandledException($"Department code {departmentCode} does not exist", 404);
            }
        }

        private async Task ThrowIfDepartmentDoesNotExist(int departmentId, CancellationToken cancellationToken)
        {
            if (!await _departmentRepository.IsExistingDepartment(departmentId, cancellationToken))
            {
                throw new HandledException($"Department Id {departmentId} does not exist", 404);
            }
        }

        private async Task ThrowIfDepartmentExists(string departmentCode, CancellationToken cancellationToken)
        {
            if (await _departmentRepository.IsExistingDepartment(departmentCode, cancellationToken))
            {
                throw new HandledException($"Department code {departmentCode} already exist", 400);
            }
        }
    }
}

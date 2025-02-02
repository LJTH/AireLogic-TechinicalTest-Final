using PANDA.ClientModel.Model.Clinician;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;
using PANDA.Service.Exceptions;
using PANDA.Service.Services.Interfaces;

namespace PANDA.Service.Services
{
    public class ClinicianService : IClinicianService
    {
        private readonly IClinicianRepository _clinicianRepository;
        private readonly IDepartmentService _departmentService;

        public ClinicianService(IClinicianRepository clinicianRepository, IDepartmentService departmentService)
        {
            _clinicianRepository = clinicianRepository;
            _departmentService = departmentService;
        }

        public async Task<GetClinicianResponse> GetClinicianByGmcCode(string clinicianGmcCode, CancellationToken cancellationToken)
        {
            await ThrowIfClinicianDoesNotExist(clinicianGmcCode, cancellationToken);

            Clinician clinician = await _clinicianRepository.GetByGmcNumberAsync(clinicianGmcCode, cancellationToken);

            return new GetClinicianResponse
            {
                Id = clinician.Id,
                GmcCode = clinician.GmcCode,
                Surname = clinician.Surname,
                Forename = clinician.Forename
            };
        }

        private async Task ThrowIfClinicianDoesNotExist(string clinicianGmcCode, CancellationToken cancellationToken)
        {
            if (!await _clinicianRepository.IsExistingClinician(clinicianGmcCode, cancellationToken))
            {
                throw new HandledException($"Clinician code {clinicianGmcCode} does not exist", 404);
            }
        }

        public async Task<CreateClinicianResponse> CreateClinician(CreateClinicianRequest createClinicianRequest, CancellationToken cancellationToken)
        {
            if (await _clinicianRepository.IsExistingClinician(createClinicianRequest.GmcCode, cancellationToken))
            {
                throw new HandledException($"Clinician code {createClinicianRequest.GmcCode} already exist", 400);
            }

            Department department = await _departmentService.GetDepartmentAsync(createClinicianRequest.DepartmentId, cancellationToken);

            Clinician clinician = new Clinician()
            {
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Department = department,
                Forename = createClinicianRequest.Forename,
                GmcCode = createClinicianRequest.GmcCode,
                Surname = createClinicianRequest.Surname
            };

            await _clinicianRepository.AddAsync(clinician, cancellationToken);

            return new CreateClinicianResponse
            {
                Forename = clinician.Forename,
                Surname = clinician.Surname,
                GmcCode = clinician.GmcCode,
                DepartmentCode = department.Code,
                DepartmentName = department.Name
            };
        }
    }
}

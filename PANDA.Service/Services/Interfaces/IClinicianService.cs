using PANDA.ClientModel.Model.Clinician;

namespace PANDA.Service.Services.Interfaces
{
    public interface IClinicianService
    {
        Task<CreateClinicianResponse> CreateClinician(CreateClinicianRequest createClinicianRequest, CancellationToken cancellationToken);
        Task<GetClinicianResponse> GetClinicianByGmcCode(string clinicianGmcCode, CancellationToken cancellationToken);
    }
}
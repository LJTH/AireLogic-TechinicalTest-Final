using PANDA.ClientModel.Model.Patient;

namespace PANDA.Service.Services.Interfaces
{
    public interface IPatientService
    {
        Task<CreatePatientResponse> CreatePatient(CreatePatientRequest createPatientRequest, CancellationToken cancellationToken);
        Task DeletePatient(int patientId, CancellationToken cancellationToken);
        Task<GetPatientResponse> GetPatient(string patientIdentifier, CancellationToken cancellationToken);
        Task<UpdatePatientResponse> UpdatePatient(int patientId, UpdatePatientRequest updatePatientRequest, CancellationToken cancellationToken);
    }
}
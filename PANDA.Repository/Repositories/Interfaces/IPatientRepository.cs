using PANDA.Repository.Model;

namespace PANDA.Repository.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken);
        Task DeletePatientAsync(int patientId, CancellationToken cancellationToken);
        Task<Patient> GetPatientAsync(int id, CancellationToken cancellationToken);
        Task<Patient> GetPatientAsync(string patientIdentifier, CancellationToken cancellationToken);
        Task<bool> IsDeletedPatientAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsDeletedPatientAsync(string patientIdentifier, CancellationToken cancellationToken);
        Task<bool> IsExistingPatientAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsExistingPatientAsync(string patientIdentifier, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
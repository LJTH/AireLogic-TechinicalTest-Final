using PANDA.Repository.Model;

namespace PANDA.Repository.Repositories.Interfaces
{
    public interface IClinicianRepository
    {
        Task<Clinician> AddAsync(Clinician clinician, CancellationToken cancellationToken);
        Task<Clinician> GetByGmcNumberAsync(string clinicianGmcNumber, CancellationToken cancellationToken);
        Task<bool> IsExistingClinician(string clinicianGmcCode, CancellationToken cancellationToken);
    }
}
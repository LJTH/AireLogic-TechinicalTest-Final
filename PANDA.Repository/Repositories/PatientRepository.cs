using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Context;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;

namespace PANDA.Repository.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PandaDbContext pandaDbContext;

        public PatientRepository(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task<Patient> GetPatientAsync(string patientIdentifier, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients
                .Include(p => p.Address)
                .SingleAsync(c => c.LocalIdentifier == patientIdentifier, cancellationToken);
        }
        public async Task<Patient> GetPatientAsync(int id, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> IsExistingPatientAsync(string patientIdentifier, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients.AnyAsync(p =>
            p.LocalIdentifier == patientIdentifier
            && !p.DeletedDateTime.HasValue
            , cancellationToken);
        }

        public async Task<bool> IsDeletedPatientAsync(string patientIdentifier, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients.AnyAsync(p =>
            p.LocalIdentifier == patientIdentifier
            && p.DeletedDateTime.HasValue
            , cancellationToken);
        }

        public async Task<bool> IsDeletedPatientAsync(int id, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients.AnyAsync(p =>
            p.Id == id
            && p.DeletedDateTime.HasValue
            , cancellationToken);
        }

        public async Task<bool> IsExistingPatientAsync(int id, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Patients
                .AnyAsync(p =>
                p.Id == id
                && !p.DeletedDateTime.HasValue
                , cancellationToken);
        }

        public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken)
        {
            pandaDbContext.Patients.Add(patient);
            await pandaDbContext.SaveChangesAsync(cancellationToken);
            return patient;
        }

        public async Task DeletePatientAsync(int patientId, CancellationToken cancellationToken)
        {
            var patient = await GetPatientAsync(patientId, cancellationToken);
            patient.DeletedDateTime = DateTime.UtcNow;
            await pandaDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await pandaDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

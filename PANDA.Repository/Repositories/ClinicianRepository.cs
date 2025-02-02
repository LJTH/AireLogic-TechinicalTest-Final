using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Context;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;

namespace PANDA.Repository.Repositories
{
    public class ClinicianRepository : IClinicianRepository
    {
        private readonly PandaDbContext _pandaDbContext;

        public ClinicianRepository(PandaDbContext pandaDbContext)
        {
            this._pandaDbContext = pandaDbContext;
        }

        public async Task<Clinician> GetByGmcNumberAsync(string clinicianGmcNumber, CancellationToken cancellationToken)
        {
            return await _pandaDbContext.Clinicians.SingleAsync(c => c.GmcCode == clinicianGmcNumber, cancellationToken);
        }

        public async Task<Clinician> AddAsync(Clinician clinician, CancellationToken cancellationToken)
        {
            _pandaDbContext.Clinicians.Add(clinician);
            await _pandaDbContext.SaveChangesAsync(cancellationToken);
            return clinician;
        }

        public async Task<bool> IsExistingClinician(string clinicianGmcCode, CancellationToken cancellationToken)
        {
            return await _pandaDbContext.Clinicians.AnyAsync(c => c.GmcCode == clinicianGmcCode, cancellationToken);
        }
    }
}

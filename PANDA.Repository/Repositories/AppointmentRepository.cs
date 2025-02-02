using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Context;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;

namespace PANDA.Repository.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PandaDbContext pandaDbContext;

        public AppointmentRepository(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task<IList<Appointment>> GetPatientAppointmentsAsync
            (int patientId, CancellationToken cancellationToken)
        {
            return await pandaDbContext
                .Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinician)
                .Where(a => a.Patient.Id == patientId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AppointmentExists(int id, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Appointments.AnyAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Appointment> GetAppointmentAsync(int id, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Appointments.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> AppointmentClashesWithExistingAppointmentAsync(int patientId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken)
        {
            return await pandaDbContext
                .Appointments
                .AnyAsync(c => c.Patient.Id == patientId
                && startDateTime < c.EndDateTime
                && endDateTime > c.StartDateTime, cancellationToken);
        }

        public async Task<Appointment> AddAsync(Appointment Appointment, CancellationToken cancellationToken)
        {
            pandaDbContext.Appointments.Add(Appointment);
            await pandaDbContext.SaveChangesAsync(cancellationToken);
            return Appointment;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await pandaDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

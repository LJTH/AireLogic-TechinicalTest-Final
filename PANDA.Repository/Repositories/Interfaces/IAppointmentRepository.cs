using PANDA.Repository.Model;

namespace PANDA.Repository.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment Appointment, CancellationToken cancellationToken);
        Task<bool> AppointmentClashesWithExistingAppointmentAsync(int patientId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken);
        Task<bool> AppointmentExists(int id, CancellationToken cancellationToken);
        Task<Appointment> GetAppointmentAsync(int id, CancellationToken cancellationToken);
        Task<IList<Appointment>> GetPatientAppointmentsAsync(int patientId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
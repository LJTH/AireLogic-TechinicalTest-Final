using PANDA.ClientModel.Model.Appointment;
using PANDA.ClientModel.Model.Clinician;

namespace PANDA.Service.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task CancelAppointment(int id, CancellationToken cancellationToken);
        Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest createAppointmentRequest, CancellationToken cancellationToken);
        Task<GetAppointmentResponse> GetAppointment(int id, CancellationToken cancellationToken);
        Task<IEnumerable<PatientAppointment>> GetPatientAppointmentsAsync(int patientId, CancellationToken cancellationToken);
        Task<UpdateAppointmentResponse> UpdateAppointment(int id, UpdateAppointmentRequest updateAppointmentRequest, CancellationToken cancellationToken);
    }
}
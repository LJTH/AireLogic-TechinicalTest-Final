using PANDA.ClientModel.Model.Appointment;
using PANDA.ClientModel.Model.Clinician;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;
using PANDA.Service.Exceptions;
using PANDA.Service.Services.Interfaces;

namespace PANDA.Service.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<GetAppointmentResponse> GetAppointment(int id, CancellationToken cancellationToken)
        {
            Appointment appointment = await GetAppointmentAsync(id, cancellationToken);

            return new GetAppointmentResponse
            {
                Id = appointment.Id,
                StartDateTime = appointment.StartDateTime,
                EndDateTime = appointment.EndDateTime,
                AttendanceDateTime = appointment.AttendanceDateTime,
                ClinicianId = appointment.Clinician.Id,
                PatientId = appointment.Patient.Id
            };
        }

        public async Task<UpdateAppointmentResponse> UpdateAppointment(int id, UpdateAppointmentRequest updateAppointmentRequest, CancellationToken cancellationToken)
        {
            Appointment appointment = await GetAppointmentAsync(id, cancellationToken);

            appointment.ClinicianId = updateAppointmentRequest.ClinicianId;
            appointment.StartDateTime = updateAppointmentRequest.StartDateTime;
            appointment.EndDateTime = updateAppointmentRequest.EndDateTime;

            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return new UpdateAppointmentResponse
            {
                Id = appointment.Id,
                StartDateTime = appointment.StartDateTime,
                EndDateTime = appointment.EndDateTime,
                ClinicianId = appointment.Clinician.Id,
                PatientId = appointment.Patient.Id
            };
        }

        public async Task<IEnumerable<PatientAppointment>> GetPatientAppointmentsAsync(int patientId, CancellationToken cancellationToken)
        {
            IList<Appointment> appointment = await _appointmentRepository.GetPatientAppointmentsAsync(patientId, cancellationToken);

            return appointment.Select(a => new PatientAppointment
            {
                Id = a.Id,
                StartDateTime = a.StartDateTime,
                EndDateTime = a.EndDateTime,
                AttendanceDateTime = a.AttendanceDateTime,
                ClinicianId = a.Clinician.Id,
            });
        }

        public async Task CancelAppointment(int id, CancellationToken cancellationToken)
        {
            Appointment appointment = await GetAppointmentAsync(id, cancellationToken);

            appointment.IsCancelled = true;

            await _appointmentRepository.SaveChangesAsync(cancellationToken);
        }

        internal async Task<Appointment> GetAppointmentAsync(int appointmentId, CancellationToken cancellationToken)
        {
            await ThrowIfAppointmentDoesNotExist(appointmentId, cancellationToken);

            Appointment appointment = await _appointmentRepository.GetAppointmentAsync(appointmentId, cancellationToken);
            return appointment;
        }

        public async Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest createAppointmentRequest, CancellationToken cancellationToken)
        {
            await ThrowIfAppointmentClashes(createAppointmentRequest.PatientId, createAppointmentRequest.StartDateTime, createAppointmentRequest.EndDateTime, cancellationToken);

            Appointment appointment = new Appointment()
            {
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                StartDateTime = createAppointmentRequest.StartDateTime,
                EndDateTime = createAppointmentRequest.EndDateTime,
                PatientId = createAppointmentRequest.PatientId,
                ClinicianId = createAppointmentRequest.ClinicianId
            };

            await _appointmentRepository.AddAsync(appointment, cancellationToken);

            return new CreateAppointmentResponse
            {
                Id = appointment.Id,
                ClinicianId = appointment.ClinicianId,
                PatientId = appointment.PatientId,
                StartDateTime = appointment.StartDateTime,
                EndDateTime = appointment.EndDateTime
            };
        }

        private async Task ThrowIfAppointmentDoesNotExist(int appointmentId, CancellationToken cancellationToken)
        {
            if (!await _appointmentRepository.AppointmentExists(appointmentId, cancellationToken))
            {
                throw new HandledException($"Appointment id {appointmentId} does not exist", 404);
            }
        }

        private async Task ThrowIfAppointmentClashes(int patientId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken)
        {
            if (await _appointmentRepository.AppointmentClashesWithExistingAppointmentAsync(patientId, startDateTime, endDateTime, cancellationToken))
            {
                throw new HandledException($"An appointment already exists during the time of the appointment.", 400);
            }
        }
    }
}

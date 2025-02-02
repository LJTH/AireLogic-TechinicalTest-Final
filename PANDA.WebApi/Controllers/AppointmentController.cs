using Microsoft.AspNetCore.Mvc;
using PANDA.ClientModel.Model.Appointment;
using PANDA.ClientModel.Model.Clinician;
using PANDA.Service.Services.Interfaces;
using PANDA.WebApi.ExceptionHandling;

namespace PANDA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// Retrieves a Appointment.
        /// </summary>
        /// <param name="id">Id of the appointment</param>
        /// <returns>A appointment</returns>
        /// <response code="200">Returns if the appointment exists</response>
        /// <response code="404">Returns if the appointment cannot be found for the "id" provided</response>
        [ProducesResponseType(typeof(GetAppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<GetAppointmentResponse> Get(int id, CancellationToken cancellationToken)
        {
            return await _appointmentService.GetAppointment(id, cancellationToken);
        }

        /// <summary>
        /// Retrieves all appointments for a patient.
        /// </summary>
        /// <param name="patientId">Id of the patient to return all appointments for</param>
        /// <returns>A list of Patient Appointments</returns>
        /// <response code="200">Returns if the appointment exists</response>
        [ProducesResponseType(typeof(IEnumerable<PatientAppointment>), StatusCodes.Status200OK)]
        [HttpGet("Patient/{patientId}")]
        public async Task<IEnumerable<PatientAppointment>> GetPatientAppointments(int patientId, CancellationToken cancellationToken)
        {
            return await _appointmentService.GetPatientAppointmentsAsync(patientId, cancellationToken);
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="appointmentId">Id of the appointment to update</param>
        /// <param name="updateAppointmentRequest">Changes to the appointment</param>
        /// <returns>A appointment</returns>
        /// <response code="200">Returns if the appointment exists and the update was successful</response>
        /// <response code="404">Returns if the appointment cannot be found by the id.</response>
        [ProducesResponseType(typeof(UpdateAppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPatch("{appointmentId}")]
        public async Task<UpdateAppointmentResponse> Update(int appointmentId, UpdateAppointmentRequest updateAppointmentRequest,  CancellationToken cancellationToken)
        {
            return await _appointmentService.UpdateAppointment(appointmentId, updateAppointmentRequest, cancellationToken);
        }

        /// <summary>
        /// Deletes an appointment.
        /// </summary>
        /// <param name="appointmentId">Id of the appointment to delete</param>
        /// <returns>A appointment</returns>
        /// <response code="200">Returns if the appointment exists and the delete was successful</response>
        /// <response code="404">Returns if the appointment cannot be found by the id.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPatch("{appointmentId}/Cancel")]
        public async Task Cancel(int appointmentId, CancellationToken cancellationToken)
        {
            await _appointmentService.CancelAppointment(appointmentId, cancellationToken);
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="createAppointmentRequest">Model for creating a new appointment</param>
        /// <returns>A appointment</returns>
        /// <response code="200">Returns if the appointment exists and the delete was successful</response>
        /// <response code="404">Returns if the appointment cannot be found by the id.</response>
        [ProducesResponseType(typeof(CreateAppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<CreateAppointmentResponse> Add(CreateAppointmentRequest createAppointmentRequest, CancellationToken cancellationToken)
        {
            return await _appointmentService.CreateAppointment(createAppointmentRequest, cancellationToken);
        }
    }
}

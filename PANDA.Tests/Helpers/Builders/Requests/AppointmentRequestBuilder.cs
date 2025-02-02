using PANDA.ClientModel.Model.Appointment;

namespace PANDA.Tests.Helpers.Builders.Requests
{
    public static class AppointmentRequestBuilder
    {
        public static CreateAppointmentRequest BuildCreateAppointmentRequest(DateTime startDateTime, DateTime endDateTime, int patientId, int clinicianId)
        {
            return new CreateAppointmentRequest
            {
               StartDateTime = startDateTime,
               ClinicianId = clinicianId,
               PatientId = patientId,
               EndDateTime = endDateTime
            };
        }

        public static UpdateAppointmentRequest BuildUpdateAppointmentRequest(DateTime startDateTime, DateTime endDateTime, int clinicianId)
        {
            return new UpdateAppointmentRequest
            {
                StartDateTime = startDateTime,
                ClinicianId = clinicianId,
                EndDateTime = endDateTime
            };
        }
    }
}

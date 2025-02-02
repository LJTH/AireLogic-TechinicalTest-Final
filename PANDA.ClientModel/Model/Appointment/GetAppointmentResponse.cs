using System;

namespace PANDA.ClientModel.Model.Clinician
{
    public class GetAppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? AttendanceDateTime { get; set; }

        public bool MissedAppointment
            => DateTime.UtcNow > EndDateTime
                && (!AttendanceDateTime.HasValue
                || AttendanceDateTime.Value > EndDateTime);
        public int ClinicianId { get; set; }
    }
}

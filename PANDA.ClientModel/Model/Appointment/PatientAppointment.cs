using System;

namespace PANDA.ClientModel.Model.Clinician
{
    public class PatientAppointment
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? AttendanceDateTime { get; set; }
        public int ClinicianId { get; set; }
    }
}

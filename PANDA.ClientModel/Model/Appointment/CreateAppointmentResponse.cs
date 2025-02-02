using System;

namespace PANDA.ClientModel.Model.Appointment
{
    public class CreateAppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ClinicianId { get; set; }
    }
}

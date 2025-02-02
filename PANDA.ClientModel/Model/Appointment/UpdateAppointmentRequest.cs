using System;

namespace PANDA.ClientModel.Model.Appointment
{
    public class UpdateAppointmentRequest
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ClinicianId { get; set; }
    }
}

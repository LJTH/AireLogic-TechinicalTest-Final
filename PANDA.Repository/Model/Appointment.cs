using System.ComponentModel.DataAnnotations.Schema;

namespace PANDA.Repository.Model
{
    public class Appointment : EntityModelBase
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? AttendanceDateTime { get; set; }
        
        [ForeignKey("Patient")]
        public int PatientId {  get; set; }
        public Patient Patient { get; set;}
        [ForeignKey("Clinician")]
        public int ClinicianId { get; set; }
        public Clinician Clinician { get; set; }
    }
}

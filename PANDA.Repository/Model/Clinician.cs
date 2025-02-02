namespace PANDA.Repository.Model
{
    public class Clinician : EntityModelBase
    {
        public string GmcCode { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; } = null;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); 

    }
}

namespace PANDA.Repository.Model
{
    public class Patient : EntityModelBase
    {
        public string LocalIdentifier { get; set; }
        public string NhsNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); 

    }
}

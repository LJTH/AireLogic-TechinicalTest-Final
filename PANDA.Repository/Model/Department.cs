namespace PANDA.Repository.Model
{
    public class Department : EntityModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Clinician> Consultants { get; set; }
    }
}

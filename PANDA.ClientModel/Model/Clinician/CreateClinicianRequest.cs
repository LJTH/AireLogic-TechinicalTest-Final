namespace PANDA.ClientModel.Model.Clinician
{
    public class CreateClinicianRequest
    {
        public string GmcCode { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int DepartmentId { get; set; }
    }
}

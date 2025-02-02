using System;

namespace PANDA.ClientModel.Model.Patient
{
    public class GetPatientResponse
    {
        public int Id { get; set; }
        public string LocalIdentifier { get; set; }
        public string NhsNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Postcode { get; set; }
    }
}

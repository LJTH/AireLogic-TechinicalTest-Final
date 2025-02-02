using System;

namespace PANDA.ClientModel.Model.Patient
{
    public class UpdatePatientRequest
    {
        public string NhsNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string PostCode { get; set; }
    }
}

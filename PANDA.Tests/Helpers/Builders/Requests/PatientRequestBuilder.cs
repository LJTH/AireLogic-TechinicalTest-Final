using PANDA.ClientModel.Model.Patient;

namespace PANDA.Tests.Helpers.Builders.Database
{
    public static class PatientRequestBuilder
    {
        public static CreatePatientRequest BuildCreatePatientRequest( 
            string localIdentifier = "P12345678", 
            string nhsNumber = "9434765919",
            string surname = "TEST",
            string forename = "Patient",
            string addressLine1 = "AddressLine1",
            string addressLine2 = "AddressLine2",
            string addressLine3 = "AddressLine3",
            string addressLine4 = "AddressLine4",
            string addressLine5 = "AddressLine5",
            string postcode = "PO48SY",
            DateTime? dateOfBirth = null)
        {
            if (dateOfBirth is null) dateOfBirth = DateTime.Now;

            var patient = new CreatePatientRequest()
            {
                LocalIdentifier = localIdentifier,
                NhsNumber = nhsNumber,
                Surname = surname,
                Forename = forename,
                DateOfBirth = dateOfBirth.Value,
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    AddressLine3 = addressLine3,
                    AddressLine4 = addressLine4,
                    AddressLine5 = addressLine5,
                    PostCode = postcode
                
            };

            return patient;
        }

        public static UpdatePatientRequest BuildUpdatePatientRequest(
            string nhsNumber = "1234567890",
            string surname = "TEST",
            string forename = "Patient",
            string addressLine1 = "AddressLine1",
            string addressLine2 = "AddressLine2",
            string addressLine3 = "AddressLine3",
            string addressLine4 = "AddressLine4",
            string addressLine5 = "AddressLine5",
            string postcode = "PO48SY",
            DateTime? dateOfBirth = null)
        {
            if (dateOfBirth is null) dateOfBirth = DateTime.Now;

            var patient = new UpdatePatientRequest()
            {
                NhsNumber = nhsNumber,
                Surname = surname,
                Forename = forename,
                DateOfBirth = dateOfBirth.Value,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                AddressLine4 = addressLine4,
                AddressLine5 = addressLine5,
                PostCode = postcode

            };

            return patient;
        }
    }
}

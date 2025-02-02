using PANDA.Repository.Context;
using PANDA.Repository.Model;

namespace PANDA.Tests.Helpers.Builders.Database
{
    public static class PatientBuilder
    {
        public static Patient CreatePatient(PandaDbContext dbContext, 
            string localIdentifier = "P12345678", 
            string nhsNumber = "1234567890",
            string surname = "TEST",
            string forename = "Patient",
            string addressLine1 = "AddressLine1",
            string addressLine2 = "AddressLine2",
            string addressLine3 = "AddressLine3",
            string addressLine4 = "AddressLine4",
            string addressLine5 = "AddressLine5",
            string postcode = "PO48SY",
            DateTime? dateOfBirth = null,
            DateTime? deletedDate = null)
        {
            if (dateOfBirth is null) dateOfBirth = DateTime.Now;

            var patient = new Patient()
            {
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
                DeletedDateTime = deletedDate,
                LocalIdentifier = localIdentifier,
                NhsNumber = nhsNumber,
                Surname = surname,
                Forename = forename,
                DateOfBirth = dateOfBirth.Value,
                Address = new Address
                {
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    AddressLine3 = addressLine3,
                    AddressLine4 = addressLine4,
                    AddressLine5 = addressLine5,
                    PostCode = postcode
                }
            };

            dbContext.Patients.Add(patient);
            dbContext.SaveChanges();
            return patient;
        }
    }
}

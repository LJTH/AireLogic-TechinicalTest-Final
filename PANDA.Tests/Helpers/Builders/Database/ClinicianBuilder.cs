using PANDA.Repository.Context;
using PANDA.Repository.Model;

namespace PANDA.Tests.Helpers.Builders.Database
{
    public static class ClinicianBuilder
    {
        public static Clinician CreateClinician(PandaDbContext dbContext, Department department, string consultantGmcCode = "C12345678", string consultantForename = "David", string consultantSurname = "Smith")
        {
            var consultant = new Clinician()
            {
                GmcCode = consultantGmcCode,
                Surname = consultantSurname,
                Forename = consultantSurname,
                Department = department,
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            };
            dbContext.Clinicians.Add(consultant);
            dbContext.SaveChanges();
            return consultant;
        }
    }
}

using PANDA.ClientModel.Model.Clinician;


namespace PANDA.Tests.Helpers.Builders.Requests
{
    public static class ClinicianRequestBuilder
    {
        public static CreateClinicianRequest BuildCreateClinicianRequest(string gmcCode = "12345678", string forename = "David", string surname = "Smith", int departmentId = 0)
        {
            return new CreateClinicianRequest
            {
                DepartmentId = departmentId,
                Forename = forename,
                GmcCode = gmcCode,
                Surname = surname
            };
        }
    }
}

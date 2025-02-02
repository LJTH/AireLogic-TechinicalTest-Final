using PANDA.ClientModel.Model.Department;

namespace PANDA.Tests.Helpers.Builders.Requests
{
    public static class DepartmentRequestBuilder
    {
        public static CreateDepartmentRequest BuildCreateDepartmentRequest(string departmentCode = "ABC", string departmentName = "Test name")
        {
            return new CreateDepartmentRequest
            {
                Code = departmentCode,
                Name = departmentName
            };
        }
    }
}

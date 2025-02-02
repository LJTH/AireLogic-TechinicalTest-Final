using PANDA.Repository.Context;
using PANDA.Repository.Model;

namespace PANDA.Tests.Helpers.Builders.Database
{
    public static class DepartmentBuilder
    {
        public static Department CreateDepartment(PandaDbContext dbContext, string departmentCode = "CARD", string departmentName = "Cardiology")
        {
            var department = new Department()
            {
                Code = departmentCode,
                Name = departmentName,
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            };
            dbContext.Departments.Add(department);
            dbContext.SaveChanges();
            return department;
        }
    }
}

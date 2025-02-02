using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Context;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;

namespace PANDA.Repository.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PandaDbContext pandaDbContext;

        public DepartmentRepository(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task<bool> IsExistingDepartment(string departmentCode, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Departments.AnyAsync(d => d.Code == departmentCode, cancellationToken);
        }
        public async Task<bool> IsExistingDepartment(int departmentId, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Departments.AnyAsync(d => d.Id == departmentId, cancellationToken);
        }
        public async Task<Department> GetDepartmentAsync(string departmentCode, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Departments.SingleAsync(d => d.Code == departmentCode, cancellationToken);
        }

        public async Task<Department> GetDepartmentAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await pandaDbContext.Departments.FindAsync(new object[] { departmentId }, cancellationToken);
        }

        public async Task<Department> AddAsync(Department Department, CancellationToken cancellationToken)
        {
            pandaDbContext.Departments.Add(Department);
            await pandaDbContext.SaveChangesAsync(cancellationToken);
            return Department;
        }
    }
}

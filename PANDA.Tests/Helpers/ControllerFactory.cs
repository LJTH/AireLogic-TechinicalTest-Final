using Microsoft.Extensions.DependencyInjection;
using PANDA.Repository.Context;
using PANDA.Repository.Repositories;
using PANDA.Repository.Repositories.Interfaces;
using PANDA.Service.Services;
using PANDA.Service.Services.Interfaces;

namespace PANDA.Tests.Helpers
{
    public class ControllerFactory
    {
        public ControllerFactory WithFakeSampleDataRepository()
        {
            return this;
        }

        public TController Build<TController>(PandaDbContext dbContext)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(dbContext);

            serviceCollection.AddScoped<IDepartmentRepository, DepartmentRepository>();
            serviceCollection.AddScoped<IClinicianRepository, ClinicianRepository>();
            serviceCollection.AddScoped<IAppointmentRepository, AppointmentRepository>();
            serviceCollection.AddScoped<IPatientRepository, PatientRepository>();
            serviceCollection.AddScoped<IClinicianService, ClinicianService>();
            serviceCollection.AddScoped<IDepartmentService, DepartmentService>();
            serviceCollection.AddScoped<IPatientService, PatientService>();
            serviceCollection.AddScoped<IAppointmentService, AppointmentService>();

            var service = serviceCollection.BuildServiceProvider();

            var sampleController = ActivatorUtilities.CreateInstance<TController>(service);

            return sampleController;
        }
    }
}

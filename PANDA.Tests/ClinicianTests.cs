using PANDA.Service.Exceptions;
using PANDA.Tests.Helpers.Builders.Database;
using PANDA.Tests.Helpers;
using PANDA.WebApi.Controllers;
using PANDA.Tests.Helpers.Builders.Requests;

namespace PANDA.Tests
{
    public class ClinicianTests : TestSetup
    {
        [Test]
        public async Task Given_CreateClinicianRequest_When_ClinicianAlreadyExists_Return400BadRequest()
        {
            //Arrange
            var createClinicianRequest = ClinicianRequestBuilder.BuildCreateClinicianRequest();

            //Create the same clinician in the database with department
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            ClinicianBuilder.CreateClinician(PandaDbContext, department, createClinicianRequest.GmcCode);

            //Act and Assert
            var clinicianController = new ControllerFactory()
                                    .Build<ClinicianController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            Assert.ThrowsAsync<HandledException>(async () => await clinicianController.Add(createClinicianRequest, new CancellationToken()));

        }

        [Test]
        public async Task Given_CreateClinicianRequest_When_ClinicianDoesNotExist_CreateNewClinician()
        {
            //Arrange
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var createClinician = ClinicianRequestBuilder.BuildCreateClinicianRequest(departmentId: department.Id);

            //Act
            var clinicianController = new ControllerFactory()
                                    .Build<ClinicianController>(PandaDbContext);

            var response = await clinicianController.Add(createClinician, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task Given_GetClinicianRequest_When_ClinicianDoesNotExist_Return404NotFound()
        {
            //Arrange
            var clinicianGmcCode = "CARD";

            //Act and Assert
            var clinicianController = new ControllerFactory()
                                    .Build<ClinicianController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            var exception = Assert.ThrowsAsync<HandledException>(async () => await clinicianController.Get(clinicianGmcCode, new CancellationToken()));
            //Assert - Status code 404 returned
            Assert.That(exception.StatusCode == 404);

        }

        [Test]
        public async Task Given_GetClinicianRequest_When_ClinicianNotExist_ReturnTheClinician()
        {
            //Arrange
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var createClinician = ClinicianBuilder.CreateClinician(PandaDbContext, department: department);

            //Act
            var clinicianController = new ControllerFactory()
                                    .Build<ClinicianController>(PandaDbContext);

            var response = await clinicianController.Get(createClinician.GmcCode, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.GmcCode == createClinician.GmcCode);
            Assert.That(response.Surname == createClinician.Surname);
            Assert.That(response.Forename == createClinician.Forename);
            Assert.That(response.Id > 0);
        }
    }
}

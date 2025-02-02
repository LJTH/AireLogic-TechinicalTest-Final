using PANDA.Service.Exceptions;
using PANDA.Tests.Helpers;
using PANDA.Tests.Helpers.Builders.Database;
using PANDA.Tests.Helpers.Builders.Requests;
using PANDA.WebApi.Controllers;

namespace PANDA.Tests
{
    public class DepartmentTests : TestSetup
    {
        [Test]
        public async Task Given_CreateDepartmentRequest_When_DepartmentAlreadyExists_Return400BadRequest()
        {
            //Arrange
            var createDepartmentRequest = DepartmentRequestBuilder.BuildCreateDepartmentRequest();

            //Create the same department in the database
            DepartmentBuilder.CreateDepartment(PandaDbContext, createDepartmentRequest.Code, createDepartmentRequest.Name);

            //Act and Assert
            var departmentController = new ControllerFactory()
                                    .Build<DepartmentController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            Assert.ThrowsAsync<HandledException>(async () => await departmentController.Add(createDepartmentRequest, new CancellationToken()));

        }

        [Test]
        public async Task Given_CreateDepartmentRequest_When_DepartmentDoesNotExist_CreateNewDepartment()
        {
            //Arrange
            var createDepartment = DepartmentRequestBuilder.BuildCreateDepartmentRequest();

            //Act
            var departmentController = new ControllerFactory()
                                    .Build<DepartmentController>(PandaDbContext);

            var response = await departmentController.Add(createDepartment, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task Given_GetDepartmentRequest_When_DepartmentDoesNotExist_Return404NotFound()
        {
            //Arrange
            var departmentCode = "CARD";
            var departmentName = "Cardiology";
            
            //Act and Assert
            var departmentController = new ControllerFactory()
                                    .Build<DepartmentController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            var exception = Assert.ThrowsAsync<HandledException>(async () => await departmentController.Get(departmentCode, new CancellationToken()));

            Assert.That(exception.StatusCode == 404);
            
        }

        [Test]
        public async Task Given_GetDepartmentRequest_When_DepartmentExists_ReturnTheDepartment()
        {
            //Arrange
            var departmentCode = "CARD";
            var departmentName = "Cardiology";

            //Create the same department in the database
            DepartmentBuilder.CreateDepartment(PandaDbContext, departmentCode, departmentName);


            //Act
            var departmentController = new ControllerFactory()
                                    .Build<DepartmentController>(PandaDbContext);

            var response = await departmentController.Get(departmentCode, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Code == departmentCode);
            Assert.That(response.Name == departmentName);
            Assert.That(response.Id > 0);
        }
    }
}

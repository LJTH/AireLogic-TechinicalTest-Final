using PANDA.Service.Exceptions;
using PANDA.Tests.Helpers;
using PANDA.Tests.Helpers.Builders.Database;
using PANDA.WebApi.Controllers;

namespace PANDA.Tests
{
    public class PatientTests : TestSetup
    {
        [Test]
        public async Task Given_CreatePatientRequest_When_PatientAlreadyExists_Return400BadRequest()
        {
            //Arrange - Create the patient in the database
            var patient = PatientBuilder.CreatePatient(PandaDbContext);


            var createPatientRequest = PatientRequestBuilder.BuildCreatePatientRequest(localIdentifier: patient.LocalIdentifier);

            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Act and Assert - should throw HandledException as the patient already exists
            var exception = Assert.ThrowsAsync<HandledException>(async () => await patientController.Add(createPatientRequest, new CancellationToken()));
            Assert.That(exception.StatusCode == 400);

        }

        [Test]
        public async Task Given_CreatePatientRequest_When_PatientIsDeleted_Return400BadRequest()
        {
            //Arrange - Create the patient in the database as deleted
            var patient = PatientBuilder.CreatePatient(PandaDbContext,
                deletedDate: DateTime.UtcNow);

            var createPatientRequest = PatientRequestBuilder.BuildCreatePatientRequest(localIdentifier: patient.LocalIdentifier);
            
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Act and Assert - Should throw Handled Exception as the patient exists but is deleted.
            var exception = Assert.ThrowsAsync<HandledException>(async () => await patientController.Add(createPatientRequest, new CancellationToken()));
            Assert.That(exception.StatusCode == 400);

        }

        [Test]
        public async Task Given_CreatePatientRequest_When_PatientDoesNotExist_CreateNewPatient()
        {
            //Arrange - Build CreatePatientRequest
            var createPatient = PatientRequestBuilder.BuildCreatePatientRequest();

            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Act - Add the patient
            var response = await patientController.Add(createPatient, new CancellationToken());

            //Assert patient was created successfully
            Assert.That(response, Is.Not.Null);
            Assert.That(response.LocalIdentifier == createPatient.LocalIdentifier);
            Assert.That(response.NhsNumber == createPatient.NhsNumber);
            Assert.That(response.DateOfBirth == createPatient.DateOfBirth);
            Assert.That(response.Surname == createPatient.Surname);
            Assert.That(response.Forename == createPatient.Forename);
            Assert.That(response.AddressLine1 == createPatient.AddressLine1);
            Assert.That(response.AddressLine2 == createPatient.AddressLine2);
            Assert.That(response.AddressLine3 == createPatient.AddressLine3);
            Assert.That(response.AddressLine4 == createPatient.AddressLine4);
            Assert.That(response.AddressLine5 == createPatient.AddressLine5);
            Assert.That(response.Postcode == createPatient.PostCode);

        }

        [Test]
        [TestCase("PO48SY","PO4 8SY")]
        [TestCase("po48sy", "PO4 8SY")]
        [TestCase("po4 8sy", "PO4 8SY")]
        [TestCase("PO4 8SY", "PO4 8SY")]
        [TestCase("EC1A1BB", "EC1A 1BB")]
        [TestCase("M11AE", "M1 1AE")]
        public async Task Given_CreatePatientRequest_When_ReturningPatient_EnsurePostcodeIsFormattedCorrectly(string unformattedPostcode, string expectedPostcodeFormat)
        {
            //Arrange - Create the patient with the test case unformattedPostcode
            var createPatient = PatientRequestBuilder.BuildCreatePatientRequest(postcode: unformattedPostcode);

            //Act
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            var response = await patientController.Add(createPatient, new CancellationToken());

            //Assert - FormattedPostcode should be the expectedPostcodeFormat in the TestCase
            Assert.That(response, Is.Not.Null);
           Assert.That(response.FormattedPostcode == expectedPostcodeFormat);

        }

        [Test]
        public async Task Given_CreatePatientRequest_When_NhsNumberFailsValidation_Return400BadRequest()
        {
            //Arrange - Build a CreatePatientRequest with a NHS Number that fails checksum validation
            var createPatient = PatientRequestBuilder.BuildCreatePatientRequest(nhsNumber: "1234567890");
            
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Act and Assert

            var response = Assert.ThrowsAsync<HandledException>(async () => await patientController.Add(createPatient, new CancellationToken()));
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task Given_GetPatientRequest_When_PatientDoesNotExist_Return404NotFound()
        {
            //Arrange - this local identifier wont exist in the database
            var patientLocalIdentifier = "P12345";

            //Act and Assert
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Assert - should throw a HandledException as the patient does not exist
            var exception = Assert.ThrowsAsync<HandledException>(async () => await patientController.Get(patientLocalIdentifier, new CancellationToken()));

            Assert.That(exception.StatusCode == 404);

        }

        [Test]
        public async Task Given_GetPatientRequest_When_PatientExists_ReturnThePatient()
        {
            //Arrange
            var patientIdentifier = "P123456";

            //Create the same patient in the database
            var patient = PatientBuilder.CreatePatient(PandaDbContext, patientIdentifier);
            
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);


            //Act - Get the patient
            var response = await patientController.Get(patientIdentifier, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.LocalIdentifier == patient.LocalIdentifier);
            Assert.That(response.NhsNumber == patient.NhsNumber);
            Assert.That(response.DateOfBirth == patient.DateOfBirth);
            Assert.That(response.Surname == patient.Surname);
            Assert.That(response.Forename == patient.Forename);
            Assert.That(response.AddressLine1 == patient.Address.AddressLine1);
            Assert.That(response.AddressLine2 == patient.Address.AddressLine2);
            Assert.That(response.AddressLine3 == patient.Address.AddressLine3);
            Assert.That(response.AddressLine4 == patient.Address.AddressLine4);
            Assert.That(response.AddressLine5 == patient.Address.AddressLine5);
            Assert.That(response.Postcode == patient.Address.PostCode);
            Assert.That(response.Id > 0);
        }

        [Test]
        public async Task Given_UpdatePatientRequest_When_PatientDoesNotExists_Return404NotFound()
        {
            //Arrange - Build UpdatePatientRequest
            var updatePatientRequest = PatientRequestBuilder.BuildUpdatePatientRequest();

            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Act and Assert - Should throw a HandledException as the patient does not exist bu Id
            var exception = Assert.ThrowsAsync<HandledException>(async () => await patientController.Update(101, updatePatientRequest, new CancellationToken()));

            Assert.That(exception.StatusCode == 404);
        }

        [Test]
        public async Task Given_UpdatePatientRequest_When_PatientExists_UpdateAndReturnThePatient()
        {
            //Arrange
            var patient = PatientBuilder.CreatePatient(PandaDbContext);
            var updatePatientRequest = PatientRequestBuilder.BuildUpdatePatientRequest(
                nhsNumber: "9434765919",
                surname: "NewSurname",
                forename: "NewForename",
                dateOfBirth: DateTime.Now.AddMonths(-10),
                addressLine1: "newAddressLine1",
                addressLine2: "newAddressLine2",
                addressLine3: "newAddressLine3",
                addressLine4: "newAddressLine4",
                addressLine5: "newAddressLine5",
                postcode: "PO22AP");

            //Act
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            var response = await patientController.Update(patient.Id, updatePatientRequest, new CancellationToken());

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.LocalIdentifier == patient.LocalIdentifier);
            Assert.That(response.NhsNumber == updatePatientRequest.NhsNumber);
            Assert.That(response.DateOfBirth == updatePatientRequest.DateOfBirth);
            Assert.That(response.Surname == updatePatientRequest.Surname);
            Assert.That(response.Forename == updatePatientRequest.Forename);
            Assert.That(response.AddressLine1 == updatePatientRequest.AddressLine1);
            Assert.That(response.AddressLine2 == updatePatientRequest.AddressLine2);
            Assert.That(response.AddressLine3 == updatePatientRequest.AddressLine3);
            Assert.That(response.AddressLine4 == updatePatientRequest.AddressLine4);
            Assert.That(response.AddressLine5 == updatePatientRequest.AddressLine5);
            Assert.That(response.Postcode == updatePatientRequest.PostCode);
            Assert.That(response.Id > 0);
        }

        [Test]
        public async Task Given_UpdatePatientRequest_When_NhsNumberIsInvalid_Return400BadRequest()
        {
            //Arrange
            var patient = PatientBuilder.CreatePatient(PandaDbContext);
            var updatePatientRequest = PatientRequestBuilder.BuildUpdatePatientRequest(
                nhsNumber: "1234567890");

            //Act
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            var response = Assert.ThrowsAsync<HandledException>(async () => await patientController.Update(patient.Id, updatePatientRequest, new CancellationToken()));

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task Given_DeletePatientRequest_When_PatientDoesNotExists_Return404NotFound()
        {
            //Arrange
            var patientId = 123;

            //Act
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            var exception = Assert.ThrowsAsync<HandledException>(async () => await patientController.Delete(patientId, new CancellationToken()));

            Assert.That(exception.StatusCode == 404);
        }

        [Test]
        public async Task Given_DeletePatientRequest_When_PatientDoesExist_DeletePatient()
        {
            //Arrange
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            //Act
            var patientController = new ControllerFactory()
                                    .Build<PatientController>(PandaDbContext);

            //Assert - repeat request and ensure Custom Exception is thrown
            Assert.DoesNotThrowAsync(async () => await patientController.Delete(patient.Id, new CancellationToken()));
            Assert.That(patient.DeletedDateTime, Is.Not.Null);
        }
    }
}

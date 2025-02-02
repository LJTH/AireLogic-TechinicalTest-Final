using PANDA.Service.Exceptions;
using PANDA.Tests.Helpers.Builders.Database;
using PANDA.Tests.Helpers.Builders.Requests;
using PANDA.Tests.Helpers;
using PANDA.WebApi.Controllers;

namespace PANDA.Tests
{
    internal class AppointmentTests : TestSetup
    {
        [Test]
        public async Task Given_CreateAppointmentRequest_When_AppointmentAlreadyExists_Return400BadRequest()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-10);
            var endDateTime = DateTime.UtcNow;

            //Add the appointment to the database
            AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id);

            //Build a CreateAppointmentRequest
            var createAppointmentRequest = AppointmentRequestBuilder.BuildCreateAppointmentRequest
                (startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);


            //Act and Assert - Should throw a HandledException as an appointment exists for the patient at the same datetime
            var exception = Assert.ThrowsAsync<HandledException>(async () => await appointmentController.Add(createAppointmentRequest, new CancellationToken()));
            Assert.That(exception.StatusCode == 400);
        }

        [Test]
        public async Task Given_CreateAppointmentRequest_When_AppointmentDoesNotExist_CreateNewAppointment()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-10);
            var endDateTime = DateTime.UtcNow;

            //Build a CreateAppointmentRequest
            var createAppointmentRequest = AppointmentRequestBuilder.BuildCreateAppointmentRequest
                (startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.Add(createAppointmentRequest, new CancellationToken());

            //Assert - Appointment should be created and returned
            Assert.That(response, Is.Not.Null);
            Assert.That(response.PatientId, Is.EqualTo(patient.Id));
            Assert.That(response.ClinicianId, Is.EqualTo(clinician.Id));
            Assert.That(response.StartDateTime, Is.EqualTo(startDateTime));
            Assert.That(response.EndDateTime, Is.EqualTo(endDateTime));

        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentDoesNotExist_Return404NotFound()
        {
            //Arrange
            var appointmentId = 1;

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act and Assert - Should throw a HandledException as the appointment does not exist
            var exception = Assert.ThrowsAsync<HandledException>(async () => await appointmentController.Get(appointmentId, new CancellationToken()));
            Assert.That(exception.StatusCode == 404);

        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentExists_ReturnTheAppointment()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-10);
            var endDateTime = DateTime.UtcNow;

            //Create the appointment in the database
            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act 
            var response = await appointmentController.Get(appointment.Id, new CancellationToken());

            //Assert - Appointment is returned
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id == appointment.Id);
            
        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentEndDateNotPassed_MissedAppointmentShouldBeFalse()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow;
            var endDateTime = DateTime.UtcNow.AddMinutes(30); 

            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.Get(appointment.Id, new CancellationToken());

            //Assert - MissedAppointment should be false
            Assert.That(response, Is.Not.Null);
            Assert.That(response.MissedAppointment, Is.False);

        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentIsAttendedDuringTheAppointment_MissedAppointmentShouldBeFalse()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow;
            var attendanceDateTime = startDateTime.AddMinutes(5);
            var endDateTime = DateTime.UtcNow.AddMinutes(30);

            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id, attendanceDateTime);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.Get(appointment.Id, new CancellationToken());

            //Assert - MissedAppointment should be false
            Assert.That(response, Is.Not.Null);
            Assert.That(response.MissedAppointment, Is.False);

        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentIsAttendedAfterTheAppointmentEndDateTime_MissedAppointmentShouldBeTrue()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-30);
            var endDateTime = DateTime.UtcNow.AddMinutes(-15);
            var attendanceDateTime = endDateTime.AddMinutes(5);

            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id, attendanceDateTime);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.Get(appointment.Id, new CancellationToken());

            //Assert - MissedAppointment should be true
            Assert.That(response, Is.Not.Null);
            Assert.That(response.MissedAppointment, Is.True);

        }

        [Test]
        public async Task Given_GetAppointmentRequest_When_AppointmentHasNotBeenAttendedAndTheEndDateHasPassed_MissedAppointmentShouldBeTrue()
        {
            //Arrange - Create all entities required to create a new appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-30);
            var endDateTime = DateTime.UtcNow.AddMinutes(-15);

            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.Get(appointment.Id, new CancellationToken());

            //Assert - MissedAppointment should be true
            Assert.That(response, Is.Not.Null);
            Assert.That(response.MissedAppointment, Is.True);

        }

        [Test]
        public async Task Given_GetPatientAppointmentsRequest_When_AppointmentExists_ReturnTheAppointments()
        {
            //Arrange - Create all entities required to create multiple appointments
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTimeOne = DateTime.UtcNow.AddMinutes(-10);
            var endDateTimeOne = DateTime.UtcNow;

            var appointmentOne = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTimeOne, endDateTimeOne, patient.Id, clinician.Id);

            var startDateTimeTwo = DateTime.UtcNow.AddMinutes(-10);
            var endDateTimeTwo = DateTime.UtcNow;
            
            var appointmentTwo = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTimeTwo, endDateTimeTwo, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act
            var response = await appointmentController.GetPatientAppointments(patient.Id, new CancellationToken());

            //Assert - there should be exactly 2 appointments returned for the patient
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(2));

        }

        [Test]
        public async Task Given_CancelAppointmentRequest_When_AppointmentExists_CancelTheAppointment()
        {
            //Arrange - Create all entities required to create the appointment
            var department = DepartmentBuilder.CreateDepartment(PandaDbContext);
            var clinician = ClinicianBuilder.CreateClinician(PandaDbContext, department);
            var patient = PatientBuilder.CreatePatient(PandaDbContext);

            var startDateTime = DateTime.UtcNow.AddMinutes(-10);
            var endDateTime = DateTime.UtcNow;

            var appointment = AppointmentBuilder.CreateAppointment(PandaDbContext, startDateTime, endDateTime, patient.Id, clinician.Id);

            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            //Act - Cancel the appointment
            Assert.DoesNotThrowAsync(async () => await appointmentController.Cancel(appointment.Id, new CancellationToken()));

            //Assert - The appointment is cancelled
            Assert.That(appointment.IsCancelled, Is.True);
            

        }

        [Test]
        public async Task Given_CancelAppointmentRequest_When_AppointmentDoesNotExist_CancelTheAppointment()
        {
            //Arrange
            int nonexistentAppointmentId = 99;
            //Act
            var appointmentController = new ControllerFactory()
                                    .Build<AppointmentController>(PandaDbContext);

            var exception = Assert.ThrowsAsync<HandledException>(async () => await appointmentController.Cancel(nonexistentAppointmentId, new CancellationToken()));

            //Assert
            Assert.That(exception.StatusCode == 404);


        }
    }
}

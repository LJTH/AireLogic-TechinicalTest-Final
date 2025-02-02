using PANDA.Repository.Context;
using PANDA.Repository.Model;

namespace PANDA.Tests.Helpers.Builders.Database
{
    public static class AppointmentBuilder
    {
        public static Appointment CreateAppointment(PandaDbContext dbContext, DateTime startDateTime, DateTime endDateTime, int patientId, int clinicianId, DateTime? attendanceDateTime = null)
        {
            var appointment = new Appointment()
            {
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                PatientId = patientId,
                ClinicianId = clinicianId,
                AttendanceDateTime = attendanceDateTime
            };
            dbContext.Appointments.Add(appointment);
            dbContext.SaveChanges();
            return appointment;
        }
    }
}

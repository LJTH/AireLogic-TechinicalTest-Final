using Microsoft.AspNetCore.Mvc;
using PANDA.ClientModel.Model.Patient;
using PANDA.Service.Services.Interfaces;
using PANDA.WebApi.ExceptionHandling;

namespace PANDA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Retrieves a Patient by its local patient identifier
        /// </summary>
        /// <param name="patientLocalIdentifier">Local patient identifier</param>
        /// <returns>A patient</returns>
        /// <response code="200">Returns the patient if it exists by local identifier</response>
        /// <response code="404">Returns if the patient cannot be found for the "local identifier" provided</response>
        [ProducesResponseType(typeof(GetPatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{patientIdentifier}")]
        public async Task<GetPatientResponse> Get(string patientLocalIdentifier, CancellationToken cancellationToken)
        {
            return await _patientService.GetPatient(patientLocalIdentifier, cancellationToken);
        }

        /// <summary>
        /// Updates a patient
        /// </summary>
        /// <param name="patientId">PatientId for the patient</param>
        /// <param name="updatePatientRequest">Model to update the patient with</param>
        /// <returns>An updated patient</returns>
        /// <response code="200">Returns the updated patient if the update was successful</response>
        /// <response code="404">Returns if the patient cannot be found for the patientId provided</response>
        [ProducesResponseType(typeof(GetPatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPatch("{patientId}")]
        public async Task<UpdatePatientResponse> Update(int patientId, UpdatePatientRequest updatePatientRequest,  CancellationToken cancellationToken)
        {
            return await _patientService.UpdatePatient(patientId, updatePatientRequest, cancellationToken);
        }

        /// <summary>
        /// Deletes a patient
        /// </summary>
        /// <param name="patientId">Id for the patient</param>
        /// <returns></returns>
        [HttpDelete("{patientId}")]
        public async Task Delete(int patientId, CancellationToken cancellationToken)
        {
            await _patientService.DeletePatient(patientId, cancellationToken);
        }

        /// <summary>
        /// /// Creates a new Patient. Patient must not already exist.
        /// </summary>
        /// <param name="createPatientRequest">Model for creating a new patient</param>
        /// <returns>A new patient</returns>
        [HttpPost]
        public async Task<CreatePatientResponse> Add(CreatePatientRequest createPatientRequest, CancellationToken cancellationToken)
        {
            return await _patientService.CreatePatient(createPatientRequest, cancellationToken);
        }
    }
}

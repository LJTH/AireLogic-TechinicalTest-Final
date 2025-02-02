using Microsoft.AspNetCore.Mvc;
using PANDA.ClientModel.Model.Clinician;
using PANDA.Service.Services.Interfaces;
using PANDA.WebApi.ExceptionHandling;

namespace PANDA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicianController : ControllerBase
    {
        private readonly IClinicianService _clinicianService;

        public ClinicianController(IClinicianService clinicianService)
        {
            _clinicianService = clinicianService;
        }

        /// <summary>
        /// Retrieves a Clinician by its Clinician Code
        /// </summary>
        /// <param name="clinicianGmcCode">Clinician code</param>
        /// <returns>A clinician</returns>
        /// <response code="200">Returns the clinician if it exists by code</response>
        /// <response code="404">Returns if the clinician cannot be found for the "clinicianCode" provided</response>
        [ProducesResponseType(typeof(GetClinicianResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{clinicianGmcCode}")]
        public async Task<GetClinicianResponse> Get(string clinicianGmcCode, CancellationToken cancellationToken)
        {
            return await _clinicianService.GetClinicianByGmcCode(clinicianGmcCode, cancellationToken);
        }

        /// <summary>
        /// Creates a new Clinician. Clinician must not already exist with the same GMC Code.
        /// </summary>
        /// <param name="createClinicianRequest">Model for creating a new clinician</param>
        /// <returns>A new clinician</returns>
        /// <response code="200">Returns the clinician</response>
        /// <response code="400">Returns if a Clinician already exists with the ClinicianCode provided</response>
        /// <response code="404">Returns if the clinician cannot be found for the "clinicianCode" provided</response>
        [ProducesResponseType(typeof(CreateClinicianResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<CreateClinicianResponse> Add(CreateClinicianRequest createClinicianRequest, CancellationToken cancellationToken)
        {
            return await _clinicianService.CreateClinician(createClinicianRequest, cancellationToken);
        }
    }
}

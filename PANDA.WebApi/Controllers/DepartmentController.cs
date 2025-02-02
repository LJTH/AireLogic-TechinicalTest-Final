using Microsoft.AspNetCore.Mvc;
using PANDA.ClientModel.Model.Department;
using PANDA.Service.Services.Interfaces;
using PANDA.WebApi.ExceptionHandling;

namespace PANDA.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Retrieves a Department by its Department Code
        /// </summary>
        /// <param name="departmentCode">Department code</param>
        /// <returns>A department</returns>
        /// <response code="200">Returns the department if it exists by code</response>
        /// <response code="404">Returns if the department cannot be found for the "departmentCode" provided</response>
        [ProducesResponseType(typeof(GetDepartmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{departmentCode}")]
        public async Task<GetDepartmentResponse> Get(string departmentCode, CancellationToken cancellationToken)
        {
            return await _departmentService.GetDepartmentResponseByDepartmentCodeAsync(departmentCode, cancellationToken);
        }

        /// <summary>
        /// Creates a new Department. Department must not already exist.
        /// </summary>
        /// <param name="createDepartmentRequest">Model for creating a new department</param>
        /// <returns>A new department</returns>
        /// <response code="200">Returns the department</response>
        /// <response code="400">Returns if a Department already exists with the DepartmentCode provided</response>
        /// <response code="404">Returns if the department cannot be found for the "departmentCode" provided</response>
        [ProducesResponseType(typeof(CreateDepartmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<CreateDepartmentResponse> Add(CreateDepartmentRequest createDepartmentRequest, CancellationToken cancellationToken)
        {
            return await _departmentService.CreateDepartment(createDepartmentRequest, cancellationToken);
        }
    }
}

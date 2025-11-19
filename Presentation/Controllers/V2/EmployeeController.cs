using Asp.Versioning;
using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        /// <returns>
        /// A List of Employees or an empty list if no employees are found.
        /// </returns>
        [Authorize(Policy = "SupervisorOrManager")]
        [HttpGet("EmployeesList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<EmployeeBasicDTO>>> GetEmployeeList()
        {
            _logger.LogInformation("Version-2 Controller");
            List<ReadEmployeeDTO> employeeList = await _mediator.Send(new GetEmployeeListQuery());
            List<EmployeeBasicDTO> employeeBasicInfo = _mapper.Map<List<EmployeeBasicDTO>>(employeeList);
            return Ok(employeeBasicInfo);
        }
    }
}

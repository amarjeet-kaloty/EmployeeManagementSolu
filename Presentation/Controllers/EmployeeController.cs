using AutoMapper;
using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSolu.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds an employee to the system.
        /// </summary>
        /// <param name="employeeDto">The data transfer object containing the details of the new employee to be added.</param>
        /// <returns>
        /// The newly created object, including its assigned ID.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<ReadEmployeeDTO>> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
            ReadEmployeeDTO newEmployeeDto = await _mediator.Send(employeeDto);

            if (newEmployeeDto == null)
            {
                return BadRequest("The employee could not be created. Please try again.");
            }

            return Ok(newEmployeeDto);
        }

        /// <summary>
        /// Updates the existing employee in the system.
        /// </summary>
        /// <param name="employeeDto">The data transfer object containing the details of the employee to be updated.</param>
        /// <returns>
        /// The updated object.
        /// </returns>
        [HttpPut]
        public async Task<ActionResult<ReadEmployeeDTO>> UpdateEmployee([FromBody] UpdateEmployeeDTO employeeDto)
        {
            ReadEmployeeDTO updatedEmployee = await _mediator.Send(employeeDto);

            if (updatedEmployee == null)
            {
                return NotFound($"Employee with ID {employeeDto.Id} not found");
            }

            return Ok(updatedEmployee);
        }

        /// <summary>
        /// Deletes an employee in the system.
        /// </summary>
        /// <param name="id">The unique integer identifier of the employee to delete. Must be a positive integer.</param>
        /// <returns>
        /// An integer representing the number of rows affected (typically 1 for successful deletion).
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteEmployee(string id)
        {
            int employeeDeletedCount = await _mediator.Send(new DeleteEmployeeCommand() { Id = id });

            if (employeeDeletedCount == 0)
            {
                return NotFound($"Employee with ID {id} not found for deletion.");
            }

            return Ok(id);
        }

        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        /// <returns>
        /// A List of Employees or an empty list if no employees are found.
        /// </returns>
        [HttpGet("EmployeesList")]
        public async Task<List<ReadEmployeeDTO>> GetEmployeeList()
        {
            List<ReadEmployeeDTO> employeeList = await _mediator.Send(new GetEmployeeListQuery());
            return employeeList;
        }

        /// <summary>
        /// Retrieves a specific employee by their unique identifier.
        /// </summary>
        /// <param name="id">The unique integer identifier of the employee to retrieve. Must be a positive integer.</param>
        /// <returns>
        /// An employee object corresponding to the provided unique identifier, if one exists.
        /// </returns>
        [HttpGet("ById")]
        public async Task<ActionResult<ReadEmployeeDTO>> GetEmployee(string id)
        {
            ReadEmployeeDTO employee = await _mediator.Send(new GetEmployeeByIdQuery() { Id = id });

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }

        /// <summary>
        /// Retrieves a specific employee by their unique email.
        /// </summary>
        /// <param name="email">The unique email identifier of the employee to retrieve.</param>
        /// <returns>
        /// An employeeDTO object corresponding to the provided unique email identifier, if one exists.
        /// </returns>
        [HttpGet("ByEmail")]
        public async Task<ActionResult<ReadEmployeeDTO>> GetEmployeeByEmail(string email)
        {
            ReadEmployeeDTO employee = await _mediator.Send(new GetEmployeeByEmailQuery { Email = email });

            if (employee == null)
            {
                return NotFound($"Employee with the email {email} not found.");
            }

            return Ok(employee);
        }
    }
}
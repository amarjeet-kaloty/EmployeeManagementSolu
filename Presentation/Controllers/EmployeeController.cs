using Application.Exceptions;
using AutoMapper;
using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using FluentValidation;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
            try
            {
                ReadEmployeeDTO newEmployeeDto = await _mediator.Send(employeeDto);
                return Ok(newEmployeeDto);
            }
            catch (AutoMapperMappingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the existing employee in the system.
        /// </summary>
        /// <param name="employeeDto">The data transfer object containing the details of the employee to be updated.</param>
        /// <returns>
        /// The updated object.
        /// </returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> UpdateEmployee([FromBody] UpdateEmployeeDTO employeeDto)
        {
            try
            {
                ReadEmployeeDTO updatedEmployee = await _mediator.Send(employeeDto);
                return Ok(updatedEmployee);
            }
            catch (AutoMapperMappingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an employee in the system.
        /// </summary>
        /// <param name="id">The unique integer identifier of the employee to delete. Must be a positive integer.</param>
        /// <returns>
        /// An integer representing the number of rows affected (typically 1 for successful deletion).
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> DeleteEmployee(string id)
        {
            try
            {
                int employeeDeletedCount = await _mediator.Send(new DeleteEmployeeCommand() { Id = id });
                return Ok(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
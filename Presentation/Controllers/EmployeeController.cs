﻿using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace EmployeeManagementSolu.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
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
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
            ReadEmployeeDTO newEmployeeDto = await _mediator.Send(employeeDto);
            return Ok(newEmployeeDto);
        }

        /// <summary> 
        /// Updates the existing employee in the system.
        /// </summary>
        /// <param name="employeeDto">The data transfer object containing the details of the employee to be updated.</param>
        /// <returns>
        /// The updated object.
        /// </returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> UpdateEmployee([FromBody] UpdateEmployeeDTO employeeDto)
        {
            ReadEmployeeDTO updatedEmployee = await _mediator.Send(employeeDto);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> DeleteEmployee(string id)
        {
            int employeeDeletedCount = await _mediator.Send(new DeleteEmployeeCommand() { Id = id });
            return Ok(employeeDeletedCount);
        }

        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        /// <returns>
        /// A List of Employees or an empty list if no employees are found.
        /// </returns>
        [HttpGet("EmployeesList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ReadEmployeeDTO>>> GetEmployeeList()
        {
            List<ReadEmployeeDTO> employeeList = await _mediator.Send(new GetEmployeeListQuery());
            return Ok(employeeList);
        }

        /// <summary>
        /// Retrieves a specific employee by their unique identifier.
        /// </summary>
        /// <param name="id">The unique integer identifier of the employee to retrieve. Must be a positive integer.</param>
        /// <returns>
        /// An employee object corresponding to the provided unique identifier, if one exists.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> GetEmployee(string id)
        {
            ReadEmployeeDTO employee = await _mediator.Send(new GetEmployeeByIdQuery() { Id = id });
            return Ok(employee);
        }

        /// <summary>
        /// Retrieves a specific employee by their unique email.
        /// </summary>
        /// <param name="email">The unique email identifier of the employee to retrieve.</param>
        /// <returns>
        /// An employeeDTO object corresponding to the provided unique email identifier, if one exists.
        /// </returns>
        [HttpGet("ByEmail/{email}")]
        [ValidateEmail]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReadEmployeeDTO>> GetEmployeeByEmail(string email)
        {
            ReadEmployeeDTO employee = await _mediator.Send(new GetEmployeeByEmailQuery { Email = email });
            return Ok(employee);
        }
    }
}
using Application.Exceptions;
using AutoMapper;
using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Presentation.Controllers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EmployeeManagementSolu.Presentation.Tests
{
    public class EmployeeControllerTests
    {
        private readonly IMediator _mediator;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new EmployeeController(_mediator);
        }

        #region Create Employee
        [Fact]
        public async Task AddEmployee_ValidEmployee_ReturnsCreated()
        {
            // Arrange
            CreateEmployeeDTO employeeDto = new CreateEmployeeDTO
            {
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            ReadEmployeeDTO newEmployeeDTO = new ReadEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = employeeDto.Name,
                Address = employeeDto.Address,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone
            };

            _mediator.Send(Arg.Any<CreateEmployeeDTO>()).Returns(newEmployeeDTO);

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<CreateEmployeeDTO>(cmd =>
             cmd.Name == employeeDto.Name &&
             cmd.Address == employeeDto.Address &&
             cmd.Email == employeeDto.Email &&
             cmd.Phone == employeeDto.Phone));

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployee = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(newEmployeeDTO.Id, actualEmployee.Id);
            Assert.Equal(newEmployeeDTO.Name, actualEmployee.Name);
            Assert.Equal(newEmployeeDTO.Address, actualEmployee.Address);
            Assert.Equal(newEmployeeDTO.Email, actualEmployee.Email);
            Assert.Equal(newEmployeeDTO.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task AddEmployee_ValidationException_ReturnsBadRequest()
        {
            // Arrange
            CreateEmployeeDTO employeeDto = new CreateEmployeeDTO
            {
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<CreateEmployeeDTO>()).ThrowsAsync(new ValidationException("Simulated validation error"));

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<CreateEmployeeDTO>(cmd =>
             cmd.Name == employeeDto.Name &&
             cmd.Address == employeeDto.Address &&
             cmd.Email == employeeDto.Email &&
             cmd.Phone == employeeDto.Phone));
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Simulated validation error", badRequestResult.Value);
        }
        #endregion Create Employee

        #region Update Employee
        [Fact]
        public async Task UpdateEmployee_ValidEmployee_ReturnsOkWithUpdatedId()
        {
            // Arrange
            UpdateEmployeeDTO updateEmployeeDTO = new UpdateEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Update Name",
                Address = "Updated Address",
                Email = "updated@gmail.com",
                Phone = "404-111-1234"
            };

            ReadEmployeeDTO expectedEmployeeDTO = new ReadEmployeeDTO
            {
                Id = updateEmployeeDTO.Id,
                Name = updateEmployeeDTO.Name,
                Address = updateEmployeeDTO.Address,
                Email = updateEmployeeDTO.Email,
                Phone = updateEmployeeDTO.Phone
            };

            _mediator.Send(Arg.Any<UpdateEmployeeDTO>()).Returns(expectedEmployeeDTO);

            // Act
            var result = await _controller.UpdateEmployee(updateEmployeeDTO);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<UpdateEmployeeDTO>(cmd =>
                cmd.Id == updateEmployeeDTO.Id &&
                cmd.Name == updateEmployeeDTO.Name &&
                cmd.Address == updateEmployeeDTO.Address &&
                cmd.Email == updateEmployeeDTO.Email &&
                cmd.Phone == updateEmployeeDTO.Phone));

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            ReadEmployeeDTO actualUpdatedEmployeeDTO = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(expectedEmployeeDTO.Id, actualUpdatedEmployeeDTO.Id);
            Assert.Equal(expectedEmployeeDTO.Name, actualUpdatedEmployeeDTO.Name);
            Assert.Equal(expectedEmployeeDTO.Address, actualUpdatedEmployeeDTO.Address);
            Assert.Equal(expectedEmployeeDTO.Email, actualUpdatedEmployeeDTO.Email);
            Assert.Equal(expectedEmployeeDTO.Phone, actualUpdatedEmployeeDTO.Phone);
        }

        [Fact]
        public async Task UpdateEmployee_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            UpdateEmployeeDTO updateEmployeeDTO = new UpdateEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Update Name",
                Address = "Update Address",
                Email = "updated@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<UpdateEmployeeDTO>()).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _controller.UpdateEmployee(updateEmployeeDTO);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<UpdateEmployeeDTO>(cmd =>
                cmd.Id == updateEmployeeDTO.Id &&
                cmd.Name == updateEmployeeDTO.Name &&
                cmd.Address == updateEmployeeDTO.Address &&
                cmd.Email == updateEmployeeDTO.Email &&
                cmd.Phone == updateEmployeeDTO.Phone));
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Simulated error", badRequestResult.Value);
        }
        #endregion Update Employee

        #region Delete Employee
        [Fact]
        public async Task DeleteEmployee_ValidId_ReturnsOkWithDeletedId()
        {
            // Arrange
            string employeeIdToDelete = ObjectId.GenerateNewId().ToString();

            _mediator.Send(Arg.Any<DeleteEmployeeCommand>()).Returns(1);

            // Act
            var result = await _controller.DeleteEmployee(employeeIdToDelete);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<DeleteEmployeeCommand>(cmd => cmd.Id == employeeIdToDelete));
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDeletedId = Assert.IsType<string>(okResult.Value);
            Assert.Equal(employeeIdToDelete, actualDeletedId);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ThrowsEmployeeNotFoundException_ReturnsNotFoundResult()
        {
            // Arrange
            string nonExistentId = ObjectId.GenerateNewId().ToString();

            _mediator.Send(Arg.Any<DeleteEmployeeCommand>()).ThrowsAsync(new NotFoundException($"Employee with ID {nonExistentId} not found."));

            // Act
            var result = await _controller.DeleteEmployee(nonExistentId);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<DeleteEmployeeCommand>(cmd => cmd.Id == nonExistentId));
            var badRequestResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with ID {nonExistentId} not found.", badRequestResult.Value);
        }
        #endregion Delete Employee

        #region GetEmployeeList
        [Fact]
        public async Task GetEmployeeList_ReturnListOfAllEmployees_SuccessAsync()
        {
            // Arrange
            ReadEmployeeDTO employeeDTO1 = new ReadEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            ReadEmployeeDTO employeeDTO2 = new ReadEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Employee2",
                Address = "456 Orange Lane",
                Email = "employee2@gmail.com",
                Phone = "505-000-7896"
            };

            List<ReadEmployeeDTO> expectedEmployees = new List<ReadEmployeeDTO>
            {
                employeeDTO1, employeeDTO2
            };

            _mediator.Send(Arg.Any<GetEmployeeListQuery>()).Returns(expectedEmployees);

            // Act
            var result = await _controller.GetEmployeeList();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeListQuery>());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployeesList = Assert.IsType<List<ReadEmployeeDTO>>(okResult.Value);
            Assert.Equal(expectedEmployees.Count, actualEmployeesList.Count);
            for (int i = 0; i < actualEmployeesList.Count; i++)
            {
                Assert.Equal(expectedEmployees[i].Id, actualEmployeesList[i].Id);
                Assert.Equal(expectedEmployees[i].Name, actualEmployeesList[i].Name);
                Assert.Equal(expectedEmployees[i].Address, actualEmployeesList[i].Address);
                Assert.Equal(expectedEmployees[i].Email, actualEmployeesList[i].Email);
                Assert.Equal(expectedEmployees[i].Phone, actualEmployeesList[i].Phone);
            }
        }

        [Fact]
        public async Task GetEmployeeList_ReturnsEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetEmployeeListQuery>()).Returns(new List<ReadEmployeeDTO>());

            // Act
            var result = await _controller.GetEmployeeList();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeListQuery>());
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployeesList = Assert.IsType<List<ReadEmployeeDTO>>(okResult.Value);
            Assert.Empty(actualEmployeesList);
        }
        #endregion GetEmployeeList

        #region GetEmployeeById
        [Fact]
        public async Task GetEmployeeById_ValidId_ReturnsEmployee()
        {
            // Arrange
            string employeeID = ObjectId.GenerateNewId().ToString();
            ReadEmployeeDTO expectedEmployee = new ReadEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Employee1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<GetEmployeeByIdQuery>()).Returns(expectedEmployee);

            // Act
            var result = await _controller.GetEmployee(employeeID);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByIdQuery>());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            ReadEmployeeDTO actualEmployee = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(expectedEmployee.Id, actualEmployee.Id);
            Assert.Equal(expectedEmployee.Name, actualEmployee.Name);
            Assert.Equal(expectedEmployee.Address, actualEmployee.Address);
            Assert.Equal(expectedEmployee.Email, actualEmployee.Email);
            Assert.Equal(expectedEmployee.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task GetEmployeeById_InValidIdThrowsException_ReturnsBadRequest()
        {
            // Arrange
            string nonExistentId = ObjectId.GenerateNewId().ToString();

            _mediator.Send(Arg.Any<GetEmployeeByIdQuery>()).ThrowsAsync(new AutoMapperMappingException("Simulated automapping error."));

            // Act
            var result = await _controller.GetEmployee(nonExistentId);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByIdQuery>());
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal($"Simulated automapping error.", badRequestResult.Value);
        }
        #endregion GetEmployeeById

        #region GetEmployeeByEmail
        [Fact]
        public async Task GetEmployeeByEmail_EmailExists_ReturnsEmployee()
        {
            // Arrange
            string employeeID = ObjectId.GenerateNewId().ToString();
            ReadEmployeeDTO expectedEmployee = new ReadEmployeeDTO
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Employee1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<GetEmployeeByEmailQuery>()).Returns(expectedEmployee);

            // Act
            var result = await _controller.GetEmployeeByEmail(expectedEmployee.Email);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByEmailQuery>());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            ReadEmployeeDTO actualEmployee = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(expectedEmployee.Id, actualEmployee.Id);
            Assert.Equal(expectedEmployee.Name, actualEmployee.Name);
            Assert.Equal(expectedEmployee.Address, actualEmployee.Address);
            Assert.Equal(expectedEmployee.Email, actualEmployee.Email);
            Assert.Equal(expectedEmployee.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task GetEmployeeByIEmail_ThrowsNotFoundException_ReturnsNotFoundResult()
        {
            // Arrange
            string nonExistentEmail = "emailnotexist@gmail.com";

            _mediator.Send(Arg.Any<GetEmployeeByEmailQuery>()).ThrowsAsync(new NotFoundException($"Employee with email {nonExistentEmail} not found."));

            // Act
            var result = await _controller.GetEmployeeByEmail(nonExistentEmail);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByEmailQuery>());
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with email {nonExistentEmail} not found.", notFoundResult.Value);
        }
        #endregion GetEmployeeByEmail
    }
}
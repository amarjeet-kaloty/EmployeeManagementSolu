using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Presentation.Controllers;
using EmployeeManagementSolu.Presentation.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NSubstitute;

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

        [Fact]
        public async Task GetEmployeeList_ReturnListOfAllEmployees_SuccessAsync()
        {
            // Arrange
            var expectedEmployees = new List<Employee>
            {
                Employee.Create(
                    name: "Test Employee1",
                    address: "123 Praline Ave",
                    email: "employee1@gmail.com",
                    phone: "404-111-1234"
                ),

                Employee.Create(
                    name: "Test Employee2",
                    address: "456 Orange Lane",
                    email: "employee2@gmail.com",
                    phone: "505-000-7896"
                )
            };

            _mediator.Send(Arg.Any<GetEmployeeListQuery>()).Returns(expectedEmployees);

            // Act
            var result = await _controller.GetEmployeeList();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeListQuery>());

            Assert.NotNull(result);
            Assert.Equal(expectedEmployees.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedEmployees[i].Id, result[i].Id);
                Assert.Equal(expectedEmployees[i].Name, result[i].Name);
                Assert.Equal(expectedEmployees[i].Address, result[i].Address);
                Assert.Equal(expectedEmployees[i].Email, result[i].Email);
                Assert.Equal(expectedEmployees[i].Phone, result[i].Phone);
            }
        }

        [Fact]
        public async Task GetEmployeeList_ReturnsEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetEmployeeListQuery>()).Returns(new List<Employee>());

            // Act
            var result = await _controller.GetEmployeeList();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeListQuery>());
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEmployeeById_ValidId_ReturnsEmployee()
        {
            // Arrange
            Employee expectedEmployee = new Employee(
                id: ObjectId.GenerateNewId().ToString(),
                name: new string("Test Employee1"),
                address: "123 Praline Ave",
                email: "employee1@gmail.com",
                phone: "404-111-1234"
            );

            _mediator.Send(Arg.Any<GetEmployeeByIdQuery>()).Returns(expectedEmployee);

            // Act
            var result = await _controller.GetEmployee(expectedEmployee.Id);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByIdQuery>());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            EmployeeDTO actualEmployee = Assert.IsType<EmployeeDTO>(okResult.Value);
            Assert.Equal(expectedEmployee.Id, actualEmployee.Id);
            Assert.Equal(expectedEmployee.Name, actualEmployee.Name);
            Assert.Equal(expectedEmployee.Address, actualEmployee.Address);
            Assert.Equal(expectedEmployee.Email, actualEmployee.Email);
            Assert.Equal(expectedEmployee.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task GetEmployeeById_InValidId_FailsToReturnEmployee()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetEmployeeByIdQuery>()).Returns(Task.FromResult<Employee>(null));

            var nonExistentId = ObjectId.GenerateNewId().ToString();

            // Act
            var result = await _controller.GetEmployee(nonExistentId);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByIdQuery>());
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with ID {nonExistentId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddEmployee_ValidEmployee_ReturnsCreated()
        {
            // Arrange
            EmployeeRequestDTO employeeDto = new EmployeeRequestDTO
            {
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            Employee newEmployee = new Employee
            (
                id: ObjectId.GenerateNewId().ToString(),
                name: employeeDto.Name,
                address: employeeDto.Address,
                email: employeeDto.Email,
                phone: employeeDto.Phone
            );

            _mediator.Send(Arg.Is<CreateEmployeeCommand>(cmd =>
                cmd.Name == employeeDto.Name &&
                cmd.Address == employeeDto.Address &&
                cmd.Email == employeeDto.Email))
            .Returns(newEmployee);

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<CreateEmployeeCommand>(cmd =>
             cmd.Name == employeeDto.Name &&
             cmd.Address == employeeDto.Address &&
             cmd.Email == employeeDto.Email));

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployee = Assert.IsType<EmployeeDTO>(okResult.Value);

            Assert.Equal(newEmployee.Id, actualEmployee.Id);
            Assert.Equal(newEmployee.Name, actualEmployee.Name);
            Assert.Equal(newEmployee.Address, actualEmployee.Address);
            Assert.Equal(newEmployee.Email, actualEmployee.Email);
            Assert.Equal(newEmployee.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task AddEmployee_MediatorReturnsNull_ReturnsInternalServerError()
        {
            // Arrange
            EmployeeRequestDTO employeeDto = new EmployeeRequestDTO
            {
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            Employee validEmployee = new Employee
            (
                id: ObjectId.GenerateNewId().ToString(),
                name: new string(employeeDto.Name),
                address: employeeDto.Address,
                email: employeeDto.Email,
                phone: employeeDto.Phone
            );

            _mediator.Send(Arg.Any<CreateEmployeeCommand>()).Returns(Task.FromResult<Employee>(null));

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<CreateEmployeeCommand>());
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Failed to create employee. An unexpected error occurred.", statusCodeResult.Value);
        }

        [Fact]
        public async Task UpdateEmployee_ValidEmployee_ReturnsOkWithUpdatedId()
        {
            // Arrange
            string employeeIdToUpdate = ObjectId.GenerateNewId().ToString();
            EmployeeRequestDTO updateEmployeeDTO = new EmployeeRequestDTO
            { 
                Name = "Update Name",
                Address = "Updated Address",
                Email = "updated@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<UpdateEmployeeCommand>()).Returns(1);

            // Act
            var result = await _controller.UpdateEmployee(employeeIdToUpdate, updateEmployeeDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualUpdatedId = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, actualUpdatedId);
        }

        [Fact]
        public async Task UpdateEmployee_EmployeeNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            string employeeIdToUpdate = ObjectId.GenerateNewId().ToString();
            EmployeeRequestDTO updateEmployeeDTO = new EmployeeRequestDTO
            {
                Name = "Update Name",
                Address = "Updated Address",
                Email = null,
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<UpdateEmployeeCommand>()).Returns(Task.FromResult(0));

            // Act
            var result = await _controller.UpdateEmployee(employeeIdToUpdate, updateEmployeeDTO);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<UpdateEmployeeCommand>(cmd => cmd.Id == employeeIdToUpdate));
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with ID {employeeIdToUpdate} not found.", notFoundResult.Value);
        }

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
        public async Task DeleteEmployee_EmployeeNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            string nonExistentId = ObjectId.GenerateNewId().ToString();

            _mediator.Send(Arg.Any<DeleteEmployeeCommand>()).Returns(Task.FromResult(0));

            // Act
            var result = await _controller.DeleteEmployee(nonExistentId);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<DeleteEmployeeCommand>(cmd => cmd.Id == nonExistentId));
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal($"Employee with ID {nonExistentId} not found for deletion.", notFoundResult.Value);
        }
    }
}

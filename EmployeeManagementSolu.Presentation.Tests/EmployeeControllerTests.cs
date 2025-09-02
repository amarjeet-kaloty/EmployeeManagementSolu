using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Presentation.Controllers;
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

            _mediator.Send(Arg.Is<CreateEmployeeCommand>(cmd =>
                cmd.Name == employeeDto.Name &&
                cmd.Address == employeeDto.Address &&
                cmd.Email == employeeDto.Email &&
                cmd.Phone == employeeDto.Phone))
            .Returns(newEmployeeDTO);

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<CreateEmployeeCommand>(cmd =>
             cmd.Name == employeeDto.Name &&
             cmd.Address == employeeDto.Address &&
             cmd.Email == employeeDto.Email));

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployee = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(newEmployeeDTO.Id, actualEmployee.Id);
            Assert.Equal(newEmployeeDTO.Name, actualEmployee.Name);
            Assert.Equal(newEmployeeDTO.Address, actualEmployee.Address);
            Assert.Equal(newEmployeeDTO.Email, actualEmployee.Email);
            Assert.Equal(newEmployeeDTO.Phone, actualEmployee.Phone);
        }

        [Fact]
        public async Task AddEmployee_MediatorReturnsNull_ReturnsInternalServerError()
        {
            // Arrange
            CreateEmployeeDTO employeeDto = new CreateEmployeeDTO
            {
                Name = "Test Employee 1",
                Address = "123 Praline Ave",
                Email = "employee1@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<CreateEmployeeCommand>()).Returns(Task.FromResult<ReadEmployeeDTO>(null!));

            // Act
            var result = await _controller.AddEmployee(employeeDto);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<CreateEmployeeCommand>());
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Failed to create employee. An unexpected error occurred.", statusCodeResult.Value);
        }
        #endregion Create Employee

        #region Update Employee
        [Fact]
        public async Task UpdateEmployee_ValidEmployee_ReturnsOkWithUpdatedId()
        {
            // Arrange
            string employeeIdToUpdate = ObjectId.GenerateNewId().ToString();
            EmployeeDTO updateEmployeeDTO = new EmployeeDTO
            {
                Name = "Update Name",
                Address = "Updated Address",
                Email = "updated@gmail.com",
                Phone = "404-111-1234"
            };

            ReadEmployeeDTO expectedEmployeeDTO = new ReadEmployeeDTO
            {
                Id = employeeIdToUpdate,
                Name = updateEmployeeDTO.Name,
                Address = updateEmployeeDTO.Address,
                Email = updateEmployeeDTO.Email,
                Phone = updateEmployeeDTO.Phone
            };

            _mediator.Send(Arg.Any<UpdateEmployeeCommand>()).Returns(expectedEmployeeDTO);

            // Act
            var result = await _controller.UpdateEmployee(employeeIdToUpdate, updateEmployeeDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            ReadEmployeeDTO actualUpdatedEmployeeDTO = Assert.IsType<ReadEmployeeDTO>(okResult.Value);
            Assert.Equal(expectedEmployeeDTO.Id, actualUpdatedEmployeeDTO.Id);
            Assert.Equal(expectedEmployeeDTO.Name, actualUpdatedEmployeeDTO.Name);
            Assert.Equal(expectedEmployeeDTO.Address, actualUpdatedEmployeeDTO.Address);
            Assert.Equal(expectedEmployeeDTO.Email, actualUpdatedEmployeeDTO.Email);
            Assert.Equal(expectedEmployeeDTO.Phone, actualUpdatedEmployeeDTO.Phone);
        }

        [Fact]
        public async Task UpdateEmployee_EmployeeNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            string employeeIdToUpdate = ObjectId.GenerateNewId().ToString();
            EmployeeDTO updateEmployeeDTO = new EmployeeDTO
            {
                Name = "Update Name",
                Address = "Update Address",
                Email = "updated@gmail.com",
                Phone = "404-111-1234"
            };

            _mediator.Send(Arg.Any<UpdateEmployeeCommand>()).Returns(Task.FromResult<ReadEmployeeDTO>(null!));

            // Act
            var result = await _controller.UpdateEmployee(employeeIdToUpdate, updateEmployeeDTO);

            // Assert
            await _mediator.Received(1).Send(Arg.Is<UpdateEmployeeCommand>(cmd => cmd.Id == employeeIdToUpdate));
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with ID {employeeIdToUpdate} not found", notFoundResult.Value);
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
            _mediator.Send(Arg.Any<GetEmployeeListQuery>()).Returns(new List<ReadEmployeeDTO>());

            // Act
            var result = await _controller.GetEmployeeList();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeListQuery>());
            Assert.NotNull(result);
            Assert.Empty(result);
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
        public async Task GetEmployeeById_InValidId_FailsToReturnEmployee()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetEmployeeByIdQuery>()).Returns(Task.FromResult<ReadEmployeeDTO>(null!));

            string nonExistentId = ObjectId.GenerateNewId().ToString();

            // Act
            var result = await _controller.GetEmployee(nonExistentId);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByIdQuery>());
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with ID {nonExistentId} not found.", notFoundResult.Value);
        }
        #endregion GetEmployeeById

        #region GetEmployeeByEmail
        [Fact]
        public async Task GetEmployeeByEmail_EmailExists_ReturnsEmployee()
        {
            // Arrange
            string employeeID = ObjectId.GenerateNewId().ToString();
            EmployeeSearchDTO expectedEmployee = new EmployeeSearchDTO
            {
                EmployeeId = ObjectId.GenerateNewId().ToString(),
                FullName = "Test Employee1",
                ContactEmail = "employee1@gmail.com"
            };

            _mediator.Send(Arg.Any<GetEmployeeByEmailQuery>()).Returns(expectedEmployee);

            // Act
            var result = await _controller.GetEmployeeByEmail(expectedEmployee.ContactEmail);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByEmailQuery>());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            EmployeeSearchDTO actualEmployee = Assert.IsType<EmployeeSearchDTO>(okResult.Value);
            Assert.Equal(expectedEmployee.EmployeeId, actualEmployee.EmployeeId);
            Assert.Equal(expectedEmployee.FullName, actualEmployee.FullName);
            Assert.Equal(expectedEmployee.ContactEmail, actualEmployee.ContactEmail);
        }

        [Fact]
        public async Task GetEmployeeByIEmail_InvalidEmail_FailsToReturnEmployee()
        {
            // Arrange
            string nonExistentEmail = "This mail does not exits.";

            _mediator.Send(Arg.Any<GetEmployeeByEmailQuery>()).Returns(Task.FromResult<EmployeeSearchDTO>(null!));

            // Act
            var result = await _controller.GetEmployeeByEmail(nonExistentEmail);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<GetEmployeeByEmailQuery>());
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Employee with the email {nonExistentEmail} not found.", notFoundResult.Value);
        }
        #endregion GetEmployeeByEmail
    }
}
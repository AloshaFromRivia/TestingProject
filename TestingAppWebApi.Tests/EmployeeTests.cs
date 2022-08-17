using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestingApp.WebApi.Controllers;
using TestingApp.WebApi.Entities;
using Xunit;

namespace TestingAppWebApi.Tests
{
    public class EmployeeTests
    {
        private List<Employee> GetTestEmployees()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = "Thomas",
                    Age = 30,
                },
                new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = "Anna",
                    Age = 22,
                },
                new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = "David",
                    Age = 34,
                }
            };
        }


        [Fact]
        public void EmployeeController_GetAllEmployee_ShouldReturn3()
        {
            //arrange
            var mock = new Mock<IRepository<Employee>>();
            mock.Setup(e=>e.Items)
                .Returns(GetTestEmployees);
            var controller = new EmployeeController(mock.Object);
            //act
            var result = controller.Get();
            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Equal(3,returnValue.Count());
        }

        [Fact]
        public void EmployeeController_AddEmployee_ShouldReturnEmployee()
        {
            //arrange
            var mock = new Mock<IRepository<Employee>>();
            mock.Setup(e=>e.Items)
                .Returns(new List<Employee>());
            var controller = new EmployeeController(mock.Object);
            var id = Guid.NewGuid();
            var employee = new Employee() { Id = id };
            //act
            var result = controller.Post(employee);
            //assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Employee>(createdAtActionResult.Value);
            Assert.Equal(employee,returnValue);
        }

        [Fact]
        public void EmployeeController_AddExistingEmployee_ShouldReturnBadRequest()
        {
            //arrange
            var mock = new Mock<IRepository<Employee>>();
            var employeeToAdd = GetTestEmployees().First();
            mock.Setup(e => e.GetById(employeeToAdd.Id))
                .Returns(employeeToAdd);
            var controller = new EmployeeController(mock.Object);
            
            //act
            var result = controller.Post(employeeToAdd);
            //assert
            var badRequest = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EmployeeController_ReturnsBadRequest_GivenInvalidModel()
        {
            //arrange
            var mock = new Mock<IRepository<Employee>>();
            var controller = new EmployeeController(mock.Object);
            controller.ModelState.AddModelError("Error","Some error");
            //act
            var result = controller.Post(null);
            //assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
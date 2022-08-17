using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestingApp.WebApi.Entities;

namespace TestingApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        
        public EmployeeController(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _repository.Items.ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid guid)
        {
            var employee = _repository.GetById(guid);
            if (employee is null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Post(Employee ep)
        {
            if (ModelState.IsValid)
            {
                var employee = _repository.GetById(ep.Id);

                if (employee is null)
                {
                    _repository.Add(ep);
                    return CreatedAtAction("Get",new {id= ep.Id},ep);
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var employee = _repository.GetById(id);
            if (employee is null)
            {
                return BadRequest();
            }
            _repository.Remove(id);

            return Ok(employee);
        }
    }
}
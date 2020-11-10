using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardAndBarber.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        CustomerRepository _repo;
        public CustomersController(CustomerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateCustomer(Customer customer)
        {
            _repo.Add(customer);

            return Created($"/ api / customers /{ customer.Id}", customer);
        }

        [HttpGet]
        public IActionResult GetAllCustomers() 
        {
            var allCustomers = _repo.GetAll();

            return Ok(allCustomers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _repo.GetById(id);

            if (customer == null) return NotFound("No Customer with that ID was found");

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            var updatedCustomer = _repo.Update(id, customer);

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {

            if (_repo.GetById(id) == null)
            {
                NotFound();
            }

            _repo.Remove(id);

            return Ok();
        }
    }
}

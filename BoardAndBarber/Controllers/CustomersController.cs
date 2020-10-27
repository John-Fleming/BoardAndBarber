using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardAndBarber.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            var repo = new CustomerRepository();
            repo.Add(customer);

            return Created($"/ api / customers /{ customer.Id}", customer);
        }

        [HttpGet]
        public IActionResult GetAllCustomers() 
        {
            var repo = new CustomerRepository();
            var allCustomers = repo.GetAll();

            return Ok(allCustomers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var repo = new CustomerRepository();
            var customer = repo.GetById(id);

            if (customer == null) return NotFound("No Customer with that ID was found");

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            var repo = new CustomerRepository();
            var updatedCustomer = repo.Update(id, customer);

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {

            var repo = new CustomerRepository();
            if (repo.GetById(id) == null)
            {
                NotFound();
            }

            repo.Remove(id);

            return Ok();
        }
    }
}

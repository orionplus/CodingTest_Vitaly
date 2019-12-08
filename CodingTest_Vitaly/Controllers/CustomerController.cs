using Microsoft.AspNetCore.Mvc;

namespace CodingTest_Vitaly
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Route("customers")]
        [HttpGet]
        public CustomerViewModel Get()
        {
            return _customerService.GetCustomers();
        }

        [Route("create")]
        [HttpGet]
        public CustomerViewModel Create()
        {
            return _customerService.CreateCustomer();
        }

        [Route("edit/{id}")]
        [HttpGet]
        public CustomerViewModel Edit(int id)
        {
            var vm = _customerService.GetCustomers(id);
            return vm;
        }
        
        // POST: api/Customer
        [Route("search")]
        [HttpPost]
        public CustomerViewModel Post([FromBody] CustomerSearch searchModel)
        {
            return _customerService.SearchForCustomers(searchModel);
        }

        [Route("save")]
        [HttpPost]
        public CustomerViewModel Save([FromBody] CustomerViewModel model)
        {
            return _customerService.UpdateCustomer(model);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public CustomerViewModel Delete(int id)
        {
            return _customerService.DeleteCustomer(id);
        }
    }
}

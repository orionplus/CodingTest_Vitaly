using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTest_Vitaly
{
    public interface ICustomerService
    {
        CustomerViewModel GetCustomers(int customerId = 0);
        CustomerViewModel CreateCustomer();
        CustomerViewModel UpdateCustomer(CustomerViewModel model);
        CustomerViewModel SearchForCustomers(CustomerSearch searchModel);
        CustomerViewModel DeleteCustomer(int customerId);
    }
}

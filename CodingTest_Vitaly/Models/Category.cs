using System.Collections.Generic;
namespace CodingTest_Vitaly
{
    public class Category
    {
       public Category()
        {
            this.Customers = new HashSet<Customer>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}

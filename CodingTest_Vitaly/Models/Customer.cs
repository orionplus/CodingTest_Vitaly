using System;
using System.ComponentModel.DataAnnotations;

namespace CodingTest_Vitaly
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

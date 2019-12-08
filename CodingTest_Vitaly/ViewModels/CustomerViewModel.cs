using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTest_Vitaly
{
    public class CustomerViewModel
    {
        #region Public Properties
        public List<Customer> Customers { get; set; }
        public List<Category> Categories { get; set; }
        public CustomerSearch SearchEntity { get; set; }
        public List<Category> SearchCategories { get; set; }
        public Customer Customer { get; set; }

        public string EventCommand { get; set; }
        public string EventArgument { get; set; }
        public bool IsValid { get; set; }
        public string PageMode { get; set; }
        public bool IsDetailAreaVisible { get; set; }
        public bool IsListAreaVisible { get; set; }
        public bool IsSearchAreaVisible { get; set; }
        //public ModelStateDictionary Messages { get; set; }
        #endregion

    }
}

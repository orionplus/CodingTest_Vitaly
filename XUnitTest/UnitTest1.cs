using System;
using Xunit;
using CodingTest_Vitaly;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void GetAllTest()
        {
            var options = new DbContextOptionsBuilder<CodingTestDbContext>()
                .UseInMemoryDatabase(databaseName: "CodingTest")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new CodingTestDbContext(options))
            {
                context.Customers.Add(new Customer { CustomerId = 1, FirstName = "John", LastName = "Deacon", Birth = DateTime.Now });
                context.Customers.Add(new Customer { CustomerId = 2, FirstName = "Brian", LastName = "May", Birth = DateTime.Now });
                context.Customers.Add(new Customer { CustomerId = 3, FirstName = "Roger", LastName = "Taylor", Birth = DateTime.Now });
                context.SaveChanges();
            }

            using (var context = new CodingTestDbContext(options))
            {
                CustomerService customerService = new CustomerService(context);
                var model = customerService.GetCustomers();
    
            Assert.Equal(3, model.Customers.Count);
            }
        }

        [Theory, InlineData("ohn", "John", "Deacon")]
        public void GetBySearch(string searchString, string expectedFirstName, string expectedLastName)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CodingTestDbContext>()
                    .UseInMemoryDatabase(databaseName: "CodingTest")
                    .Options;

            using (var context = new CodingTestDbContext(options))
            {
                context.Customers.Add(new Customer { CustomerId = 1, FirstName = "John", LastName = "Deacon", Birth = DateTime.Now });
                context.Customers.Add(new Customer { CustomerId = 2, FirstName = "Brian", LastName = "May", Birth = DateTime.Now });
                context.Customers.Add(new Customer { CustomerId = 3, FirstName = "Roger", LastName = "Taylor", Birth = DateTime.Now });
                context.SaveChanges();
            }

            // Act & Assert
            using (var context = new CodingTestDbContext(options))
            {
                CustomerService customerService = new CustomerService(context);
                List<Customer> matchedCustomers = customerService.SearchForCustomers( new CustomerSearch() { CustomerName = searchString }).Customers;
                // Assert
                Assert.NotNull(matchedCustomers);
                Assert.True(matchedCustomers.All(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString)));
            }
        }
    }
}

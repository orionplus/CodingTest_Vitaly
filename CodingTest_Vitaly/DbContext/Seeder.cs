using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTest_Vitaly
{
    public class Seeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CodingTestDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CodingTestDbContext>>()))
            {
                // Look for any Categories.
                if (context.Categories.Any())
                {
                    return;
                }

                context.Categories.AddRange(
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Government"
                    },
                    new Category
                    {
                        CategoryId = 2,
                        CategoryName = "Corporate Customer"
                    },
                      new Category
                      {
                          CategoryId = 3,
                          CategoryName = "Individual"
                      });

                context.SaveChanges();
            }
        }
    }
}

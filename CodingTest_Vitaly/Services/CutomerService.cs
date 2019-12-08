using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CodingTest_Vitaly
{
    public class CustomerService : ICustomerService
    {
        private CodingTestDbContext _dbContext;
        public CustomerService(CodingTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerViewModel SearchForCustomers(CustomerSearch searchModel)
        {
            var model = InitialiseModel();
            // Perform Search
            model.Customers = _dbContext.Customers.Where(p =>
                  (searchModel.CategoryId == 0 ? true :
                       p.Category.CategoryId == searchModel.CategoryId) &&
                  (string.IsNullOrEmpty(searchModel.CustomerName) ? true :
                       p.FirstName.Contains(searchModel.CustomerName) ||
                        p.LastName.Contains(searchModel.CustomerName)
                       )).
                  OrderBy(p => p.CustomerId).ToList();

            SetUIState(model, Constants.LIST);

            return model;
        }
        public CustomerViewModel CreateCustomer()
        {
            var model = InitialiseModel();
            model.Customer.Birth = DateTime.Now;
            SetUIState(model, Constants.ADD);
            return model;
        }
        public CustomerViewModel UpdateCustomer(CustomerViewModel model)
        {
            //model.Messages.Clear();

            // Ensure the correct category is set
            model.Customer.Category = _dbContext.Categories.Find(model.Customer.Category?.CategoryId);

            try
            {
                // Either Update or Insert product
                if (model.PageMode == Constants.EDIT)
                {
                    _dbContext.Entry(model.Customer).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                else if (model.PageMode == Constants.ADD)
                {
                    _dbContext.Customers.Add(model.Customer);
                    _dbContext.SaveChanges();
                }

                // Get all the data again in case anything changed
                model.Customers = _dbContext.Customers.OrderBy(p => p.CategoryId).ToList();
                model.SearchCategories = _dbContext.Categories.OrderBy(p => p.CategoryName).ToList();

                // Add category for 'Search All'
                Category category = new Category();
                category.CategoryId = 0;
                category.CategoryName = "-- Search All Categories --";
                model.SearchCategories.Insert(0, category);
                model.SearchEntity = new CustomerSearch();
                model.IsValid = true;
                SetUIState(model, Constants.LIST);
            }
            catch (ValidationException ex)
            {
                model.IsValid = false;
                // Validation errors
                foreach (KeyValuePair<string, string> item in ex.Data)
                {
                    //foreach (var item in errors.ValidationErrors)
                    {
                        //model.Messages.AddModelError(item.Key, item.Value);
                    }
                }

                // Set page state
                SetUIState(model, model.PageMode);
            }
            catch (Exception ex)
            {
            }
            return model;
        }

        public CustomerViewModel DeleteCustomer(int customerId)
        {
            Customer customer = _dbContext.Customers.Find(customerId);
            _dbContext.Customers.Remove(customer);
            _dbContext.SaveChanges();
            return GetCustomers();
        }

        public CustomerViewModel GetCustomers(int customerId = 0)
        {
            var model = InitialiseModel();
            if (customerId > 0)
            {
                model.Customer = _dbContext.Customers.Find(customerId);
                SetUIState(model, Constants.EDIT);
            }
            else
            {
                model.Customers = _dbContext.Customers.OrderBy(p => p.CustomerId).ToList();
                SetUIState(model, Constants.LIST);
            }
            return model;
        }

        private CustomerViewModel InitialiseModel()
        {
            var model = new CustomerViewModel();
            model.Customers = new List<Customer>();
            model.Customer = new Customer();
            model.SearchEntity = new CustomerSearch();
            model.SearchCategories = new List<Category>();

            model.EventCommand = string.Empty;
            model.EventArgument = string.Empty;
            model.IsValid = true;
            model.IsDetailAreaVisible = false;
            model.IsListAreaVisible = true;
            model.IsSearchAreaVisible = true;
            model.PageMode = Constants.LIST;
            // model.Messages = new ModelStateDictionary();

            model.Categories = _dbContext.Categories.OrderBy(p => p.CategoryName).ToList();

            if (model.Categories.Count == 0)
            {
                model.SearchCategories.AddRange(_dbContext.Categories);
            }
            else
            {
                model.SearchCategories.AddRange(model.Categories);
            }

            // Add category for 'Search All'

            model.SearchCategories.Insert(0,
                new Category() { CategoryId = 0, CategoryName = "-- Search All Categories --" });

            return model;
        }

        private void SetUIState(CustomerViewModel model, string state)
        {
            model.PageMode = state;
            model.IsDetailAreaVisible = (model.PageMode == "Add" || model.PageMode == "Edit");
            model.IsListAreaVisible = (model.PageMode == "List");
            model.IsSearchAreaVisible = (model.PageMode == "List");
        }
    }
}

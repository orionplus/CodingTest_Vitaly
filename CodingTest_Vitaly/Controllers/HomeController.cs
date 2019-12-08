using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingTest_Vitaly.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CodingTest_Vitaly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            CustomerViewModel model = new CustomerViewModel();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50069/");
                HttpResponseMessage res = await client.GetAsync("api/customer/customers");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<CustomerViewModel>(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CustomerViewModel vm)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50069/");
                HttpResponseMessage response = new HttpResponseMessage();
                switch (vm.EventCommand.ToLower())
                {
                    case "":
                    case "search":
                        response = await client.PostAsJsonAsync("api/customer/search", vm.SearchEntity);
                        vm = JsonConvert.DeserializeObject<CustomerViewModel>(response.Content.ReadAsStringAsync().Result);
                        break;

                    case "cancel":
                    case "resetsearch":
                        vm = await GetData("customers");
                        break;

                    case "add":
                        vm = await GetData("create");
                        break;

                    case "edit":
                        vm = await GetData("edit", vm.EventArgument);
                        break;

                    case "save":
                        vm = await PostData("save", vm);
                        break;

                    case "delete":
                        vm = await DeleteData(vm.EventArgument);
                        break;

                    default:
                        break;
                }

                // If everything is OK, update the model state on the page
                if (vm.IsValid)
                {
                    ModelState.Clear();
                }
            }
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Private Methods
        private async Task<CustomerViewModel> DeleteData(string customerId)
        {
            CustomerViewModel vm = new CustomerViewModel();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50069/");
                var response = await client.DeleteAsync($"api/customer/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    vm = JsonConvert.DeserializeObject<CustomerViewModel>(response.Content.ReadAsStringAsync().Result);
                }
            }
            return vm;
        }

        private async Task<CustomerViewModel> PostData(string uri, CustomerViewModel vm)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50069/");
                var response = await client.PostAsJsonAsync($"api/customer/{uri}", vm);
                if (response.IsSuccessStatusCode)
                {
                    vm = JsonConvert.DeserializeObject<CustomerViewModel>(response.Content.ReadAsStringAsync().Result);
                }
            }
            return vm;
        }

        private async Task<CustomerViewModel> GetData(string uri, string customerId = "")
        {
            CustomerViewModel res = new CustomerViewModel();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50069/");
                //HttpResponseMessage response = new HttpResponseMessage();

                var response = await client.GetAsync($"api/customer/{uri}/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    res = JsonConvert.DeserializeObject<CustomerViewModel>(result);
                }
            }
            return res;
        }
        #endregion
    }
}

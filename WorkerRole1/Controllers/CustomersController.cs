using System;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using WorkerRole1.Database;
using System.Collections.Generic;
using WorkerRole1.Models;
using Newtonsoft.Json;

namespace WorkerRole1.Controllers
{
   /* This is the targeted resource when you make an API call to:
    *  /customers
    *  /customers/'name'
    *  /customers/new
    */  
 
    public class CustomersController : ApiController
    {
        // an instance of the DBHandler to use its methods
        DBHandler dBHandler = new DBHandler();

        /*
         *Http get method that returns a list of customers in a json format 
         *The list of customers comes from the method call to GetCustomers method in the DBHandler class
         */
        public HttpResponseMessage Get()
        {
            List<Customer> customers = dBHandler.GetCustomers();
            var json = JsonConvert.SerializeObject(customers);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        /*
         *HttpGet method that returns a list of specific customers in a json format 
         *it takes a string parameter which is the name of the specific customers that are targeted
         *The list of specific customers comes from the method call to GetSpecCustomers method in the DBHandler class
         */
        public HttpResponseMessage Get(String name)
        {
            List<Customer> customers = dBHandler.GetSpecCustomers(name);
            var json = JsonConvert.SerializeObject(customers);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        /*
         * HttpPost method to add a customer, with a defined new route, different from the default route(in the Startup.cs)
         * it takes a customer object parameter which is sent by the client that consumes the API
         * if the entered customer's email doesn't exist in the db,
         *          then it will be added to the db and a string message is sent to inform the client that a customer is added.
         * if the entered customer's email exist in the db,
         *          then it won't be added and a string message is sent to inform the client that the customer cannot be added.
         */
        [HttpPost]
        [Route("~/Customers/new")]
        public HttpResponseMessage AddCustomer(Customer customer)
        {
          
           if (dBHandler.AddCustomer(customer))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "A new customer is added");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Failed to add a new customer");
            }
        }
    }
}

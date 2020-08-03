using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkerRole1.Models;

namespace WorkerRole1.Database
{
    /*
     * DBHandler class to handle db communication between the cloudstorage and the api
     * it is responsible for table creation and data operations such as getting and inserting data to the cloud table.
     */ 
    public class DBHandler
    {
        private CloudStorageAccount cloudStorageAccount;
        private CloudTableClient cloudTableClient;
        private CloudTable table;


        /*
         * the constructor establishes the connection with the cloud storage and creates a table if it doesn't exist
         */ 
        public DBHandler()
        {
            cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            table = cloudTableClient.GetTableReference("KriAssignment");
            table.CreateIfNotExists();
        }

        /*
         * It returns a list of customer objects that exist in the table with Customers as partition key
         */ 
        public List<Customer> GetCustomers()
        {
            var entities = table.ExecuteQuery(new TableQuery<Customer>()).ToList();
            List<Customer> customers = new List<Customer>();
            foreach (Customer entity in entities)
            {
                if (entity.PartitionKey == "Customers")
                {
                    customers.Add(entity);
                }
            }
            return customers;
        }

        /*
         * It returns a list of specific customer objects, that have the same name.
         * 
         */ 
        public List<Customer> GetSpecCustomers(String name)
        {
            List<Customer> customers = new List<Customer>();
            foreach (Customer customer in GetCustomers())
            {
                if (customer.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    customers.Add(customer);
                }
            }
            return customers;
        }

        /*
         * To insert a new instance of customer in the table
         * It checks first, if the customer's email exists or not
         * in case it exists, it returns false and no customer is inserted
         * in caise it does not exist, it inserts a new customer and returns true.
         */ 
        public bool AddCustomer(Customer customer)
        {
            if (GetCustomers().Find(a => a.Email == customer.Email) == null)
            {
                Customer c = new Customer()
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone
                };
                c.RowKey = customer.Email;

                TableOperation insert = TableOperation.Insert(c);
                table.Execute(insert);

                return true;
            }
            else
            {
                return false;
            }

        }
    }

}

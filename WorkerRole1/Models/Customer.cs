using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1.Models
{
    /*
     * Model class of a customer object, it implements tableentity to add a customer object to cloud table
     * ParitionKey is customers
     * and the Rowkey is set before inserting each customer into the cloudtable.
     */ 
    public class Customer : TableEntity
    {
        public Customer()
        {
            this.PartitionKey = "Customers";
        }

        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }

    }
}

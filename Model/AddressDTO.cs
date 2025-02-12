using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AdventureWorks.Model
{
    public class AddressDTO
    {
        public int AddressID{get; set;}

        public string AddressLine1{get; set;}

        public string AddressLine2{get; set;}

        public string City{get; set;}

        public int StateProvinceID{get; set;}

        public string PostalCode{get; set;}

        public System.Guid rowguid{get; set;}

        public System.DateTime ModifiedDate{get; set;}

        public string StatesProvinceName { get; set;}
        public string AddressesTypeName { get; set;}

        public int BusinessEntityID { get; set;}

        public string AddressessFullName { get; set;}
    }
}
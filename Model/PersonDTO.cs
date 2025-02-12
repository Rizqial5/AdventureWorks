using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AdventureWorks.Model
{
    public class PersonDTO
    {
        public int BusinessEntityID { get; set; }

        public string PersonType {get; set;}

        public bool NameStyle{get; set;}

        public string Title{get; set;}

        public string FirstName{get; set;}

        public string MiddleName{get; set;}

        public string LastName{get; set;}

        public string Suffix{get; set;}

        public string FullName{get; set;}

        public int EmailPromotion{get; set;}

        public System.Guid rowguid{get; set;}

        public System.DateTime ModifiedDate{get; set;}


    }
}
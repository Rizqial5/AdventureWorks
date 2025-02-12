using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventureWorks.Model
{
    public class StateProvinceDTO
    {
        public int StateProvinceID{get; set;}

        public string StateProvinceCode{get; set;}

        public string CountryRegionCode{get; set;}

        public bool IsOnlyStateProvinceFlag{get; set;}

        public string Name{get; set;}

        public int TerritoryID{get; set;}

        public System.Guid rowguid{get; set;}

        public System.DateTime ModifiedDate{get; set;}

        public string TerritorryNames { get; set;}
        public string CountryRegionNames { get; set;}

        public IEnumerable<CountryRegion> CountriesRegionsDdl{get; set;}

        public IEnumerable<SelectListItem> SalesTerritorriesDdl { get; set; }

        public string Flag 
        { 
            get 
            {
                return IsOnlyStateProvinceFlag ? "Yes" : "No"; 
            } 
        }
    }
}
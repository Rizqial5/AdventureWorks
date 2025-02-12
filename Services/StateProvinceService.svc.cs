using AdventureWorks.Model;
using PersoneManagement.Web.Models.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AdventureWorks.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StateProvinceService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StateProvinceService.svc or StateProvinceService.svc.cs at the Solution Explorer and start debugging.
    public class StateProvinceService : IStateProvinceService
    {
        private AdventureWorks22DataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["AdventureWorks2022ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public StateProvinceService()
        {
            _context = new AdventureWorks22DataContext(ConnectionString);
        }

        public IEnumerable<StateProvinceDTO> GetAll()
        {
            var query = from sp in _context.StateProvinces
                        join c in _context.CountryRegions on sp.CountryRegionCode equals c.CountryRegionCode
                        join t in _context.SalesTerritories on sp.TerritoryID equals t.TerritoryID
                        select new StateProvinceDTO
                        {
                            StateProvinceID = sp.StateProvinceID,
                            StateProvinceCode = sp.StateProvinceCode,
                            Name = sp.Name,
                            CountryRegionNames = c.Name,
                            TerritorryNames = t.Name,
                            IsOnlyStateProvinceFlag = sp.IsOnlyStateProvinceFlag,
                            ModifiedDate = sp.ModifiedDate,
                        };

            return query.ToList().OrderByDescending(sp => sp.ModifiedDate).Take(20);
        }

        public StateProvinceDTO GetStateProvinceById(int id)
        {
            var selectData = _context.StateProvinces.FirstOrDefault(sp => sp.StateProvinceID == id);

            return Mapping.Mapper.Map<StateProvinceDTO>(selectData);
        }

        public void InsertStateProvince(StateProvinceDTO stateProvinceDTO)
        {

            var newStateProvince = new StateProvince
            {
                StateProvinceID = stateProvinceDTO.StateProvinceID,
                Name = stateProvinceDTO.Name,
                StateProvinceCode = stateProvinceDTO.StateProvinceCode,
                IsOnlyStateProvinceFlag = stateProvinceDTO.IsOnlyStateProvinceFlag,
                CountryRegionCode = stateProvinceDTO.CountryRegionCode,
                TerritoryID = stateProvinceDTO.TerritoryID,
                ModifiedDate = DateTime.Now,
                rowguid = Guid.NewGuid()

            };



            _context.StateProvinces.InsertOnSubmit(newStateProvince);
            _context.SubmitChanges();
        }

        public void UpdateStateProvince(StateProvinceDTO stateProvinceDTO, Guid oldGUid)
        {
            var selectData = _context.StateProvinces.FirstOrDefault(sp => sp.StateProvinceID == stateProvinceDTO.StateProvinceID);

            var updatedData = Mapping.Mapper.Map(stateProvinceDTO,selectData);

            updatedData.ModifiedDate = DateTime.Now;
            updatedData.rowguid = oldGUid;
            

            _context.SubmitChanges();
        }

        public void DeleteStateProvince(int id)
        {
            var selectData = _context.StateProvinces.FirstOrDefault(sp => sp.StateProvinceID == id);

            _context.StateProvinces.DeleteOnSubmit(selectData);
            _context.SubmitChanges();
        }

        public IEnumerable<SalesTerritorryDTO> GetTerritoriesByRegionCode(string regionCode)
        {
            var territories = _context.SalesTerritories.ToList()
    .       Where(t => t.CountryRegionCode == regionCode).Select(t=> Mapping.Mapper.Map<SalesTerritorryDTO>(t));

            return territories;
        }

        public IEnumerable<CountryRegionDTO> GetCountryRegions()
        {
            var regions = from r in _context.CountryRegions
                          where r.CountryRegionCode == "US" || r.CountryRegionCode == "CA"
                          select r;

            var data = regions.Select(r=> Mapping.Mapper.Map<CountryRegionDTO>(r))
                .ToList();

            return data;
        }

        public IEnumerable<SalesTerritorryDTO> GetAllTerritories()
        {
            var territories = _context.SalesTerritories.ToList().Select(t => Mapping.Mapper.Map<SalesTerritorryDTO>(t));

            return territories;
        }

        public void ImportFromExcel(StateProvinceDTO stateProvinceDTO)
        {

            if(CheckStateProvinceId(stateProvinceDTO.StateProvinceID))
            {
                UpdateStateProvince(stateProvinceDTO, stateProvinceDTO.rowguid);
            }
            else
            {
                InsertStateProvince(stateProvinceDTO);
            }

        }

        private bool CheckStateProvinceId(int id)
        {
            var checkData = _context.StateProvinces.ToList();

            foreach (var item in checkData)
            {
                if (item.StateProvinceID == id) return true;
            }

            return false;   

        }

        public int GetTerritoriesIdByName(string territoriesName)
        {
            var checkData = _context.SalesTerritories.FirstOrDefault(st=> st.Name == territoriesName);

            return checkData.TerritoryID;
        }

        public string GetCountryRegionIdByName(string regionName)
        {
            var checkData = _context.CountryRegions.FirstOrDefault(cr => cr.Name == regionName);

            return checkData.CountryRegionCode;
        }
    }
}

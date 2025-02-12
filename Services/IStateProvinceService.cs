using AdventureWorks.Model;
using PersoneManagement.Web.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AdventureWorks.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStateProvinceService" in both code and config file together.
    [ServiceContract]
    public interface IStateProvinceService
    {
        [OperationContract]
        IEnumerable<StateProvinceDTO> GetAll();

        [OperationContract]
        StateProvinceDTO GetStateProvinceById(int id);

        [OperationContract]
        IEnumerable<SalesTerritorryDTO> GetTerritoriesByRegionCode(string regionCode);

        [OperationContract]
        int GetTerritoriesIdByName(string territoriesName);

        [OperationContract]
        IEnumerable<SalesTerritorryDTO> GetAllTerritories();

        [OperationContract]
        IEnumerable<CountryRegionDTO> GetCountryRegions();

        [OperationContract]
        string GetCountryRegionIdByName(string regionName);

        [OperationContract]
        void ImportFromExcel(StateProvinceDTO stateProvinceDTO);

        [OperationContract]
        void InsertStateProvince(StateProvinceDTO stateProvinceDTO);

        [OperationContract]
        void UpdateStateProvince(StateProvinceDTO stateProvinceDTO, Guid oldGUid);

        [OperationContract]
        void DeleteStateProvince(int id);   
    }
}

using AdventureWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AdventureWorks.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAddressService" in both code and config file together.
    [ServiceContract]
    public interface IAddressService
    {
        [OperationContract]
        IEnumerable<AddressDTO> GetAddressesByBusinessId(int businessEntityId);

        [OperationContract]
        AddressDTO GetAddressById(int id, int businessEntityID);

        [OperationContract]
        IEnumerable<AddressTypeDTO> GetAddressTypes();

        [OperationContract]
        void AddAddress(AddressDTO addressDTO, int businessEntityID);
        [OperationContract]
        void UpdateAddress(AddressDTO addressDTO, int businessEntityID, Guid oldGuid);
        [OperationContract]
        void DeleteAddress(int id, int businessEntityID);


    }
}

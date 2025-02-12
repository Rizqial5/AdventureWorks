using AdventureWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AdventureWorks.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPersonService" in both code and config file together.
    [ServiceContract]
    public interface IPersonService
    {
        [OperationContract]
        IEnumerable<PersonDTO> GetPersonLists();

        [OperationContract]
        PersonDTO GetPerson(int businessEntityId);


        [OperationContract]
        void CreatePerson(PersonDTO personDTO);

        [OperationContract]
        void UpdatePerson(PersonDTO personDTO, Guid oldGuild);

        [OperationContract]
        void DeletePerson(int businessEntityId);
       
        [OperationContract]
        string GetFullName(int businessEntityId);

    }
}

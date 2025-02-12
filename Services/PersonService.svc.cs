using AdventureWorks.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AdventureWorks.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PersonService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PersonService.svc or PersonService.svc.cs at the Solution Explorer and start debugging.
    public class PersonService : IPersonService
    {

        private AdventureWorks22DataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["AdventureWorks2022ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public PersonService()
        {
            _context = new AdventureWorks22DataContext(ConnectionString);
        }
        public void CreatePerson(PersonDTO personDTO)
        {
            var businessEntity = new BusinessEntity
            {
                ModifiedDate = DateTime.Now,
                rowguid = Guid.NewGuid(),
            };
            _context.BusinessEntities.InsertOnSubmit(businessEntity);
            _context.SubmitChanges();


            personDTO.BusinessEntityID = businessEntity.BusinessEntityID;
            personDTO.rowguid = businessEntity.rowguid;
            personDTO.ModifiedDate = businessEntity.ModifiedDate;


            var newData = Mapping.Mapper.Map<Person>(personDTO);

            _context.Persons.InsertOnSubmit(newData);
            _context.SubmitChanges();
        }

        public void DeletePerson(int businessEntityId)
        {
            var existingData = _context.Persons.First(sp => sp.BusinessEntityID == businessEntityId);

            _context.Persons.DeleteOnSubmit(existingData);

            _context.SubmitChanges();

            var businesEntityTarget = _context.BusinessEntities.First(be => be.BusinessEntityID == businessEntityId);


            _context.BusinessEntities.DeleteOnSubmit(businesEntityTarget);


            _context.SubmitChanges();
        }

        public string GetFullName(int businessEntityId)
        {
            var person = _context.Persons.First(p => p.BusinessEntityID == businessEntityId);



            return (person.Title ?? "")+ " " + person.FirstName + " " + (person.MiddleName ?? "") + " " + person.LastName;
        }

        public PersonDTO GetPerson(int businessEntityId)
        {
            var person = _context.Persons.FirstOrDefault(p => p.BusinessEntityID == businessEntityId);

            if (person == null)
            {
                return null;
            }

            var result = Mapping.Mapper.Map<PersonDTO>(person);

            return result;
        }

        public IEnumerable<PersonDTO> GetPersonLists()
        {
            var personList = from p in _context.Persons
                             select new PersonDTO
                             {
                                 BusinessEntityID = p.BusinessEntityID,
                                 EmailPromotion = p.EmailPromotion,
                                 FirstName = p.FirstName,
                                 FullName = GetFullName(p.BusinessEntityID),
                                 LastName = p.LastName,
                                 MiddleName = p.MiddleName,
                                 ModifiedDate = p.ModifiedDate,
                                 NameStyle = p.NameStyle,
                                 PersonType = p.PersonType,
                                 rowguid = p.rowguid,
                                 Suffix = p.Suffix,
                                 Title = p.Title,
                                 
                             };

            //var result = personList.Select(p=> Mapping.Mapper.Map<PersonDTO>(p));

           

            

            return personList.OrderByDescending(p=> p.ModifiedDate).Take(10);
        }

        public void UpdatePerson(PersonDTO personDTO, Guid oldGuild)
        {
            var targetData = _context.Persons.FirstOrDefault(p => p.BusinessEntityID == personDTO.BusinessEntityID);

            var updatedData = Mapping.Mapper.Map(personDTO, targetData);

            updatedData.ModifiedDate = DateTime.Now;
            updatedData.rowguid = oldGuild;

            _context.SubmitChanges();
        }

    }
}

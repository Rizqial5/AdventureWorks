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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AddressService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AddressService.svc or AddressService.svc.cs at the Solution Explorer and start debugging.
    public class AddressService : IAddressService
    {

        private AdventureWorks22DataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["AdventureWorks2022ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public AddressService()
        {
            _context = new AdventureWorks22DataContext(ConnectionString);
        }
        public void AddAddress(AddressDTO addressDTO, int businessEntityID)
        {


            //addressDTO.AddressID = businessEntityAddress.AddressID;
            //addressDTO.rowguid = businessEntityAddress.rowguid;
            //addressDTO.ModifiedDate = businessEntityAddress.ModifiedDate;

            var newData = Mapping.Mapper.Map<Address>(addressDTO);

            var addressData = new Address
            {
                AddressLine1 = addressDTO.AddressLine1,
                AddressLine2 = addressDTO.AddressLine2,
                City = addressDTO.City,
                PostalCode = addressDTO.PostalCode,
                rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now,
            };

            addressData.StateProvinceID = addressDTO.StateProvinceID;


            _context.Addresses.InsertOnSubmit(addressData);
            _context.SubmitChanges();


            var businessEntityAddress = new BusinessEntityAddress
            {
                ModifiedDate = DateTime.Now,
                AddressID = addressData.AddressID,
                AddressTypeID = Convert.ToInt16(addressDTO.AddressesTypeName),
                BusinessEntityID = businessEntityID,
                rowguid = Guid.NewGuid(),
            };

            _context.BusinessEntityAddresses.InsertOnSubmit(businessEntityAddress);

            _context.SubmitChanges();
        }

        public void DeleteAddress(int id, int businessEntityID)
        {
            var targetData = from a in _context.Addresses
                             join bea in _context.BusinessEntityAddresses on a.AddressID equals bea.AddressID
                             where bea.BusinessEntityID == businessEntityID
                             select a;

            var beaData = from bea in _context.BusinessEntityAddresses
                          where bea.BusinessEntityID == businessEntityID
                          select bea;

            var target = targetData.First(a => a.AddressID == id);

            var targetTypeId = beaData.First(bea => bea.AddressID == id);


            _context.BusinessEntityAddresses.DeleteOnSubmit(targetTypeId);

            _context.SubmitChanges();

            _context.Addresses.DeleteOnSubmit(target);

            _context.SubmitChanges();


        }

        public AddressDTO GetAddressById(int id, int businessEntityID)
        {
            var targetData = from a in _context.Addresses
                             join bea in _context.BusinessEntityAddresses on a.AddressID equals bea.AddressID
                             where bea.BusinessEntityID == businessEntityID
                             select a;

            var target = targetData.FirstOrDefault(a => a.AddressID == id);

            var result = Mapping.Mapper.Map<AddressDTO>(target);

            return result;
        }

        public IEnumerable<AddressDTO> GetAddressesByBusinessId(int businessEntityId)
        {
            var addressList = from a in _context.Addresses
                              join sp in _context.StateProvinces on a.StateProvinceID equals sp.StateProvinceID
                              join bea in _context.BusinessEntityAddresses on a.AddressID equals bea.AddressID
                              join at in _context.AddressTypes on bea.AddressTypeID equals at.AddressTypeID
                              join p in _context.Persons on bea.BusinessEntityID equals p.BusinessEntityID
                              where bea.BusinessEntityID == businessEntityId
                              select new AddressDTO
                              {
                                  AddressID = a.AddressID,
                                  BusinessEntityID = p.BusinessEntityID,
                                  AddressesTypeName = at.Name,
                                  AddressessFullName = a.AddressLine1 + ", " + (a.AddressLine2 ?? ""),
                                  StatesProvinceName = sp.Name,
                                  City = a.City,
                                  PostalCode = a.PostalCode,
                                  ModifiedDate = a.ModifiedDate,


                              };

            var result = addressList.ToList().OrderByDescending(a => a.ModifiedDate).Take(10);

            return result;
        }

        public IEnumerable<AddressTypeDTO> GetAddressTypes()
        {
            var addressList = from at in _context.AddressTypes select at;

            var result = addressList.Select(a => Mapping.Mapper.Map<AddressTypeDTO>(a));

            return result.ToList();
        }

        public void UpdateAddress(AddressDTO addressDTO, int businessEntityID, Guid oldGuild)
        {
            var targetData = from a in _context.Addresses
                             join bea in _context.BusinessEntityAddresses on a.AddressID equals bea.AddressID
                             where bea.BusinessEntityID == businessEntityID
                             select a;

            var beaData = from bea in _context.BusinessEntityAddresses
                          where bea.BusinessEntityID == businessEntityID
                          select bea;

            var target = targetData.First(a => a.AddressID == addressDTO.AddressID);

            var targetTypeId = beaData.First(bea => bea.AddressID == addressDTO.AddressID);


            _context.BusinessEntityAddresses.DeleteOnSubmit(targetTypeId);

            _context.SubmitChanges();


            var updatedData = Mapping.Mapper.Map(addressDTO, target);

            //target.AddressLine1 = addressDTO.AddressLine1;
            //target.AddressLine2 = addressDTO.AddressLine2;
            //target.rowguid = oldGuild;
            //target.ModifiedDate = DateTime.Now;
            //target.PostalCode = addressDTO.PostalCode;
            //target.City = addressDTO.City;
            //target.StateProvinceID = addressDTO.StateProvinceID;

            updatedData.ModifiedDate = DateTime.Now;
            updatedData.rowguid = oldGuild;

            _context.SubmitChanges();


            var businessEA = new BusinessEntityAddress
            {
                AddressID = addressDTO.AddressID,
                AddressTypeID = Convert.ToInt16(addressDTO.AddressesTypeName),
                BusinessEntityID = businessEntityID,
                ModifiedDate = DateTime.Now,
                rowguid = Guid.NewGuid(),
            };

            _context.BusinessEntityAddresses.InsertOnSubmit(businessEA);

            _context.SubmitChanges();




        }
    }
}

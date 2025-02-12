
using AdventureWorks.Services;
using AutoMapper;
using PersoneManagement.Web.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.Model
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ModelMapping>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<StateProvince,StateProvinceDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<CountryRegion, CountryRegionDTO>();
            CreateMap<SalesTerritory, SalesTerritorryDTO>();
            CreateMap<AddressType, AddressTypeDTO>();


        }
    }
}

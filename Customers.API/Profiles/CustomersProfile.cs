using AutoMapper;

namespace Customers.API.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Entities.Customer, Models.Customer>();
            CreateMap<Models.Customer, Entities.Customer>();
        }
    }
}

using AutoMapper;
using System.Transactions;

namespace FraudDetection.Api
{
    public class DefaultAutoMapperProfile : Profile
    {
        public DefaultAutoMapperProfile()
        {
            CreateMap<Models.DTO.TransactionDto, Models.Transaction>().ReverseMap();
        }
    }
}

using AutoMapper;
using Domain.Entities.Aggregates;
using Shared.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class DeliveryProfile : Profile
    {
        public DeliveryProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodToReturnDto>();

        }
    }
}

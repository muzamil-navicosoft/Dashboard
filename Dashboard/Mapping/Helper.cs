﻿using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Mapping
{
    public class Helper : IHelper
    {
        // For multi Level Mapping  or Complex Mapping
        public void ConfigureMapster()
        {

            TypeAdapterConfig<Ticket, TicketDto>.NewConfig()
                .Map(dest => dest.Email, src => src.ClientForm.Email);

            TypeAdapterConfig<BillingInfo, BillingInfoDto>.NewConfig()
                .Map(dest => dest.Email, src => src.ClientForm.Email);

            


            //TypeAdapterConfig<ClientForm, ClientFormDto>.NewConfig()
            //    .Map(dest => dest.BillingInfoDtos, src => src.BillingInfos.Adapt<List<BillingInfo>>());

        }
    }
}



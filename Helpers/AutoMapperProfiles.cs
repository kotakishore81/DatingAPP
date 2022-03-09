﻿using AutoMapper;
using Dating_API.DTOs;
using Dating_API.Entites;
using Dating_API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_API.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles() {
            CreateMap<AppUser, MemberDto>()
           .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
               src.Photos.FirstOrDefault(x => x.IsMain).Url))
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}

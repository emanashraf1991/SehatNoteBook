using AutoMapper;
using SehatNotebook.Entities.DBSet;
using SehatNoteBook.Entities.Dtos.Incoming;
using SehatNoteBook.Entities.Dtos.Outgoing;

namespace SehatNoteBook.Api{
    public class UserProfile : Profile{
        public UserProfile()
        {
            CreateMap<UserDto,User>()
                .ForMember(dest=>dest.FirstName, from=>from.MapFrom(x=>$"{x.FirstName}"))
                .ForMember(dest=>dest.LastName, from=>from.MapFrom(x=>$"{x.LastName}"))
                .ForMember(dest=>dest.Email, from=>from.MapFrom(x=>$"{x.Email}"))
                .ForMember(dest=>dest.Phone, from=>from.MapFrom(x=>$"{x.Phone}"))
                .ForMember(dest => dest.DateOfBirth, from=>from.MapFrom(x=>Convert.ToDateTime( x.DateOfBirth)))
                .ForMember(dest =>  dest.Address, from=>from.MapFrom(x=>$""))
                .ForMember(dest =>  dest.Sex,  from=>from.MapFrom(x=>$""))
                .ForMember(dest =>  dest.MobileNumber, from=>from.MapFrom(x=>$""));

                 CreateMap<User,ProfileDto>()
                .ForMember(dest=>dest.Country, from=>from.MapFrom(x=>$"{x.Country}"))
                .ForMember(dest=>dest.FirstName, from=>from.MapFrom(x=>$"{x.FirstName}"))
                .ForMember(dest=>dest.LastName, from=>from.MapFrom(x=>$"{x.LastName}"))
                .ForMember(dest=>dest.Email, from=>from.MapFrom(x=>$"{x.Email}"))
                .ForMember(dest=>dest.Phone, from=>from.MapFrom(x=>$"{x.Phone}"))
                .ForMember(dest => dest.DateOfBirth, from=>from.MapFrom(x=>Convert.ToDateTime( x.DateOfBirth)))
                ;


        }

    }
}
using AutoMapper;
using DB.DomainObjects;
using DB.Utils;
using ManagingSavingDepositsAPI.DTOs.Users;

namespace ManagingSavingDepositsAPI.Profiles;

public class AutoMapperProfile : Profile
{
	private const char Separator = ' ';

	public AutoMapperProfile()
	{
		CreateMap<User, DTOUser>(MemberList.Destination)
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MiddleName.Length > 0 ? $"{src.FirstName} {src.MiddleName}" : src.FirstName))
			.ForMember(dest => dest.UserRoleName, opt => opt.MapFrom(src => src.UserRole.Name));

		CreateMap<DTOUserCreate, User>(MemberList.Source)
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]))
			.ForMember(dest => dest.MiddleName, opt => opt.MapFrom((src, dest) => {
				var splitedTxt = src.Name.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
				return splitedTxt.Length > 1 ? splitedTxt[1] : "";
			}))
			.ForMember(dest => dest.Password, opt => opt.MapFrom(src => Utils.EncodeString(src.Password)));
	}
}

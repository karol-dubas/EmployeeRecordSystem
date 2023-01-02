using AutoMapper;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;

namespace EmployeeRecordSystem.Server.Mappings;

public class AnnouncementMappingProfile : Profile
{
	public AnnouncementMappingProfile()
	{
		CreateMap<CreateAnnouncementRequest, Announcement>();
		CreateMap<Announcement, AnnouncementDto>()
			.ForMember(m => m.CreatedBy, c => c.MapFrom(s => s.CreatedBy.FullName));
	}
}

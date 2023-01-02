using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static EmployeeRecordSystem.Server.Installers.Helpers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IAnnouncementService
{
	AnnouncementDto Create(CreateAnnouncementRequest request);
	List<AnnouncementDto> GetAll(AnnouncementQuery query);
	void Update(Guid announcementId, CreateAnnouncementRequest request);
	void Delete(Guid announcementId);
}

[ScopedRegistration]
public class AnnouncementService : BaseService, IAnnouncementService
{
	private readonly IAuthorizationService _authorizationService;

	public AnnouncementService(
		ApplicationDbContext dbContext,
		IMapper mapper,
		IAuthorizationService authorizationService) : base(dbContext, mapper)
	{
		_authorizationService = authorizationService;
	}

	public AnnouncementDto Create(CreateAnnouncementRequest request)
	{
		var announcement = Mapper.Map<Announcement>(request);
		announcement.CreatedById = _authorizationService.UserId;
		DbContext.Announcements.Add(announcement);
		SaveChanges();
		return Mapper.Map<AnnouncementDto>(announcement);
	}

	public List<AnnouncementDto> GetAll(AnnouncementQuery query)
	{
		var queryable = DbContext.Announcements
			.Include(g => g.CreatedBy)
			.AsNoTracking();

		queryable = ApplyGetAllFilter(query, queryable);

		var announcements = queryable.ToList();
		return Mapper.Map<List<AnnouncementDto>>(announcements);
	}

	public void Update(Guid announcementId, CreateAnnouncementRequest request)
	{
		var announcement = DbContext.Announcements.SingleOrDefault(a => a.Id == announcementId);

		if (announcement is null)
			throw new NotFoundException(nameof(announcementId), "Announcement");
		
		Mapper.Map(request, announcement);
		SaveChanges();
	}

	public void Delete(Guid announcementId)
	{
		var announcement = DbContext.Announcements.SingleOrDefault(a => a.Id == announcementId);

		if (announcement is null)
			throw new NotFoundException(nameof(announcementId), "Announcement");

		DbContext.Announcements.Remove(announcement);
		SaveChanges();
	}
	
	private static IQueryable<Announcement> ApplyGetAllFilter(AnnouncementQuery query, IQueryable<Announcement> queryable)
	{
		if (query.Id != default)
			queryable = queryable.Where(g => g.Id == query.Id);

		return queryable;
	}
}

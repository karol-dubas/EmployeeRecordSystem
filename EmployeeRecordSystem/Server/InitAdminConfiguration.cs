using System.ComponentModel.DataAnnotations;

namespace EmployeeRecordSystem.Server;

public class InitAdminConfiguration
{
	public const string SectionName = "Seeder:InitAdmin";

	[Required]
	[MaxLength(50)]
	public string FirstName { get; set; }

	[Required]
	[MaxLength(50)]
	public string LastName { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
	public string Password { get; set; }
}

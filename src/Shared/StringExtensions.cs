using EmployeeRecordSystem.Shared.Constants;

namespace EmployeeRecordSystem.Shared;

public static class StringTranslateExtensions
{
	public static string TranslateRole(this string role) => role switch
	{
		Roles.Admin => "Administrator",
		Roles.Supervisor => "Kierownik",
		Roles.Employee => "Pracownik",
		_ => throw new InvalidOperationException()
	};
	
	public static string TranslateWithdrawalRequest(this string wr) => wr switch
	{
		WithdrawalRequestStatusTypeCodes.Accepted => "akceptowany",
		WithdrawalRequestStatusTypeCodes.Denied => "odrzucony",
		WithdrawalRequestStatusTypeCodes.Pending => "oczekujący",
		_ => throw new InvalidOperationException()
	};
}

using System.Text;

namespace EmployeeRecordSystem.IntegrationTests.Helpers;

public static class RandomString
{
	public static string Generate(int length)
	{
		const string allowedCharacters = "0123456789abcdefghijklmnopqrstuvwxyz";
		var sb = new StringBuilder();

		for (int i = 0; i < length; i++)
		{
			int randomIndex = Random.Shared.Next(allowedCharacters.Length);
			sb.Append(allowedCharacters.ElementAt(randomIndex));
		}

		return sb.ToString();
	}
}

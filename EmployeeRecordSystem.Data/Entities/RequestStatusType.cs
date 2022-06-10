using EmployeeRecordSystem.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class RequestStatusType : Enumeration
    {
		public static readonly RequestStatusType New = new(Codes.New, nameof(New));
		public static readonly RequestStatusType Denied = new(Codes.Denied, nameof(Denied));
		public static readonly RequestStatusType Accepted = new(Codes.Accepted, nameof(Accepted));

		private RequestStatusType() { }
		private RequestStatusType(string code, string name) : base(code, name) { }

		public static class Codes
		{
			public const string New = "new";
			public const string Denied = "denied";
			public const string Accepted = "accepted";
		}
	}
}

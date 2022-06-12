using EmployeeRecordSystem.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class WithdrawalRequestStatusType : Enumeration
    {
		public static readonly WithdrawalRequestStatusType Pending = new(Codes.Pending, nameof(Pending));
		public static readonly WithdrawalRequestStatusType Denied = new(Codes.Denied, nameof(Denied));
		public static readonly WithdrawalRequestStatusType Accepted = new(Codes.Accepted, nameof(Accepted));

		private WithdrawalRequestStatusType() { }
		private WithdrawalRequestStatusType(string code, string name) : base(code, name) { }

		public static class Codes
		{
			public const string Pending = "pending";
			public const string Denied = "denied";
			public const string Accepted = "accepted";
		}
	}
}

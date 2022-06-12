using EmployeeRecordSystem.Data.Helpers;
using EmployeeRecordSystem.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class WithdrawalRequestStatusType : Enumeration
    {
		public static readonly WithdrawalRequestStatusType Pending = new(WithdrawalRequestStatusTypeCodes.Pending, nameof(Pending));
		public static readonly WithdrawalRequestStatusType Denied = new(WithdrawalRequestStatusTypeCodes.Denied, nameof(Denied));
		public static readonly WithdrawalRequestStatusType Accepted = new(WithdrawalRequestStatusTypeCodes.Accepted, nameof(Accepted));

		private WithdrawalRequestStatusType() { }
		private WithdrawalRequestStatusType(string code, string name) : base(code, name) { }
	}
}

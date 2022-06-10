using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Helpers
{
    public class Enumeration
    {
		private readonly string _code;
		private readonly string _name;

		public string Code => _code;
		public string Name => _name;

		protected Enumeration() { }

		protected Enumeration(string code, string name)
		{
			_code = code;
			_name = name;
		}

		public static IEnumerable<T> GetAll<T>() where T : Enumeration
		{
			var type = typeof(T);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (var info in fields)
			{
				var instance = Activator.CreateInstance(typeof(T), true);

				if (info.GetValue(instance) is T locatedValue)
					yield return locatedValue;
			}
		}
	}
}

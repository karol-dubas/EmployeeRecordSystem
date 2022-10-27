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
	    public string Code { get; }
		public string Name { get; }

		protected Enumeration() { }

		protected Enumeration(string code, string name)
		{
			Code = code;
			Name = name;
		}

		public static IEnumerable<T> GetAll<T>() where T : Enumeration
		{
			var type = typeof(T);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (var info in fields)
			{
				object instance = Activator.CreateInstance(typeof(T), true);

				if (info.GetValue(instance) is T locatedValue)
					yield return locatedValue;
			}
		}
	}
}

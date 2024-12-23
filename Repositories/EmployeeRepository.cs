using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_LAB_03.Models;
namespace DB_LAB_03.Repositories
{
	internal class EmployeeRepository
	{
		public static List<Employee> GetEmployees(string? title = null)
		{
			using (var context = new Lab02Context())
			{
				return context.Employees.Where(e => title == null || e.Title == title).ToList();
			}
		}

		public static List<string> GetTitles()
		{
			using (var context = new Lab02Context())
			{
				List<string> classes =  context.Employees.Select(s => s.Title).Distinct().ToList();
				return classes;
			}
		}
		public static bool AddEmpoyee(string fname, string lname, string title)
		{
			using (var context = new Lab02Context())
			{
				Employee emp = new Employee { FirstName = fname, LastName = lname, Title = title};
				context.Employees.Add(emp);
				int changes = context.SaveChanges();
				return changes > 0;
			}
		}
	}
}

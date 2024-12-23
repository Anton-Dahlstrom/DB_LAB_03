using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB_LAB_03.Models;
namespace DB_LAB_03.Repositories
{
	internal class StudentRepository
	{
		public static List<Student> GetAllStudents<T>(Expression<Func<Student, T>> orderBy, bool orderByAsc = true)
		{
			using (var context = new Lab02Context())
			{
				IQueryable<Student> query = context.Students;
				query = orderByAsc
					? query.OrderBy(orderBy)
					: query.OrderByDescending(orderBy);
				return query.ToList();
			}
		}
		public static List<Student> GetStudentsInClass(string cls)
		{
			using (var context = new Lab02Context())
			{
				return context.Students.Where(s => s.Class == cls).ToList();
			}
		}
		public static List<string> GetClasses()
		{
			using (var context = new Lab02Context())
			{
				List<string> classes =  context.Students.Select(s => s.Class).Distinct().ToList();
				return classes;
			}
		}

		public static bool AddStudent(string fname, string lname, string socialsecurity, string cls)
		{
			using (var context = new Lab02Context())
			{
				Student student = new Student { FirstName = fname, LastName = lname, SocialSecurity = socialsecurity, Class = cls};
				context.Students.Add(student);
				int changes = context.SaveChanges();
				return changes > 0;
			}
		}
		public static bool CheckUniqueSSN(string ssn)
		{
			Student? student;
			using (var context = new Lab02Context())
			{
				student = context.Students.FirstOrDefault(s => s.SocialSecurity == ssn);
			}
			return student == null; 
		}
	}
}

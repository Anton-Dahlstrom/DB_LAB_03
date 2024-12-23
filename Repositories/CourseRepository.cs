using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_LAB_03.Models;
using Microsoft.EntityFrameworkCore;
namespace DB_LAB_03.Repositories
{
	internal class CourseRepository
	{
		public static List<Course> GetCourseGrades()
		{
			using (var context = new Lab02Context())
			{
				List<Course> courses = context.Courses
					.Include(c => c.Grades)
					.ToList();
				return courses;
			}
		}
	}
}

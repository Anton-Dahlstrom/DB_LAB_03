using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_LAB_03.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_LAB_03.Repositories
{
	internal class GradeRepository
	{
		// Get all grades set in the past month
		public static List<Grade> GetRecentGrades()
		{
			using (var context = new Lab02Context())
			{
				List<Grade> classes =  context.Grades
					.Where(g => g.GradeDate >= DateTime.Now.AddMonths(-1) )
					.Include(g => g.Student)
					.Include(g => g.Course)
					.ToList();
				return classes;
			}
		}
	}
}

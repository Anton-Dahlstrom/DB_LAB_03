using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DB_LAB_03.Models;
using DB_LAB_03.Repositories;
using Sprache;
namespace DB_LAB_03
{
	internal class Menu
	{
		public static void MainMenu()
		{
			bool valid = false;
			int input = -1;
			while(valid != true)
			{
                Console.WriteLine();
				Console.WriteLine("Choose an action");
				Console.WriteLine("0: Display employees");
				Console.WriteLine("1: Display students in a class");
				Console.WriteLine("2: Display all students");
				Console.WriteLine("3: Display grades set the past month");
				Console.WriteLine("4: Display grade stats");
				Console.WriteLine("5: Add a student");
				Console.WriteLine("6: Add an employee");
				valid  = int.TryParse(Console.ReadLine(), out input);
			}

			switch (input)
			{
				case 0:
					DisplayEmployees();
					break;

				case 1:
					DisplayStudentsInClass();
					break;
				case 2:
					DisplayAllStudents();
					break;
				case 3:
					DisplayRecentGrades();
					break;
				case 4:
					DisplayGradeStats();
					break;
				case 5:
					AddStudent();
					break;
				case 6:
					AddEmployee();
					break;
				default:
					MainMenu();
					break;
			}
		}
		public static void ReturnToMainMenu()
		{
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu!");
			Console.ReadKey();
			Console.Clear();
			MainMenu();
		}
		public static void DisplayEmployees()
		{

			// Display titles
			bool valid = false;
			int input = -1;
			string? title = null;
			while(valid != true)
			{
				int i;
				Console.WriteLine("Filter by job title:");
                Console.WriteLine("0: All titles");
				var titles = EmployeeRepository.GetTitles();
				for (i = 0; i < titles.Count; i++)
				{
					Console.WriteLine($"{i+1}: {titles[i]}");
				}
				valid  = int.TryParse(Console.ReadLine(), out input);
				if (valid && (0 <= input && input <= titles.Count))
				{
					title = titles[input - 1];
				}
				else 
				{ 
					valid = false;
					Console.WriteLine("Enter a valid number");
				}
                Console.WriteLine();
			}

			// Get employees by title 
			foreach(var s in EmployeeRepository.GetEmployees(title))
			{
                Console.WriteLine($"{s.FirstName}, {s.LastName}");
			}
			ReturnToMainMenu();
		}

		public static void DisplayAllStudents()
		{
			// Get all students
			bool valid = false;
			int input1 = -1;
			while(valid != true)
			{
                Console.WriteLine("What do you want to order by?");
                Console.WriteLine("0: First name");
                Console.WriteLine("1: Last name");
				valid  = int.TryParse(Console.ReadLine(), out input1);
				if (valid == true && (input1 == 0 || input1 == 1))
				{
					break;
				}
				else 
				{ 
					valid = false;
					Console.WriteLine("Please enter a number in the menu");
				}
                Console.WriteLine();
			}

			valid = false;
			int input2 = -1;
			while(valid != true)
			{
                Console.WriteLine("How do you want to order the students?");
                Console.WriteLine("0: ASC");
                Console.WriteLine("1: DESC");
				valid  = int.TryParse(Console.ReadLine(), out input2);
				if (valid == true && (input2 == 0 || input2 == 1))
				{
					break;
				}
				else 
				{ 
					valid = false;
					Console.WriteLine("Please enter a number in the menu");
				}
                Console.WriteLine();
			}
			Expression<Func<Student, string>> orderBy;
			if (input1 == 0)
			{
				orderBy = s => s.FirstName;
			}
			else
			{
				orderBy = s => s.LastName;

			}
			foreach(var s in StudentRepository.GetAllStudents(orderBy, input2 == 0))
			{
                Console.WriteLine($"{s.FirstName}, {s.LastName}");
			}
			ReturnToMainMenu();
		}

		public static void DisplayStudentsInClass() 
		{
			// Display classes
			var classes = StudentRepository.GetClasses();

			bool valid = false;
			string cls = "";
			int input = -1;
			while(valid != true)
			{
				Console.WriteLine("Select a class:");
				var titles = StudentRepository.GetClasses();
				for (int i = 0; i < classes.Count; i++)
				{
					Console.WriteLine($"{i}: {classes[i]}");
				}
				valid  = int.TryParse(Console.ReadLine(), out input);
				if (valid && (0 <= input && input <= titles.Count))
				{
					cls = titles[input];
				}
				else 
				{ 
					valid = false;
					Console.WriteLine("Enter a valid number");
				}
                Console.WriteLine();
			}

			// Get students by class
			foreach(var s in StudentRepository.GetStudentsInClass(cls))
			{
                Console.WriteLine($"{s.FirstName}, {s.LastName}");
			}
			ReturnToMainMenu();
		}
		public static void DisplayRecentGrades()
		{
			// Get all grades set in the past month
			foreach(var g in GradeRepository.GetRecentGrades())
			{
				Console.WriteLine($"{g.Student.FirstName} {g.Student.LastName}: {g.Grade1}, {g.Course.CourseName}, {g.GradeDate.ToShortDateString()}");
			}
			ReturnToMainMenu();
		}

		public static void DisplayGradeStats()
		{
			// Get a list of all courses with average, highest and lowest grade.
			foreach(var c in CourseRepository.GetCourseGrades())
			{
				Console.WriteLine($"{c.CourseName} Grades:");
                Console.WriteLine(
					$"Max = {c.Grades.Max(g => g.Grade1)}, " +
					$"Min = {c.Grades.Min(g => g.Grade1)}, " +
					$"Avg = {c.Grades.Average(g => g.Grade1)}");
                Console.WriteLine();
			}
			ReturnToMainMenu();
		}

		public static void AddStudent()
		{
			// Add student
			string[] inputs = {"First name", "Last name", "Class" };
			string input;
			for (int i = 0; i < inputs.Length; i++)
			{
				input = "";
				while (1 > input.Length || input.Length > 64)
				{
				Console.WriteLine($"Input {inputs[i]}: ");
				input = Console.ReadLine();
				}
				inputs[i] = input;
			}
			string ssn = "";
			bool unique = false;
			while ((1 > ssn.Length || ssn.Length > 64) || unique == false)
			{
				Console.WriteLine($"Input social security number: ");
				ssn = Console.ReadLine();
				unique = StudentRepository.CheckUniqueSSN(ssn);
				if(unique == false)
				{
                    Console.WriteLine("The social security number needs to be unique.");
                    Console.WriteLine();
				}
			}
			bool added = StudentRepository.AddStudent(inputs[0], inputs[1], ssn, inputs[2]);
			string feedback = added ? "successfully" : "not";
            Console.WriteLine($"The student was {feedback} added");
			ReturnToMainMenu();
		}


		public static void AddEmployee()
		{
			// Add employee
			string[] inputs = {"First name", "Last name", "Title" };
			string input;
			for (int i = 0; i < inputs.Length; i++)
			{
				input = "";
				while (1 > input.Length || input.Length > 64)
				{
				Console.WriteLine($"Input {inputs[i]}: ");
				input = Console.ReadLine();
				}
				inputs[i] = input;
			}
			bool added = EmployeeRepository.AddEmpoyee(inputs[0], inputs[1], inputs[2]);
			string feedback = added ? "successfully" : "not";
            Console.WriteLine($"The employee was {feedback} added");
			ReturnToMainMenu();
		}
	}
}

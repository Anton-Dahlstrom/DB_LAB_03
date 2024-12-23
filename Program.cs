using DB_LAB_03.Models;
using DB_LAB_03.Repositories;
using DotNetEnv;
namespace DB_LAB_03
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Env.Load();
			Menu.MainMenu();
		}
	}
}

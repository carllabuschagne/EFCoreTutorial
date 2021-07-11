using System;
using EFCoreTutorial.EntityClasses;
using EFCoreTutorial.ContextClasses;
using System.Linq;

namespace EFCoreTutorial
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			SaveData();
			LoadData();
		}

		static void LoadData()
		{
			var context = new SchoolContext();

			var StudentsWithSameName = context.Students
				.Where(s => s.FirstName == "David Hasselhoff")
				.ToList();


			foreach(Student s in StudentsWithSameName)
			{
				Console.WriteLine("name:" + s.FirstName);
			}

			
		}


		static void SaveData()
		{
			using (var context = new SchoolContext())
			{
				Student _student = new Student();

				_student.FirstName = "David Hasselhoff";

				context.Students.Add(_student);
				context.SaveChanges();
			}
		}


	}
}

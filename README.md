# Entity Framework .Net Core



Install Nuget Pachagaes

```C#
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

<br />
<br />

Create Entity Classes.

```C#
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
}
```

```C#
public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
}
```

<br />
<br />

Create Context Classes (DbContext)

```C#

using EFCoreTutorial.EntityClasses;
using Microsoft.EntityFrameworkCore;

 public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;");
        }
    }
```

<br />
<br />

Add Migration to create migrations 

```C#
PM>  add-migration CreateSchoolDB
```

<br />
<br />

Update Database to execute migrations

```C#
PM> update-database –verbose
```

<br />
<br />

Reading or Writing Data

```C#
class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Hello World!");

		using (var context = new ContextClasses.SchoolContext())
		{
			Student _student = new Student();

			_student.Name = "Dsvid Hasselhoff";

			context.Students.Add(_student);
			context.SaveChanges();
		}

	}
}
```

<br />
<br />

```C#
using System.Linq;

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
			.Where(s => s.Name == "David Hasselhoff")
			.ToList();

		foreach(Student s in StudentsWithSameName)
		{
			Console.WriteLine("name:" + s.Name);
		}			
	}

	static void SaveData()
	{
		using (var context = new SchoolContext())
		{
			Student _student = new Student();

			_student.Name = "David Hasselhoff";

			context.Students.Add(_student);
			context.SaveChanges();
		}
	}
}
``



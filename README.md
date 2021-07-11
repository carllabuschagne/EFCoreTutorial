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
```

<br />
<br />

Querying in Entity Framework Core

<br />

LINQ-to-Entities

```C#
var context = new SchoolContext();
    var studentsWithSameName = context.Students
                                      .Where(s => s.FirstName == "Dave")
                                      .ToList();
```

<br />

### Includes

Specify a reference property to be loaded

```C#
var context = new SchoolContext();

var studentWithGrade = context.Students
                           .Where(s => s.FirstName == "Dave")
                           .Include(s => s.Grade)
                           .FirstOrDefault();
```

<br />

Specify a Property to load

```C#
var context = new SchoolContext();

var studentWithGrade = context.Students
                        .Where(s => s.FirstName == "Bill")
                        .Include("Grade")
                        .FirstOrDefault();
```

<br />

Use a SQL String

```C#
var context = new SchoolContext();

var studentWithGrade = context.Students
                        .FromSql("Select * from Students where FirstName ='Bill'")
                        .Include(s => s.Grade)
                        .FirstOrDefault();  
```

<br />

Multiple Include to load multiple properties of the same entity.

```C#
var context = new SchoolContext();

var studentWithGrade = context.Students.Where(s => s.FirstName == "Bill")
                        .Include(s => s.Grade)
                        .Include(s => s.StudentCourses)
                        .FirstOrDefault();
```

<br />

ThenInclude to load multiple levels of related entities.

```C#
var context = new SchoolContext();

var student = context.Students.Where(s => s.FirstName == "Bill")
                        .Include(s => s.Grade)
                            .ThenInclude(g => g.Teachers)
                        .FirstOrDefault();
```


<br />

Projection Query to load multiple related entities (Result is same as ThenInclude)

```C#
var context = new SchoolContext();

var stud = context.Students.Where(s => s.FirstName == "Bill")
                        .Select(s => new
                        {
                            Student = s,
                            Grade = s.Grade,
                            GradeTeachers = s.Grade.Teachers
                        })
                        .FirstOrDefault();
```

<br />
<br />

### Saving Data in Connected Scenario

Insert Data 

<br />

Create object, Add() to context and SaveChanges() to database

```C#
using (var context = new SchoolContext())
{
    var std = new Student()
    {
        FirstName = "Bill",
        LastName = "Gates"
    };
    context.Students.Add(std);

    // or
    // context.Add<Student>(std);

    context.SaveChanges();
}
```

<br />

Updating Data 

<br />

Retrieve object, update object and SaveChanges() to database. NB: Only propeties 
whose **EntityState** is **Modified** shall be updated in the Database, the rest are ignored.

```C#
using (var context = new SchoolContext())
{
    var std = context.Students.First<Student>(); 
    std.FirstName = "Steve";
    context.SaveChanges();
}
```

<br />

Deleting Data 

<br />

Retrieve object, remove object and SaveChanges() to database. 

```C#
using (var context = new SchoolContext())
{
    var std = context.Students.First<Student>();
    context.Students.Remove(std);

    // or
    // context.Remove<Student>(std);

    context.SaveChanges();
}
```

<br />
<br />

### One to Many Relationship

Convention 1: Add a **reference** navigation property to Student class referring to the Grade class.

```C#
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
   
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
}
```

<br />

Convention 2: Add a **collection** navigation property to Grade class referring to the Student class.

```C#
public class Student
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }

    public ICollection<Student> Students { get; set; } 
}
```

<br />

Convention 3: Add a **reference** and a **collection** navigation property. (Apply convention 1 & 2)

```c#
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeID { get; set; }
    public string GradeName { get; set; }
    
    public ICollection<Student> Students { get; set; }
}
```

<br />


Convention 3: Add a **reference** and a **collection** navigation property. (Apply convention 1 & 2)

```c#
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeID { get; set; }
    public string GradeName { get; set; }
    
    public ICollection<Student> Students { get; set; }
}
```

<br />


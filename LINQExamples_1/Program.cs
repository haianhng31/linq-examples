using System; 
using System.Collections.Generic;
using System.Linq;

namespace LINQExamples_1 
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employeeList = Data.GetEmployees();
            List<Department> departmentList = Data.GetDepartments();


            // METHOD SYNTAX 
            // var results = employeeList
            // .Select(e => new {
            //     FullName = e.FirstName + " " + e.LastName,
            //     AnnualSalary = e.AnnualSalary,

            // }).Where(e => e.AnnualSalary >= 50000);


            // QUERY SYNTAX 
            // var results = from e in employeeList
            //               join dep in departmentList
            //               on e.DepartmentId equals dep.Id
            //               where e.AnnualSalary >= 50000
            //               select new {
            //                   FullName = e.FirstName + " " + e.LastName,
            //                   AnnualSalary = e.AnnualSalary,
            //                   DepartmentName = dep.LongName
            //               };


            // USE EXTENSION 
            // 1. DEFERRED EXECUTION 
            // var results = from emp in employeeList.GetHighSalariedEmployees()
            //               select new 
            //               { 
            //                 FullName = emp.FirstName + " " + emp.LastName,
            //                 AnnualSalary = emp.AnnualSalary,
            //               };


            // employeeList.Add(new Employee {
            //     Id = 5, FirstName = "Sam", LastName = "David", AnnualSalary = 100000.20m, IsManager = true, DepartmentId = 2
            // });


            // USE EXTENSION 
            // 2. IMMEDIATE EXECUTION 
            // var results = (from emp in employeeList.GetHighSalariedEmployees()
            //               select new 
            //               { 
            //                 FullName = emp.FirstName + " " + emp.LastName,
            //                 AnnualSalary = emp.AnnualSalary,
            //               }).ToList(); 

            // employeeList.Add(new Employee {
            //     Id = 5, FirstName = "Sam", LastName = "David", AnnualSalary = 100000.20m, IsManager = true, DepartmentId = 2
            // });


            // JOIN OPERATOR: METHOD SYNTAX 
            // var results = departmentList.Join(
            //     employeeList, 
            //     department => department.Id, 
            //     employee => employee.DepartmentId,
            //     // we can define an anonymous type where we can shape the data for each item
            //     // in an iEnumerable collection that we want returned from the relevant query 
            //     (department, employee) => new {
            //         FullName = employee.FirstName + " " + employee.LastName,
            //         AnnualSalary = employee.AnnualSalary,
            //         DepartmentName = department.LongName
            //     }
            // );

            // JOIN OPERATOR: QUERY SYNTAX
            // var results = from employee in employeeList 
            //               join department in departmentList
            //               on employee.DepartmentId equals department.Id
            //               select new {
            //                   FullName = employee.FirstName + " " + employee.LastName,
            //                   AnnualSalary = employee.AnnualSalary,
            //                   DepartmentName = department.LongName
            //               };


            // GROUP JOIN: METHOD SYNTAX - left outer join 
            // var results = departmentList.GroupJoin(
            //     employeeList, 
            //     department => department.Id, 
            //     employee => employee.DepartmentId,
            //     (department, employeeGroup) => new {
            //         Employees = employeeGroup,
            //         DepartmentName = department.LongName
            //     }
            // );


            // GROUP JOIN: QUERY SYNTAX 
            var results = from dep in departmentList
                          join emp in employeeList
                          on dep.Id equals emp.DepartmentId
                          into employeeGroup 
                          select new {
                            Employees = employeeGroup,
                            DepartmentName = dep.LongName
                          };

            foreach (var item in results) {
                Console.WriteLine($"{item.DepartmentName, -20}");
                foreach (var e in item.Employees) {
                    Console.WriteLine($"{e.FirstName + " " + e.LastName, -20} {e.AnnualSalary, 10}");
                }
            }

            Console.ReadKey();

        }
    }

    public static class EnumerableCollectionExtensionMethods 
    {
        static public IEnumerable<Employee> GetHighSalariedEmployees(this IEnumerable<Employee> employees)
        {
            foreach (Employee emp in employees) {
                Console.WriteLine($"Accessing employee: {emp.FirstName + " " + emp.LastName}");
                if (emp.AnnualSalary >= 50000) {
                    yield return emp;
                    // yield: return each element one at a time 
                }
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AnnualSalary { get; set; }
        public bool IsManager { get; set; }
        public int DepartmentId { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

    public static class Data
    {
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            Employee employee = new Employee
            {
                Id = 1,
                FirstName = "Bob",
                LastName = "Jones",
                AnnualSalary = 60000.3m,
                IsManager = true,
                DepartmentId = 6
            };
            employees.Add(employee);
            employee = new Employee
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Jameson",
                AnnualSalary = 80000.1m,
                IsManager = true,
                DepartmentId = 2
            };
            employees.Add(employee);
            employee = new Employee
            {
                Id = 3,
                FirstName = "Douglas",
                LastName = "Roberts",
                AnnualSalary = 40000.2m,
                IsManager = false,
                DepartmentId = 2
            };
            employees.Add(employee);
            employee = new Employee
            {
                Id = 4,
                FirstName = "Jane",
                LastName = "Stevens",
                AnnualSalary = 30000.2m,
                IsManager = false,
                DepartmentId = 2
            };
            employees.Add(employee);

            return employees;
        }

        public static List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            Department department = new Department
            {
                Id = 1,
                ShortName = "HR",
                LongName = "Human Resources"
            };
            departments.Add(department);
            department = new Department
            {
                Id = 2,
                ShortName = "FN",
                LongName = "Finance"
            };
            departments.Add(department);
            department = new Department
            {
                Id = 3,
                ShortName = "TE",
                LongName = "Technology"
            };
            departments.Add(department);

            return departments;
        }

    }
}
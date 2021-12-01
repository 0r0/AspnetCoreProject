using AspnetCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Seeder
{
    public class EmployeeSeeder
    {
        private  EmployeeProjectContext ContextForSeeding;
        public EmployeeSeeder(EmployeeProjectContext employeeProjectContext)
        {
            ContextForSeeding = employeeProjectContext;
        }
        public  void Seed(int NumberOfSeed)
        {
            for(int i=0;i<NumberOfSeed;i++)
            {
                Employee employee = new Employee();
                employee.FirstName = Faker.Name.First();
                employee.LastName = Faker.Name.Last();
                employee.Age =  new Random().Next(14, 100);
                employee.Email = Faker.Internet.Email();
                employee.Created = DateTime.Now;
                ContextForSeeding.Employees.Add(employee);

            }
            ContextForSeeding.SaveChanges();
        }
    }
}

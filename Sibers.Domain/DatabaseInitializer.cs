using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Sibers.Entities.Enums;
using Sibers.Entities.Models;
using Sibers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Domain
{
    public static class DatabaseInitializer
    {
        private static Faker<Employee> ruleForEmployee = new Faker<Employee>()
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.Patronymic, f => f.Name.LastName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Role, f => f.Random.Enum<Role>());

        private static Faker<Company> ruleForCompany = new Faker<Company>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName());

        private static Faker<Project> ruleForProject = new Faker<Project>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.StartTime, f => f.Date.Past())
            .RuleFor(c => c.EndTime, f => f.Date.Future())
            .RuleFor(c => c.Priority, f => f.Random.Int(0, 10));

        private static Faker<Goal> ruleForGoal = new Faker<Goal>()
            .RuleFor(c => c.Name, f => f.Lorem.Word())
            .RuleFor(g => g.Priority, f => f.Random.Int(0, 10))
            .RuleFor(g => g.Status, f => f.Random.Enum<GoalStatus>());

        public static async Task Seed(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SibersContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Company.AddRangeAsync(ruleForCompany.Generate(500));
            await context.Employee.AddRangeAsync(ruleForEmployee.Generate(500));
            await context.Project.AddRangeAsync(ruleForProject.Generate(100));
            await context.Goal.AddRangeAsync(ruleForGoal.Generate(100));

            await context.SaveChangesAsync();

            foreach (var project in context.Project)
            {
                var toSkip = Random.Shared.Next(0, context.Company.Count());
                var client = context.Company.Skip(toSkip).Take(1).First();

                toSkip = Random.Shared.Next(0, context.Company.Count() - 1);
                var provider = context.Company.Where(c => !c.Id.Equals(client.Id)).Skip(toSkip).Take(1).First();

                toSkip = Random.Shared.Next(0, context.Employee.Count(e => e.Role.Equals(Role.Supervisor)));
                var supervisor = context.Employee.Where(e => e.Role.Equals(Role.Supervisor)).Skip(toSkip).Take(1).First();

                toSkip = Random.Shared.Next(0, context.Employee.Count(e => e.Role.Equals(Role.Manager)));
                var manager = context.Employee.Where(e => e.Role.Equals(Role.Manager)).Skip(toSkip).Take(1).First();

                var count = Random.Shared.Next(-4, 10);
                if (count <= 0) goto SAVE_CHANGES;

                toSkip = Random.Shared.Next(0, context.Employee.Count(e => e.Role.Equals(Role.Employee)));
                var employees = context.Employee.Where(e => e.Role.Equals(Role.Employee)).Skip(toSkip).Take(count).ToList();
                project.Employees = employees;

            SAVE_CHANGES:
                project.Client = client;
                project.Provider = provider;
                project.Supervisor = supervisor;
                project.Manager = manager;

                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }
    }
}

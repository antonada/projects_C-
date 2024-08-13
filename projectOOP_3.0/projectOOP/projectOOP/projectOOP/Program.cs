using System;
using System.Collections.Generic;

namespace projectOOP
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string TeamLead { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
    }

    public class ProjectManagementSystem
    {
        private List<Project> projects = new List<Project>();
        private List<Team> teams = new List<Team>();
        private List<Customer> customers = new List<Customer>();

        public void AddProject(Project project)
        {
            project.ProjectId = projects.Count + 1;
            projects.Add(project);
            Console.WriteLine("Project added successfully.");
        }

        public void EditProject(int projectId, Project updatedProject)
        {
            var existingProject = projects.Find(p => p.ProjectId == projectId);
            if (existingProject != null)
            {
                existingProject.Name = updatedProject.Name;
                existingProject.Description = updatedProject.Description;
                Console.WriteLine("Project updated successfully.");
            }
            else
            {
                Console.WriteLine("Project not found.");
            }
        }

        public void DeleteProject(int projectId)
        {
            projects.RemoveAll(p => p.ProjectId == projectId);
            Console.WriteLine("Project deleted successfully.");
        }

        public void AddTeam(Team team)
        {
            team.TeamId = teams.Count + 1;
            teams.Add(team);
            Console.WriteLine("Team added successfully.");
        }

        public void EditTeam(int teamId, Team updatedTeam)
        {
            var existingTeam = teams.Find(t => t.TeamId == teamId);
            if (existingTeam != null)
            {
                existingTeam.Name = updatedTeam.Name;
                existingTeam.TeamLead = updatedTeam.TeamLead;
                Console.WriteLine("Team updated successfully.");
            }
            else
            {
                Console.WriteLine("Team not found.");
            }
        }

        public void DeleteTeam(int teamId)
        {
            teams.RemoveAll(t => t.TeamId == teamId);
            Console.WriteLine("Team deleted successfully.");
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerId = customers.Count + 1;
            customers.Add(customer);
            Console.WriteLine("Customer added successfully.");
        }

        public void EditCustomer(int customerId, Customer updatedCustomer)
        {
            var existingCustomer = customers.Find(c => c.CustomerId == customerId);
            if (existingCustomer != null)
            {
                existingCustomer.Name = updatedCustomer.Name;
                existingCustomer.ContactEmail = updatedCustomer.ContactEmail;
                Console.WriteLine("Customer updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public void DeleteCustomer(int customerId)
        {
            customers.RemoveAll(c => c.CustomerId == customerId);
            Console.WriteLine("Customer deleted successfully.");
        }

        public void DisplayProjects()
        {
            Console.WriteLine("Projects:");
            foreach (var project in projects)
            {
                Console.WriteLine($"Project ID: {project.ProjectId}, Name: {project.Name}, Description: {project.Description}");
            }
        }

        public void DisplayTeams()
        {
            Console.WriteLine("Teams:");
            foreach (var team in teams)
            {
                Console.WriteLine($"Team ID: {team.TeamId}, Name: {team.Name}, Team Lead: {team.TeamLead}");
            }
        }

        public void DisplayCustomers()
        {
            Console.WriteLine("Customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer ID: {customer.CustomerId}, Name: {customer.Name}, Contact Email: {customer.ContactEmail}");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var projectManagementSystem = new ProjectManagementSystem();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add Project");
                Console.WriteLine("2. Edit Project");
                Console.WriteLine("3. Delete Project");
                Console.WriteLine("4. Add Team");
                Console.WriteLine("5. Edit Team");
                Console.WriteLine("6. Delete Team");
                Console.WriteLine("7. Add Customer");
                Console.WriteLine("8. Edit Customer");
                Console.WriteLine("9. Delete Customer");
                Console.WriteLine("10. Display Projects");
                Console.WriteLine("11. Display Teams");
                Console.WriteLine("12. Display Customers");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        projectManagementSystem.AddProject(new Project { Name = "New Project", Description = "Project Description" });
                        break;

                    case "2":
                        Console.Write("Enter Project ID to edit: ");
                        var projectIdToEdit = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Project Name: ");
                        var newProjectName = Console.ReadLine();
                        Console.Write("Enter new Project Description: ");
                        var newProjectDescription = Console.ReadLine();
                        projectManagementSystem.EditProject(projectIdToEdit, new Project { Name = newProjectName, Description = newProjectDescription });
                        break;

                    case "3":
                        Console.Write("Enter Project ID to delete: ");
                        var projectIdToDelete = int.Parse(Console.ReadLine());
                        projectManagementSystem.DeleteProject(projectIdToDelete);
                        break;

                    case "4":
                        projectManagementSystem.AddTeam(new Team { Name = "New Team", TeamLead = "Team Lead" });
                        break;

                    case "5":
                        Console.Write("Enter Team ID to edit: ");
                        var teamIdToEdit = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Team Name: ");
                        var newTeamName = Console.ReadLine();
                        Console.Write("Enter new Team Lead: ");
                        var newTeamLead = Console.ReadLine();
                        projectManagementSystem.EditTeam(teamIdToEdit, new Team { Name = newTeamName, TeamLead = newTeamLead });
                        break;

                    case "6":
                        Console.Write("Enter Team ID to delete: ");
                        var teamIdToDelete = int.Parse(Console.ReadLine());
                        projectManagementSystem.DeleteTeam(teamIdToDelete);
                        break;

                    case "7":
                        projectManagementSystem.AddCustomer(new Customer { Name = "New Customer", ContactEmail = "customer@example.com" });
                        break;

                    case "8":
                        Console.Write("Enter Customer ID to edit: ");
                        var customerIdToEdit = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Customer Name: ");
                        var newCustomerName = Console.ReadLine();
                        Console.Write("Enter new Contact Email: ");
                        var newContactEmail = Console.ReadLine();
                        projectManagementSystem.EditCustomer(customerIdToEdit, new Customer { Name = newCustomerName, ContactEmail = newContactEmail });
                        break;

                    case "9":
                        Console.Write("Enter Customer ID to delete: ");
                        var customerIdToDelete = int.Parse(Console.ReadLine());
                        projectManagementSystem.DeleteCustomer(customerIdToDelete);
                        break;

                    case "10":
                        projectManagementSystem.DisplayProjects();
                        break;

                    case "11":
                        projectManagementSystem.DisplayTeams();
                        break;

                    case "12":
                        projectManagementSystem.DisplayCustomers();
                        break;

                    case "0":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
    }
}

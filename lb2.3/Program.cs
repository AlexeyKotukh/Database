using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CharityFund
{
    public class Donor
    {
        public int DonorID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Donation> Donations { get; set; }
    }

    public class Donation
    {
        public int DonationID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }

        public int DonorID { get; set; }
        public Donor Donor { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }

    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public decimal GoalAmount { get; set; }

        public ICollection<Donation> Donations { get; set; }
        public ICollection<VolunteerProject> VolunteerProjects { get; set; }
    }

    public class Volunteer
    {
        public int VolunteerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<VolunteerProject> VolunteerProjects { get; set; }
    }

    public class VolunteerProject
    {
        public int VolunteerProjectID { get; set; }
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int HoursWorked { get; set; }
    }

    public class CharityFundContext : DbContext
    {
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerProject> VolunteerProjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-1MKQH9T\\SQLEXPRESS;Initial Catalog=CharityFundDB;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donor>()
                .HasMany(d => d.Donations)
                .WithOne(dn => dn.Donor)
                .HasForeignKey(dn => dn.DonorID)
                .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення для пожертв

            modelBuilder.Entity<Project>()
                .HasMany(p => p.VolunteerProjects)
                .WithOne(vp => vp.Project)
                .HasForeignKey(vp => vp.ProjectID)
                .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення для проектів волонтерів
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CharityFundContext())
            {
                context.Database.EnsureCreated();

                while (true)
                {
                    Console.WriteLine("Select an option from the list below (enter 'exit' to close):");
                    Console.WriteLine("0 - Get all donors");
                    Console.WriteLine("1 - Get all donations");
                    Console.WriteLine("2 - Get all projects");
                    Console.WriteLine("3 - Get all volunteers");
                    Console.WriteLine("4 - Add a donor");
                    Console.WriteLine("5 - Add a donation");
                    Console.WriteLine("6 - Add a project");
                    Console.WriteLine("7 - Add a volunteer");
                    Console.WriteLine("8 - Aggregate donations");
                    Console.WriteLine("9 - Update a donor");
                    Console.WriteLine("10 - Update a donation");
                    Console.WriteLine("11 - Update a project");
                    Console.WriteLine("12 - Update a volunteer");
                    Console.WriteLine("13 - Delete a donor");
                    Console.WriteLine("14 - Delete a donation");
                    Console.WriteLine("15 - Delete a project");
                    Console.WriteLine("16 - Delete a volunteer");

                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit") break;

                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number or 'exit'.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 0:
                            GetAllDonors(context);
                            break;
                        case 1:
                            GetAllDonations(context);
                            break;
                        case 2:
                            GetAllProjects(context);
                            break;
                        case 3:
                            GetAllVolunteers(context);
                            break;
                        case 4:
                            AddDonor(context);
                            break;
                        case 5:
                            AddDonation(context);
                            break;
                        case 6:
                            AddProject(context);
                            break;
                        case 7:
                            AddVolunteer(context);
                            break;
                        case 8:
                            AggregateDonations(context);
                            break;
                        case 9:
                            UpdateDonor(context);
                            break;
                        case 10:
                            UpdateDonation(context);
                            break;
                        case 11:
                            UpdateProject(context);
                            break;
                        case 12:
                            UpdateVolunteer(context);
                            break;
                        case 13:
                            DeleteDonor(context);
                            break;
                        case 14:
                            DeleteDonation(context);
                            break;
                        case 15:
                            DeleteProject(context);
                            break;
                        case 16:
                            DeleteVolunteer(context);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please select a valid number.");
                            break;
                    }
                    Console.WriteLine();
                }
            }
        }

        static void GetAllDonors(CharityFundContext context)
        {
            var donors = context.Donors.ToList();
            Console.WriteLine("List of Donors:");
            foreach (var donor in donors)
            {
                Console.WriteLine($"ID: {donor.DonorID}, Name: {donor.Name}, Email: {donor.Email}, Phone: {donor.Phone}");
            }
        }

        static void GetAllDonations(CharityFundContext context)
        {
            var donations = context.Donations.Include(d => d.Donor).Include(d => d.Project).ToList();
            Console.WriteLine("List of Donations:");
            foreach (var donation in donations)
            {
                Console.WriteLine($"ID: {donation.DonationID}, Amount: {donation.Amount}, Date: {donation.DonationDate}, Donor: {donation.Donor.Name}, Project: {donation.Project.ProjectName}");
            }
        }

        static void GetAllProjects(CharityFundContext context)
        {
            var projects = context.Projects.ToList();
            Console.WriteLine("List of Projects:");
            foreach (var project in projects)
            {
                Console.WriteLine($"ID: {project.ProjectID}, Name: {project.ProjectName}, Description: {project.Description}, Goal Amount: {project.GoalAmount}");
            }
        }

        static void GetAllVolunteers(CharityFundContext context)
        {
            var volunteers = context.Volunteers.ToList();
            Console.WriteLine("List of Volunteers:");
            foreach (var volunteer in volunteers)
            {
                Console.WriteLine($"ID: {volunteer.VolunteerID}, Name: {volunteer.Name}, Email: {volunteer.Email}, Phone: {volunteer.Phone}");
            }
        }

        static void AddDonor(CharityFundContext context)
        {
            Console.WriteLine("Enter Donor Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Donor Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter Donor Phone:");
            string phone = Console.ReadLine();

            var donor = new Donor { Name = name, Email = email, Phone = phone };
            context.Donors.Add(donor);
            context.SaveChanges();
            Console.WriteLine("Donor added successfully!");
        }

        static void AddDonation(CharityFundContext context)
        {
            Console.WriteLine("Enter Donation Amount:");
            decimal amount = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Donor ID:");
            int donorId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Project ID:");
            int projectId = int.Parse(Console.ReadLine());

            var donation = new Donation
            {
                Amount = amount,
                DonationDate = DateTime.Now,
                DonorID = donorId,
                ProjectID = projectId
            };

            try
            {
                context.Donations.Add(donation);
                context.SaveChanges();
                Console.WriteLine("Donation added successfully!");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while saving the donation: {ex.InnerException?.Message}");
            }
        }

        static void AddProject(CharityFundContext context)
        {
            Console.WriteLine("Enter Project Name:");
            string projectName = Console.ReadLine();
            Console.WriteLine("Enter Project Description:");
            string description = Console.ReadLine();
            Console.WriteLine("Enter Goal Amount:");
            decimal goalAmount = decimal.Parse(Console.ReadLine());

            var project = new Project { ProjectName = projectName, Description = description, GoalAmount = goalAmount };
            context.Projects.Add(project);
            context.SaveChanges();
            Console.WriteLine("Project added successfully!");
        }

        static void AddVolunteer(CharityFundContext context)
        {
            Console.WriteLine("Enter Volunteer Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Volunteer Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter Volunteer Phone:");
            string phone = Console.ReadLine();

            var volunteer = new Volunteer { Name = name, Email = email, Phone = phone };
            context.Volunteers.Add(volunteer);
            context.SaveChanges();
            Console.WriteLine("Volunteer added successfully!");
        }

        static void AggregateDonations(CharityFundContext context)
        {
            var totalDonations = context.Donations.Sum(d => d.Amount);
            Console.WriteLine($"Total Donations: {totalDonations}");
        }

        static void UpdateDonor(CharityFundContext context)
        {
            Console.WriteLine("Enter Donor ID to update:");
            int donorId = int.Parse(Console.ReadLine());

            var donor = context.Donors.Find(donorId);
            if (donor != null)
            {
                Console.WriteLine("Enter new Name (leave empty to keep current):");
                string name = Console.ReadLine();
                Console.WriteLine("Enter new Email (leave empty to keep current):");
                string email = Console.ReadLine();
                Console.WriteLine("Enter new Phone (leave empty to keep current):");
                string phone = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name)) donor.Name = name;
                if (!string.IsNullOrWhiteSpace(email)) donor.Email = email;
                if (!string.IsNullOrWhiteSpace(phone)) donor.Phone = phone;

                context.SaveChanges();
                Console.WriteLine("Donor updated successfully!");
            }
            else
            {
                Console.WriteLine("Donor not found.");
            }

            DisplayAllTables(context);
        }

        static void UpdateDonation(CharityFundContext context)
        {
            Console.WriteLine("Enter Donation ID to update:");
            int donationId = int.Parse(Console.ReadLine());

            var donation = context.Donations.Find(donationId);
            if (donation != null)
            {
                Console.WriteLine("Enter new Amount (leave empty to keep current):");
                string amountInput = Console.ReadLine();
                if (decimal.TryParse(amountInput, out decimal amount))
                {
                    donation.Amount = amount;
                }

                Console.WriteLine("Enter new Donor ID (leave empty to keep current):");
                string donorIdInput = Console.ReadLine();
                if (int.TryParse(donorIdInput, out int donorId))
                {
                    donation.DonorID = donorId;
                }

                Console.WriteLine("Enter new Project ID (leave empty to keep current):");
                string projectIdInput = Console.ReadLine();
                if (int.TryParse(projectIdInput, out int projectId))
                {
                    donation.ProjectID = projectId;
                }

                context.SaveChanges();
                Console.WriteLine("Donation updated successfully!");
            }
            else
            {
                Console.WriteLine("Donation not found.");
            }

            DisplayAllTables(context);
        }

        static void UpdateProject(CharityFundContext context)
        {
            Console.WriteLine("Enter Project ID to update:");
            int projectId = int.Parse(Console.ReadLine());

            var project = context.Projects.Find(projectId);
            if (project != null)
            {
                Console.WriteLine("Enter new Name (leave empty to keep current):");
                string name = Console.ReadLine();
                Console.WriteLine("Enter new Description (leave empty to keep current):");
                string description = Console.ReadLine();
                Console.WriteLine("Enter new Goal Amount (leave empty to keep current):");
                string goalInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name)) project.ProjectName = name;
                if (!string.IsNullOrWhiteSpace(description)) project.Description = description;
                if (decimal.TryParse(goalInput, out decimal goal)) project.GoalAmount = goal;

                context.SaveChanges();
                Console.WriteLine("Project updated successfully!");
            }
            else
            {
                Console.WriteLine("Project not found.");
            }

            DisplayAllTables(context);
        }

        static void UpdateVolunteer(CharityFundContext context)
        {
            Console.WriteLine("Enter Volunteer ID to update:");
            int volunteerId = int.Parse(Console.ReadLine());

            var volunteer = context.Volunteers.Find(volunteerId);
            if (volunteer != null)
            {
                Console.WriteLine("Enter new Name (leave empty to keep current):");
                string name = Console.ReadLine();
                Console.WriteLine("Enter new Email (leave empty to keep current):");
                string email = Console.ReadLine();
                Console.WriteLine("Enter new Phone (leave empty to keep current):");
                string phone = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name)) volunteer.Name = name;
                if (!string.IsNullOrWhiteSpace(email)) volunteer.Email = email;
                if (!string.IsNullOrWhiteSpace(phone)) volunteer.Phone = phone;

                context.SaveChanges();
                Console.WriteLine("Volunteer updated successfully!");
            }
            else
            {
                Console.WriteLine("Volunteer not found.");
            }

            DisplayAllTables(context);
        }

        static void DeleteDonor(CharityFundContext context)
        {
            Console.WriteLine("Enter Donor ID to delete:");
            int donorId = int.Parse(Console.ReadLine());

            var donor = context.Donors.Find(donorId);
            if (donor != null)
            {
                context.Donors.Remove(donor);
                context.SaveChanges();
                Console.WriteLine("Donor deleted successfully!");
            }
            else
            {
                Console.WriteLine("Donor not found.");
            }

            DisplayAllTables(context);
        }

        static void DeleteDonation(CharityFundContext context)
        {
            Console.WriteLine("Enter Donation ID to delete:");
            int donationId = int.Parse(Console.ReadLine());

            var donation = context.Donations.Find(donationId);
            if (donation != null)
            {
                context.Donations.Remove(donation);
                context.SaveChanges();
                Console.WriteLine("Donation deleted successfully!");
            }
            else
            {
                Console.WriteLine("Donation not found.");
            }

            DisplayAllTables(context);
        }

        static void DeleteProject(CharityFundContext context)
        {
            Console.WriteLine("Enter Project ID to delete:");
            int projectId = int.Parse(Console.ReadLine());

            var project = context.Projects.Find(projectId);
            if (project != null)
            {
                context.Projects.Remove(project);
                context.SaveChanges();
                Console.WriteLine("Project deleted successfully!");
            }
            else
            {
                Console.WriteLine("Project not found.");
            }

            DisplayAllTables(context);
        }

        static void DeleteVolunteer(CharityFundContext context)
        {
            Console.WriteLine("Enter Volunteer ID to delete:");
            int volunteerId = int.Parse(Console.ReadLine());

            var volunteer = context.Volunteers.Find(volunteerId);
            if (volunteer != null)
            {
                context.Volunteers.Remove(volunteer);
                context.SaveChanges();
                Console.WriteLine("Volunteer deleted successfully!");
            }
            else
            {
                Console.WriteLine("Volunteer not found.");
            }

            DisplayAllTables(context);
        }

        static void DisplayAllTables(CharityFundContext context)
        {
            GetAllDonors(context);
            GetAllDonations(context);
            GetAllProjects(context);
            GetAllVolunteers(context);
        }
    }
}

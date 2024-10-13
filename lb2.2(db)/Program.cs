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
                .HasForeignKey(dn => dn.DonorID);

            modelBuilder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.ProjectID);

            modelBuilder.Entity<Project>()
                .Property(p => p.GoalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Project>()
                .HasMany(p => p.VolunteerProjects)
                .WithOne(vp => vp.Project)
                .HasForeignKey(vp => vp.ProjectID);

            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.VolunteerProjects)
                .WithOne(vp => vp.Volunteer)
                .HasForeignKey(vp => vp.VolunteerID);
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

            var project = new Project
            {
                ProjectName = projectName,
                Description = description,
                GoalAmount = goalAmount
            };

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
            var total = context.Donations.Sum(d => d.Amount);
            Console.WriteLine($"Total Donations: {total}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CharityFund
{
    // Existing classes (Donor, Donation, Project, Volunteer, VolunteerProject)

    public class CharityFundContext : DbContext
    {
        // Existing DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Existing configurations

            // Enable cascading deletes for Donations when Donor is deleted
            modelBuilder.Entity<Donor>()
                .HasMany(d => d.Donations)
                .WithOne(dn => dn.Donor)
                .HasForeignKey(dn => dn.DonorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable cascading deletes for VolunteerProjects when Project is deleted
            modelBuilder.Entity<Project>()
                .HasMany(p => p.VolunteerProjects)
                .WithOne(vp => vp.Project)
                .HasForeignKey(vp => vp.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);
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
                    Console.WriteLine("8 - Update a donor");
                    Console.WriteLine("9 - Update a donation");
                    Console.WriteLine("10 - Update a project");
                    Console.WriteLine("11 - Update a volunteer");
                    Console.WriteLine("12 - Delete a donor");
                    Console.WriteLine("13 - Delete a donation");
                    Console.WriteLine("14 - Delete a project");
                    Console.WriteLine("15 - Delete a volunteer");
                    Console.WriteLine("16 - Aggregate donations");

                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit") break;

                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number or 'exit'.");
                        continue;
                    }

                    switch (choice)
                    {
                        // Existing cases

                        case 8:
                            UpdateDonor(context);
                            break;
                        case 9:
                            UpdateDonation(context);
                            break;
                        case 10:
                            UpdateProject(context);
                            break;
                        case 11:
                            UpdateVolunteer(context);
                            break;
                        case 12:
                            DeleteDonor(context);
                            break;
                        case 13:
                            DeleteDonation(context);
                            break;
                        case 14:
                            DeleteProject(context);
                            break;
                        case 15:
                            DeleteVolunteer(context);
                            break;
                        case 16:
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

        // Existing methods for getting all entities

        static void UpdateDonor(CharityFundContext context)
        {
            Console.WriteLine("Enter Donor ID to update:");
            int donorId = int.Parse(Console.ReadLine());
            var donor = context.Donors.Find(donorId);

            if (donor == null)
            {
                Console.WriteLine("Donor not found.");
                return;
            }

            Console.WriteLine("Enter new Name (leave blank to keep current):");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) donor.Name = name;

            Console.WriteLine("Enter new Email (leave blank to keep current):");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) donor.Email = email;

            Console.WriteLine("Enter new Phone (leave blank to keep current):");
            string phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone)) donor.Phone = phone;

            context.SaveChanges();
            Console.WriteLine("Donor updated successfully!");
        }

        static void UpdateDonation(CharityFundContext context)
        {
            Console.WriteLine("Enter Donation ID to update:");
            int donationId = int.Parse(Console.ReadLine());
            var donation = context.Donations.Find(donationId);

            if (donation == null)
            {
                Console.WriteLine("Donation not found.");
                return;
            }

            Console.WriteLine("Enter new Amount (leave blank to keep current):");
            string amountInput = Console.ReadLine();
            if (decimal.TryParse(amountInput, out decimal amount)) donation.Amount = amount;

            Console.WriteLine("Enter new Donor ID (leave blank to keep current):");
            string donorIdInput = Console.ReadLine();
            if (int.TryParse(donorIdInput, out int donorId)) donation.DonorID = donorId;

            Console.WriteLine("Enter new Project ID (leave blank to keep current):");
            string projectIdInput = Console.ReadLine();
            if (int.TryParse(projectIdInput, out int projectId)) donation.ProjectID = projectId;

            context.SaveChanges();
            Console.WriteLine("Donation updated successfully!");
        }

        static void UpdateProject(CharityFundContext context)
        {
            Console.WriteLine("Enter Project ID to update:");
            int projectId = int.Parse(Console.ReadLine());
            var project = context.Projects.Find(projectId);

            if (project == null)
            {
                Console.WriteLine("Project not found.");
                return;
            }

            Console.WriteLine("Enter new Project Name (leave blank to keep current):");
            string projectName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(projectName)) project.ProjectName = projectName;

            Console.WriteLine("Enter new Description (leave blank to keep current):");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description)) project.Description = description;

            Console.WriteLine("Enter new Goal Amount (leave blank to keep current):");
            string goalAmountInput = Console.ReadLine();
            if (decimal.TryParse(goalAmountInput, out decimal goalAmount)) project.GoalAmount = goalAmount;

            context.SaveChanges();
            Console.WriteLine("Project updated successfully!");
        }

        static void UpdateVolunteer(CharityFundContext context)
        {
            Console.WriteLine("Enter Volunteer ID to update:");
            int volunteerId = int.Parse(Console.ReadLine());
            var volunteer = context.Volunteers.Find(volunteerId);

            if (volunteer == null)
            {
                Console.WriteLine("Volunteer not found.");
                return;
            }

            Console.WriteLine("Enter new Name (leave blank to keep current):");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) volunteer.Name = name;

            Console.WriteLine("Enter new Email (leave blank to keep current):");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) volunteer.Email = email;

            Console.WriteLine("Enter new Phone (leave blank to keep current):");
            string phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone)) volunteer.Phone = phone;

            context.SaveChanges();
            Console.WriteLine("Volunteer updated successfully!");
        }

        static void DeleteDonor(CharityFundContext context)
        {
            Console.WriteLine("Enter Donor ID to delete:");
            int donorId = int.Parse(Console.ReadLine());
            var donor = context.Donors.Include(d => d.Donations).FirstOrDefault(d => d.DonorID == donorId);

            if (donor == null)
            {
                Console.WriteLine("Donor not found.");
                return;
            }

            context.Donors.Remove(donor);
            context.SaveChanges();
            Console.WriteLine("Donor deleted successfully!");
            GetAllDonors(context);
            GetAllDonations(context);
        }

        static void DeleteDonation(CharityFundContext context)
        {
            Console.WriteLine("Enter Donation ID to delete:");
            int donationId = int.Parse(Console.ReadLine());
            var donation = context.Donations.Find(donationId);

            if (donation == null)
            {
                Console.WriteLine("Donation not found.");
                return;
            }

            context.Donations.Remove(donation);
            context.SaveChanges();
            Console.WriteLine("Donation deleted successfully!");
            GetAllDonations(context);
        }

        static void DeleteProject(CharityFundContext context)
        {
            Console.WriteLine("Enter Project ID to delete:");
            int projectId = int.Parse(Console.ReadLine());
            var project = context.Projects.Include(p => p.Donations).FirstOrDefault(p => p.ProjectID == projectId);

            if (project == null)
            {
                Console.WriteLine("Project not found.");
                return;
            }

            context.Projects.Remove(project);
            context.SaveChanges();
            Console.WriteLine("Project deleted successfully!");
            GetAllProjects(context);
        }

        static void DeleteVolunteer(CharityFundContext context)
        {
            Console.WriteLine("Enter Volunteer ID to delete:");
            int volunteerId = int.Parse(Console.ReadLine());
            var volunteer = context.Volunteers.Include(v => v.VolunteerProjects).FirstOrDefault(v => v.VolunteerID == volunteerId);

            if (volunteer == null)
            {
                Console.WriteLine("Volunteer not found.");
                return;
            }

            context.Volunteers.Remove(volunteer);
            context.SaveChanges();
            Console.WriteLine("Volunteer deleted successfully!");
            GetAllVolunteers(context);
        }

        static void AggregateDonations(CharityFundContext context)
        {
            var total = context.Donations.Sum(d => d.Amount);
            Console.WriteLine($"Total Donations: {total}");
        }
    }
}

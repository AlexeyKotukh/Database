using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Server=DESKTOP-1MKQH9T\\SQLEXPRESS;Database=CharityDB;Trusted_Connection=True;";

    static void Main()
    {

        DisplayDonors();

        DisplayVolunteers();
        JoinQuery();
        FilterQuery();
        AggregateQuery();
    }

    static void RemoveDuplicateVolunteers()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(
                "WITH CTE AS (" +
                "SELECT VolunteerID, " +
                "ROW_NUMBER() OVER (PARTITION BY FirstName, LastName, Email ORDER BY VolunteerID) AS RowNum " +
                "FROM Volunteers) " +
                "DELETE FROM CTE WHERE RowNum > 1", connection);
            int rowsAffected = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rowsAffected} duplicate volunteer(s) removed.");
        }
    }

    static bool VolunteerExists(string firstName, string lastName, string email)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Volunteers WHERE FirstName = @FirstName AND LastName = @LastName AND Email = @Email", connection);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            return (int)cmd.ExecuteScalar() > 0;
        }
    }

    static void DisplayVolunteers()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Volunteers", connection);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\nVolunteers:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["VolunteerID"]} {reader["FirstName"]} {reader["LastName"]} {reader["Email"]}");
            }
            reader.Close();
        }
    }

    static void AddVolunteer(string firstName, string lastName, string email)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Volunteers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)", connection);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error: " + ex.Message);
        }
    }

    static void DisplayDonors()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Donors", connection);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\nDonors:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["DonorID"]} {reader["FirstName"]} {reader["LastName"]} {reader["Email"]}");
            }
            reader.Close();
        }
    }

    static void JoinQuery()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT V.FirstName, V.LastName, P.ProjectName " +
                "FROM Volunteers V " +
                "JOIN VolunteerProjects VP ON V.VolunteerID = VP.VolunteerID " +
                "JOIN Projects P ON VP.ProjectID = P.ProjectID", connection);

            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("\nVolunteers and their projects:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} is working on {reader["ProjectName"]}");
            }
            reader.Close();
        }
    }

    static void FilterQuery()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT FirstName, LastName FROM Volunteers WHERE LastName LIKE 'P%'", connection);

            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("\nVolunteers with last name starting with 'P':");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}");
            }
            reader.Close();
        }
    }

    static void AggregateQuery()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT SUM(Amount) AS TotalDonations FROM Donations", connection);

            object result = cmd.ExecuteScalar();
            Console.WriteLine($"\nTotal Donations: {result}");
        }
    }
}

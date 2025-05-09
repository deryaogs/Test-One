namespace TavernSystem.Application;
using TavernSystem.Models;
using Microsoft.Data.SqlClient;

public class TavernSystemService : ITavernSystemService
{
    public static void Main()
    {
        
    }
    private string _connectionString;

    public TavernSystemService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Adventurer> GetAdventurers()
    {
        List<Adventurer> adventurers = [];
        const string queryString = "SELECT Id, Nickname FROM Adventurer";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var adventurerRow = new Adventurer
                        {
                            Id = reader.GetInt32(0),
                            Nickname = reader.GetString(1),
                        };
                        adventurers.Add(adventurerRow);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return adventurers;
    }

    public Adventurer? GetAdventurer(int adventurerId)
    {
        Adventurer? adventurer = null;
        string queryString = "SELECT * FROM adventurer WHERE Id = @Id";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", adventurerId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        adventurer = new Adventurer
                        {
                            Id = reader.GetInt32(0),
                            Nickname = reader.GetString(1),
                            RaceId = reader.GetInt32(2),
                            ExperienceId = reader.GetInt32(3),
                            PersonId = reader.GetString(4),
                        };
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return adventurer;
    }

    private int countRowsAdded = -1;
    public bool AddAdventurer(Adventurer adventurer)
    {
        const string insertString =
            "Insert into Adventurer (Id, Nickname,RaceId ,ExperienceId, PersonId) values (@Id, @Nickname, @RaceID, @ExperienceId, @PersonId)";
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);
            command.Parameters.AddWithValue("@Id", adventurer.Id);
            command.Parameters.AddWithValue("@Nickname", adventurer.Nickname);
            command.Parameters.AddWithValue("@RaceId", adventurer.RaceId);
            command.Parameters.AddWithValue("@ExperienceId", adventurer.ExperienceId);
            command.Parameters.AddWithValue("@PersonId", adventurer.PersonId);
            
            connection.Open();
            countRowsAdded = command.ExecuteNonQuery();
        }

        return countRowsAdded != -1;
    }
}
using System.Data.SqlClient;
using Zadanie4.DTOs;
using Zadanie4.Utils;

namespace Zadanie4.DI;

public class DBService : IDBService
{
    public IEnumerable<AnimalDTO> GetRequest(string param = "name")
    {
        List<AnimalDTO> animalsList = new List<AnimalDTO>();
        using (SqlConnection connection =
               new SqlConnection("Data Source=MSI;Initial Catalog=AnimalsDB;Integrated Security=True"))
        {
            connection.Open();
            using SqlCommand command = new SqlCommand($"SELECT * FROM  Animal ORDER BY " + param, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                animalsList.Add(new AnimalDTO
                {
                    IdAnimal = Int32.Parse(reader["IdAnimal"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Category = reader["Category"].ToString(),
                    Area = reader["Area"].ToString()
                });
            }

            reader.Close();
            connection.Close();
        }

        return animalsList;
    }

    public RequestStatus PostRequest(AnimalDTO animal)
    {
        if (animal.Category == null || animal.Name == null || animal.Area == null || animal.IdAnimal == null)
            return RequestStatus.ERROR_PARAMETER_IS_EMPTY;
        bool isAnimalExists = GetRequest().Select(x => x.IdAnimal).Contains(animal.IdAnimal);
        if (isAnimalExists) return RequestStatus.ERROR_OBJECT_ID_ALREADY_EXISTS;
        using SqlConnection connection =
            new SqlConnection("Data Source=MSI;Initial Catalog=AnimalsDB;Integrated Security=True");
        connection.Open();

        using SqlCommand command = new SqlCommand(
            $"set IDENTITY_INSERT Animal  on" +
            $" INSERT INTO Animal(IdAnimal, Name, Description, Category, Area)" +
            $" VALUES (@IdAnimal,@Name, @Description, @Category, @Area)");
        command.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);
        command.Connection = connection;
        command.ExecuteScalar();
        connection.Close();
        return RequestStatus.SUCCESS;
    }

    public string getResponse(RequestStatus requestStatus)
    {
        switch (requestStatus)
        {
            case RequestStatus.ERROR_OBJECT_ID_ALREADY_EXISTS:
                return "Object with provided id already exists";
            case RequestStatus.ERROR_PARAMETER_IS_EMPTY:
                return "Object cannot be initialized with empty parameter";
            case RequestStatus.ERROR_ID_CANNOT_BE_CHANGED:
                return "Objects ID cannot be changed";
            case RequestStatus.ERROR_ORDER_TYPE_NOT_EXISTS:
                return "Order type doesn't exist";
            case RequestStatus.ERROR_OBJECT_NOT_EXISTS:
                return "Object doesn't exist";
        }
        return "Success";
    }

    public RequestStatus DeleteRequest(int idAnimal)
    {
        bool isAnimalExists = GetRequest().Select(x => x.IdAnimal).Contains(idAnimal);
        if (!isAnimalExists) return RequestStatus.ERROR_OBJECT_NOT_EXISTS;
        using SqlConnection connection =
            new SqlConnection("Data Source=MSI;Initial Catalog=AnimalsDB;Integrated Security=True");
        connection.Open();

        using SqlCommand command = new SqlCommand("Delete from Animal where idAnimal = @idAnimal" );
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Connection = connection;
        command.ExecuteScalar();
        connection.Close();
        return RequestStatus.SUCCESS;
    }

    public RequestStatus UpdateRequest(int idAnimal, AnimalDTO animal)
    {
        bool isAnimalExists = GetRequest().Select(x => x.IdAnimal).Contains(idAnimal);
        if (!isAnimalExists) return RequestStatus.ERROR_OBJECT_NOT_EXISTS;
        if (animal.Category == null || animal.Name == null || animal.Area == null)
            return RequestStatus.ERROR_PARAMETER_IS_EMPTY;
        if (animal.IdAnimal != idAnimal)
            return RequestStatus.ERROR_ID_CANNOT_BE_CHANGED;
        using SqlConnection connection =
            new SqlConnection("Data Source=MSI;Initial Catalog=AnimalsDB;Integrated Security=True");
        connection.Open();

        using SqlCommand command = new SqlCommand(
            "Update Animal set Name=@Name,Description = @Description, Category= @Category, Area = @Area Where idAnimal = @IdAnimal");
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);
        command.Connection = connection;
        command.ExecuteReader();
        connection.Close();
        return RequestStatus.SUCCESS;
    }
}
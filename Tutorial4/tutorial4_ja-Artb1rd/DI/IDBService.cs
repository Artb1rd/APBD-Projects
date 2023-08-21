using Zadanie4.DTOs;
using Zadanie4.Utils;

namespace Zadanie4.DI;

public interface IDBService
{
    IEnumerable<AnimalDTO> GetRequest(string param = "name");
    RequestStatus PostRequest(AnimalDTO animal);
    string getResponse(RequestStatus requestStatus);
    public RequestStatus DeleteRequest(int idAnimal);
    public RequestStatus UpdateRequest(int idAnimal, AnimalDTO animal);
}
using Demo.Models;
using System.Data;

namespace Demo.Repository;

public interface IPersonRepo
{
    Task<int> DeletePerson(int id);
    Task<IEnumerable<PersonModel>> GetPeople();
    Task<PersonModel> GetPersonById(int id);
    Task<int> InsertPerson(int id, string? firstName, string? lastName);
    Task<int> UpdatePerson(int id, PersonModel model);

    IDbConnection GetConnection();
}

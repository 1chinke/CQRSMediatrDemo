using Demo.Models;
using System.Data;

namespace Demo.Repository;

public interface IPersonRepo
{
    Task<int> Delete(int id);
    Task<IEnumerable<Person>> GetAll();
    Task<Person> GetById(int id);
    Task<int> Insert(int id, string firstName, string lastName);
    Task<int> Update(int id, Person model);

    IDbConnection GetConnection();
}

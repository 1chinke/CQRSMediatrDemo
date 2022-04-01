using Dapper;
using Demo.Models;
using System.Data;


namespace Demo.Repository;

public class PersonRepo : IPersonRepo
{
    private readonly IDbConnection _conn;

    public PersonRepo()
    {
        _conn = ConnectionManager.GetInstance().GetConnection();
    }


    public async Task<IEnumerable<PersonModel>> GetPeople()
    {
        string query = "Select * from person order by id";
        return await _conn.QueryAsync<PersonModel>(query);
    }

    public async Task<PersonModel> GetPersonById(int id)
    {
        string query = "Select * from person where id = :id";

        var parameters = new { id };

        return await _conn.QueryFirstOrDefaultAsync<PersonModel>(query, param: parameters);
    }

    public async Task<int> InsertPerson(int id, string? firstName, string? lastName)
    {
        string query = "Insert Into person values(:id, :firstName, :lastName)";

        var parameters = new { id, firstName, lastName };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> DeletePerson(int id)
    {
        string query = "Delete From person where id = :id";
        var parameters = new { id };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> UpdatePerson(int id, PersonModel model)
    {
        string query = "Update person set " +
            "id = :id, " +
            "first_name = :firstName, " +
            "last_name = :lastName " +
            "Where id = :oldId";
        var parameters = new { id = model.Id, firstName = model.FirstName, lastName = model.LastName, oldId = id };


        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public IDbConnection GetConnection() => _conn;

}

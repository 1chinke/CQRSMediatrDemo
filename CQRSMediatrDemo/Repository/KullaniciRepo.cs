using Dapper;
using Demo.Models;
using System.Data;

namespace Demo.Repository;

public class KullaniciRepo : IKullaniciRepo
{
    private readonly IDbConnection _conn;

    public KullaniciRepo()
    {
        _conn = ConnectionManager.GetInstance().GetConnection();
    }

    public async Task<int> Delete(string username)
    {
        string query = "Delete From kullanici where username = :username";
        var parameters = new { username };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<IEnumerable<Kullanici>> GetAll()
    {
        string query = "Select * from kullanici order by username";
        return await _conn.QueryAsync<Kullanici>(query);
    }

    public async Task<Kullanici> GetByUsername(string username)
    {
        string query = "Select * from kullanici where username = :username";

        var parameters = new { username };

        return await _conn.QueryFirstOrDefaultAsync<Kullanici>(query, param: parameters);
    }

    public async Task<Kullanici> GetByUsernameAndPassword(string username, string password)
    {
        string query = @"Select * from kullanici 
                         Where username = :username and 
                               password = :password";

        var parameters = new { username, password };

        return await _conn.QueryFirstOrDefaultAsync<Kullanici>(query, param: parameters);
    }

    public async Task<int> Insert(Kullanici model)
    {
        string query = "Insert Into kullanici values(:username ,:password, :emailAddress, :firstName, :lastName, :role)";

        var parameters = new { model.Username,model.Password, model.EmailAddress, model.FirstName, model.LastName, model.Role };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Update(string username, Kullanici model)
    {
        string query = @"Update kullanici set
            username = :username,
            password = :password,
            email_address = :emailAddress,
            first_name = :firstName,
            last_name = :lastName,
            role = :role
            Where username = :oldUsername";
        var parameters = new { model.Username, model.Password, model.EmailAddress, model.FirstName, model.LastName, model.Role, username };


        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public IDbConnection GetConnection() => _conn;
}

using Demo.Models;
using System.Data;

namespace Demo.Repository;

public interface IKullaniciRepo
{
    Task<int> Delete(string username);
    Task<IEnumerable<Kullanici>> GetAll();
    Task<Kullanici> GetByUsername(String username);
    Task<int> Insert(Kullanici model);
    Task<int> Update(string username, Kullanici model);

    IDbConnection GetConnection();
}

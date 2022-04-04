using Demo.Mediatr.Commands.KullaniciCommands;
using Demo.Repository;
using Demo.Responses;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class InsertKullaniciHnd : IRequestHandler<InsertKullanici, GenericResponse>
{
    private readonly IKullaniciRepo _repo;
    private readonly IConfiguration _config;

    public InsertKullaniciHnd(IKullaniciRepo repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<GenericResponse> Handle(InsertKullanici request, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _repo.GetConnection().BeginTransaction();
            try
            {

                request.Model.Password = CreatePasswordHash(request.Model.Password);

                var result = await _repo.Insert(request.Model);
                transaction.Commit();

                if (result == 0)
                {
                    return new GenericResponse(StatusCode: 400, Error: "Kaydedilemedi.");
                }


                return new GenericResponse("Başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: 400, Error: ex.Message);
            }
        }
        catch (Exception ex)
        {
            return new GenericResponse(StatusCode: 400, Error: ex.Message);
        }

    }

    private string CreatePasswordHash(string password)
    {        
        using var hmac = new HMACSHA512();
        hmac.Key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);


        //return Encoding.UTF8.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}

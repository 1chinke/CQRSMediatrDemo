using Demo.Mediatr.Commands.KullaniciCommands;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class InsertKullaniciHnd : IRequestHandler<InsertKullanici, GenericResponse>
{
    private readonly IKullaniciRepo _repo;

    public InsertKullaniciHnd(IKullaniciRepo repo)
    {
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(InsertKullanici request, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _repo.GetConnection().BeginTransaction();
            try
            {
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
}

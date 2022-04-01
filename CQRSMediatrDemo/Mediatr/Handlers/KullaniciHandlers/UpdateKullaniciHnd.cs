using Demo.Mediatr.Commands.KullaniciCommands;
using Demo.Repository;
using Demo.Responses;
using MediatR;


namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class UpdateKullanicielHnd : IRequestHandler<UpdateKullanici, GenericResponse>
{

    private readonly IKullaniciRepo _repo;

    public UpdateKullanicielHnd(IKullaniciRepo repo)
    {
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(UpdateKullanici request, CancellationToken cancellationToken)
    {
        using var transaction = _repo.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Update(request.Username, request.Model);

            if (result == 0)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: 404);
            }

            transaction.Commit();

            return new GenericResponse("Güncelleme yapıldı.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new GenericResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}

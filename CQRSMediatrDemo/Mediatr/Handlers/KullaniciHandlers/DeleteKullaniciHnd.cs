using Demo.Mediatr.Commands.KullaniciCommands;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class DeleteKullaniciHnd : IRequestHandler<DeleteKullanici, GenericResponse>
{

    private readonly IKullaniciRepo _repo;

    public DeleteKullaniciHnd(IKullaniciRepo repo)
    {
        _repo = repo;

    }

    public async Task<GenericResponse> Handle(DeleteKullanici request, CancellationToken cancellationToken)
    {
        using var transaction = _repo.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Delete(request.Username);
            transaction.Commit();

            if (result == 0)
            {
                return new GenericResponse(StatusCode: 400, Error: "Kayıt silinemedi.");
            }

            return new GenericResponse("Kayıt başarıyla silindi.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new GenericResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}

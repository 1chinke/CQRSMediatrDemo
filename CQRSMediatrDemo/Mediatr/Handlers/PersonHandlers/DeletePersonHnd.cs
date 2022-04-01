using Demo.Mediatr.Commands.PersonCommands;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers.PersonHandlers;

public class DeletePersonHnd : IRequestHandler<DeletePerson, GenericResponse>
{

    private readonly IPersonRepo _repo;

    public DeletePersonHnd(IPersonRepo repo)
    {
        _repo = repo;

    }

    public async Task<GenericResponse> Handle(DeletePerson request, CancellationToken cancellationToken)
    {
        using var transaction = _repo.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Delete(request.Id);
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

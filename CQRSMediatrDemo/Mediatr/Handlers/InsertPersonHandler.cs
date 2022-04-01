using Demo.Mediatr.Commands;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers;

public class InsertPersonHandler : IRequestHandler<InsertPersonCommand, GenericResponse>
{
    private readonly IPersonRepo _repo;

    public InsertPersonHandler(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(InsertPersonCommand request, CancellationToken cancellationToken)
    { 
        try
        {
            /*var oldPerson = await _repo.GetPersonById(request.Id);

            if (oldPerson != null)
            {
                return new GenericResponse(StatusCode: 400, Error: "Bu kayıt zaten tanımlı.");
            }*/

            using var transaction = _repo.GetConnection().BeginTransaction();
            try
            {
                var result = await _repo.InsertPerson(request.Id, request.FirstName, request.LastName);
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

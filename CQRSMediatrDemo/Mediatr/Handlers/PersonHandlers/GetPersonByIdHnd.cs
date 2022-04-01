using MediatR;
using Demo.Mediatr.Queries.PersonQueries;
using Demo.Repository;
using Demo.Responses;

namespace Demo.Mediatr.Handlers.PersonHandlers;

public class GetPersonByIdHnd : IRequestHandler<GetPersonById, PersonResponse>
{
    private readonly IPersonRepo _repo;

    public GetPersonByIdHnd(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<PersonResponse> Handle(GetPersonById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetById(request.Id);

            if (result == null)
            {
                return new PersonResponse(StatusCode: 404);
            }

            return new PersonResponse(result);
        } catch (Exception ex)
        {
            return new PersonResponse(StatusCode: 400, Error: ex.Message);
        }
        
    }
}

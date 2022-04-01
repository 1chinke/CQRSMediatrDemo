using Demo.Mediatr.Queries.PersonQueries;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers.PersonHandlers;

public class GetPeopleHnd : IRequestHandler<GetPeople, PeopleResponse>
{

    private readonly IPersonRepo _repo;

    public GetPeopleHnd(IPersonRepo repo)
    {
        _repo = repo;

    }

    public async Task<PeopleResponse> Handle(GetPeople request, CancellationToken cancellationToken)
    {
       
        try
        {
            var result = await _repo.GetAll();
            return new PeopleResponse(result);
        }
        catch (Exception ex)
        {
            return new PeopleResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}

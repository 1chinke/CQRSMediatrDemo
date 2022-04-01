using Demo.Mediatr.Queries;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers;

public class GetPeopleHandler : IRequestHandler<GetPeopleQuery, PeopleResponse>
{

    private readonly IPersonRepo _repo;

    public GetPeopleHandler(IPersonRepo repo)
    {
        _repo = repo;

    }

    public async Task<PeopleResponse> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
    {
       
        try
        {
            var result = await _repo.GetPeople();
            return new PeopleResponse(result);
        }
        catch (Exception ex)
        {
            return new PeopleResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}

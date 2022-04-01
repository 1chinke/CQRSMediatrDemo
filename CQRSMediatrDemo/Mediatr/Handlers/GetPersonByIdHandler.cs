using MediatR;
using Demo.Mediatr.Queries;
using Demo.Repository;
using Demo.Responses;

namespace Demo.Mediatr.Handlers;

public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonResponse>
{
    private readonly IPersonRepo _repo;

    public GetPersonByIdHandler(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<PersonResponse> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetPersonById(request.Id);

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

using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Repository;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class GetPeopleHnd : IRequestHandler<GetAllKullanici, KullanicilarResponse>
{

    private readonly IKullaniciRepo _repo;

    public GetPeopleHnd(IKullaniciRepo repo)
    {
        _repo = repo;

    }

    public async Task<KullanicilarResponse> Handle(GetAllKullanici request, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _repo.GetAll();
            return new KullanicilarResponse(result);
        }
        catch (Exception ex)
        {
            return new KullanicilarResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}

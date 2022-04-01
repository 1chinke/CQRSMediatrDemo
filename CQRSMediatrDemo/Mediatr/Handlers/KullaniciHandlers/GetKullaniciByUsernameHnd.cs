using MediatR;
using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Repository;
using Demo.Responses;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class GetKullaniciByUsernameHnd : IRequestHandler<GetKullaniciByUsername, KullaniciResponse>
{
    private readonly IKullaniciRepo _repo;

    public GetKullaniciByUsernameHnd(IKullaniciRepo repo)
    {
        _repo = repo;
    }

    public async Task<KullaniciResponse> Handle(GetKullaniciByUsername request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetByUsername(request.Username);

            if (result == null)
            {
                return new KullaniciResponse(StatusCode: 404);
            }

            return new KullaniciResponse(result);
        }
        catch (Exception ex)
        {
            return new KullaniciResponse(StatusCode: 400, Error: ex.Message);
        }

    }
}

using MediatR;
using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Repository;
using Demo.Responses;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class GetKullaniciByUsernameAndPasswordHnd : IRequestHandler<GetKullaniciByUsernameAndPassword, KullaniciResponse>
{
    private readonly IKullaniciRepo _repo;

    public GetKullaniciByUsernameAndPasswordHnd(IKullaniciRepo repo)
    {
        _repo = repo;
    }

    public async Task<KullaniciResponse> Handle(GetKullaniciByUsernameAndPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetByUsernameAndPassword(request.Username, request.Password);

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


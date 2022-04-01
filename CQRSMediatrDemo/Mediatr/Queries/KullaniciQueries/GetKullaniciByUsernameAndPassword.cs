using MediatR;
using Demo.Responses;

namespace Demo.Mediatr.Queries.KullaniciQueries;

public record GetKullaniciByUsernameAndPassword(string Username, string Password) : IRequest<KullaniciResponse>;


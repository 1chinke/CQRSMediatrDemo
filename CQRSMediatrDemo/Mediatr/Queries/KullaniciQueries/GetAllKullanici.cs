using MediatR;
using Demo.Responses;

namespace Demo.Mediatr.Queries.KullaniciQueries;

public record GetAllKullanici() : IRequest<KullanicilarResponse>;



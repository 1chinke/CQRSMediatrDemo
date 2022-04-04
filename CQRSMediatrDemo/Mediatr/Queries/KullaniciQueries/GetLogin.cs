using MediatR;
using Demo.Responses;

namespace Demo.Mediatr.Queries.KullaniciQueries;

public record GetLogin(string Username, string Password) : IRequest<LoginResponse>;


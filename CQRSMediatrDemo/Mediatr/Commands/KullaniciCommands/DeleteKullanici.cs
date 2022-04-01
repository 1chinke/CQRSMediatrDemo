using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands.KullaniciCommands;

public record DeleteKullanici(string Username) : IRequest<GenericResponse>;


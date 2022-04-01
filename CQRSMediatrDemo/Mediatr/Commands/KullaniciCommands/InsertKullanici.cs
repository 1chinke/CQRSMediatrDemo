using Demo.Models;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands.KullaniciCommands;

public record InsertKullanici(Kullanici Model) : IRequest<GenericResponse>;

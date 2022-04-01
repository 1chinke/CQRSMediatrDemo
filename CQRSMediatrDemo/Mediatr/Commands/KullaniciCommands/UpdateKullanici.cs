using Demo.Models;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands.KullaniciCommands;

public record UpdateKullanici(string Username, Kullanici Model) : IRequest<GenericResponse>;


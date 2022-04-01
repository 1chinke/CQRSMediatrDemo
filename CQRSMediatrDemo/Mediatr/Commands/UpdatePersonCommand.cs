using Demo.Models;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands;

public record UpdatePersonCommand (int Id, PersonModel Model) : IRequest<GenericResponse>;

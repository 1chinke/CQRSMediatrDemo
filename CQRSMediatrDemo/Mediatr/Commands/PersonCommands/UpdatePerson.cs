using Demo.Models;
using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands.PersonCommands;
public record UpdatePerson (int Id, Person Model) : IRequest<GenericResponse>;

using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands.PersonCommands;

public record InsertPerson(int Id, string FirstName, string LastName) : IRequest<GenericResponse>;  

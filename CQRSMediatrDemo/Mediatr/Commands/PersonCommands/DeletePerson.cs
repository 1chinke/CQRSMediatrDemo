using Demo.Responses;
using MediatR;


namespace Demo.Mediatr.Commands.PersonCommands;

public record DeletePerson(int Id) : IRequest<GenericResponse>;  

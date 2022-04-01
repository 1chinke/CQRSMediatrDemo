using Demo.Responses;
using MediatR;


namespace Demo.Mediatr.Commands;

public record DeletePersonCommand(int Id) : IRequest<GenericResponse>;  

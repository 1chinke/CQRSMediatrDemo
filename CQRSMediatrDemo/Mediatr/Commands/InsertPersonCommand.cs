using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Commands;

public record InsertPersonCommand(int Id, string? FirstName, string? LastName) : IRequest<GenericResponse>;  

using MediatR;
using Demo.Responses;

namespace Demo.Mediatr.Queries;

public record GetPersonByIdQuery(int Id) : IRequest<PersonResponse>;

using MediatR;
using Demo.Responses;

namespace Demo.Mediatr.Queries.PersonQueries;

public record GetPersonById(int Id) : IRequest<PersonResponse>;
